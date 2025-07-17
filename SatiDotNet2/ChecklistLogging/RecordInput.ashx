<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.IO
Imports System.Text.Json
Imports SatiDotNet2.Library
Imports System.Data

Public Class StreamData
    Inherits Security
    Implements IHttpHandler, IReadOnlySessionState

    Private Format As New Format()
    Private LogAspx As New LogAspxLibrary()
    Private ActivePm As New ActivePm()
    Private PmInput As New PmInput()

    Private KeyFromQueryString As Integer

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim Res As Dictionary(Of String, Object)
        Dim JsonString As String
        Dim Json As New Dictionary(Of String, Object)
        Dim PhaseController As New PhaseController()

        context.Response.ContentType = "application/json"

        Using reader As New StreamReader(context.Request.InputStream)
            JsonString = reader.ReadToEnd()
        End Using
        Json = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(JsonString)
        KeyFromQueryString = Json("dataId").ToString()

        Try 'in case sql update query in ModifyInput continues to fail or http req body is missing data
            Res = ModifyInput(KeyFromQueryString, Json("labelId").ToString(), Json("value").ToString(), HttpContext.Current.User.Identity.Name.ToString())

            Res("dbUpdateSuccess") = True
        Catch ex As Exception
            Res = New Dictionary(Of String, Object)

            Res("input") = New Dictionary(Of String, String)
            Res("input")("state") = "invalid"
            Res("input")("endUserMessage") = "*ERROR: COULD NOT SAVE. TRY AGAIN*"

            Res("dbUpdateSuccess") = False
        End Try

        'return accurate phaseLevel even when only the dataId is passed to http endpoint
        Res("phaseLevel") = PhaseController.GetPhase(KeyFromQueryString)

        context.Response.Write(JsonSerializer.Serialize(Res))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function ModifyInput(DataKey As String, LabelKey As String, Value As String, SatiUser As String) As Dictionary(Of String, Object)
        Dim InputOfInterestString As String
        Dim InputOfInterest As Dictionary(Of String, String)
        Dim InputsJson As Dictionary(Of String, Dictionary(Of String, String))
        Dim PrevValue As String
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
             {"@DataKey", GetParamVarHash(DataKey, "int")}
        }
        Dim InputInfo As Dictionary(Of String, Object)
        Dim Res As New Dictionary(Of String, Object)

        InputOfInterestString = GetSingleDbField("SELECT Inputs FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@DataKey", QueryConfig, "Inputs")
        InputsJson = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(InputOfInterestString)
        InputOfInterest = InputsJson(LabelKey)
        PrevValue = InputOfInterest("Value")

        'Session("DisplayError") = False 'What is this used for??????

        InputOfInterest("Operator") = SatiUser
        InputOfInterest("Value") = Value
        InputOfInterest("Date") = Format.DateField(System.DateTime.Now.ToString())
        InputsJson(LabelKey) = InputOfInterest

        'in case of db upload failure, closing code below in a try catch block
        'if db update query fails, try it again
        Try
            UploadToDataTable(SatiUser, JsonSerializer.Serialize(InputsJson))
        Catch ex As Exception
            UploadToDataTable(SatiUser, JsonSerializer.Serialize(InputsJson))
        End Try

        InputInfo = GetInputValidity(LabelKey, Value)
        If InputInfo("state") = "outOfScope" Then
            Dim OutOfRangeJson As Dictionary(Of String, Object) = ActivePm.GetOutOfRange(DataKey)
            Dim ValueVerified As Boolean

            'in case OutOfRangeJson(LabelKey) is null
            Try
                ValueVerified = Boolean.Parse(OutOfRangeJson(LabelKey).ToString())
            Catch ex As Exception
                ValueVerified = False
            End Try

            InputInfo("valueVerified") = ValueVerified
        End If
        Res("input") = InputInfo

        Return Res
    End Function

    Private Function SqlProofSingleQuotes(Text As String) As String
        Return Text.Replace("'", "''") 'escape single quotes (') by doubling them ('')
    End Function

    Private Function GetInputValidity(LabelKey As Integer, Value As String) As Dictionary(Of String, Object)
        Dim UserInput As String = If(Value Is Nothing, Nothing, SqlProofSingleQuotes(Value))
        Dim FieldType As Object
        Dim Range As Object
        Dim BackColor As String = "invalid"
        Dim InputValidityRes As New Dictionary(Of String, String)
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@LabelKey", GetParamVarHash(LabelKey, "int")}
        }
        Dim InputDR As Data.DataRow = GetMyDataSetParamQuery("SELECT FieldType, Range FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey", QueryConfig).Tables(0).Rows(0)
        Dim Res As New Dictionary(Of String, Object)

        FieldType = InputDR("FieldType")
        Range = If(IsDBNull(InputDR("Range")), String.Empty, InputDR("Range"))

        If IsDBNull(FieldType) = False Then 'use fieldtype to validate input
            'search for cases where the input would be valid
            Select Case FieldType
                Case "Checkbox"
                    If UserInput = "1" Then
                        Res("state") = "valid"
                    Else
                        Res("state") = "invalid"
                    End If
                    Res("endUserMessage") = ""

                    Return Res
                Case "HOA"
                    If UserInput.Contains("...") Then
                        Res("state") = "invalid"
                    Else
                        Res("state") = "valid"
                    End If
                    Res("endUserMessage") = ""

                    Return Res
                Case "Text"
                    If String.IsNullOrEmpty(UserInput) Then
                        Res("state") = "invalid"
                    Else
                        Res("state") = "valid"
                    End If
                    Res("endUserMessage") = ""

                    Return Res
                Case "Date"
                    Dim ResMessage As String = LogAspx.ValidDate(UserInput)

                    If String.IsNullOrEmpty(ResMessage) Then
                        Res("state") = "valid"
                    Else
                        Res("state") = "invalid"
                    End If
                    Res("endUserMessage") = ResMessage

                    Return Res
                Case "STC"
                    Dim Temps As String() = UserInput.Split("/")
                    Dim Temp1 As Decimal
                    Dim Temp2 As Decimal

                    Try 'in case user types in invlaid characters
                        Temp1 = Decimal.Parse(Temps(0))
                        Temp2 = Decimal.Parse(Temps(1))
                    Catch ex As Exception
                        Exit Select
                    End Try

                    If Math.Abs(Temp1 - Temp2) > Decimal.Parse(Range.Split(" ")(1)) Then
                        Res("state") = "outOfScope"
                    Else
                        Res("state") = "valid"
                    End If
                    Res("endUserMessage") = ""

                    Return Res

                Case "DP"
                    Dim DPs As String() = UserInput.Split("/")
                    Dim DP1 As Decimal
                    Dim DP2 As Decimal

                    Try 'in case user types in invlaid characters
                        DP1 = Decimal.Parse(DPs(0))
                        DP2 = Decimal.Parse(DPs(1))
                    Catch ex As Exception
                        Exit Select
                    End Try

                    If DP1 = 1 OrElse DP2 = 1 Then
                        Res("state") = "valid"
                    Else
                        Res("state") = "invalid"
                    End If
                    Res("endUserMessage") = ""

                    Return Res
            End Select
        Else 'use range to validate input
            Return PmInput.ReportValidity("number", Range, UserInput)
        End If

        Return Res
    End Function

    Sub UploadToDataTable(LogOperator As String, InputsFieldValue As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = "Data Source=PWI-31\SATIDB;Initial Catalog=ALTS;Persist Security Info=True;User ID=sati;Password=laptopia"
        Connection.Open()

        Dim My_DA As New Data.SqlClient.SqlDataAdapter
        Dim My_DS As New Data.DataSet
        Dim My_DR As Data.DataRow

        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT Top(1) * FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@T_LogDataKey ORDER BY Date DESC" 'same query used to get MostRecRecordKey
            .Connection = Connection
            .Parameters.Add("@T_LogDataKey", SqlDbType.Int).Value = KeyFromQueryString
        End With
        My_DA.SelectCommand = MySelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim MyInsertCmd As New System.Data.SqlClient.SqlCommand
        With MyInsertCmd
            .CommandText = "INSERT INTO T_LogData (AreaKey, Inputs, OutOfRange, Date, Operator, Shift, ManagerStamp1, ManagerStamp2, ManagerStamp3, ToolNumber, Active) VALUES (@AreaKey, @Inputs, @OutOfRange, @Date, @Operator, @Shift, 0, NULL, NULL, NULL, NULL, 'False')"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@AreaKey", System.Data.SqlDbType.Int, 0, "AreaKey"), New System.Data.SqlClient.SqlParameter("@Inputs", System.Data.SqlDbType.VarChar, 0, "Inputs"), New System.Data.SqlClient.SqlParameter("@OutOfRange", System.Data.SqlDbType.VarChar, 0, "OutOfRange"), New System.Data.SqlClient.SqlParameter("@Date", System.Data.SqlDbType.SmallDateTime, 0, "Date"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.VarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@Shift", System.Data.SqlDbType.VarChar, 0, "Shift")})
        End With
        My_DA.InsertCommand = MyInsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim MyUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MyUpdateCmd
            .CommandText = "UPDATE T_LogData SET [Inputs] = @Inputs, [OutOfRange] = @OutOfRange, [Operator] = @Operator WHERE [Key]=@DataLogKey; SELECT TOP(1) * FROM T_LogData WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date DESC;"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Inputs", System.Data.SqlDbType.VarChar, 0, "Inputs"), New System.Data.SqlClient.SqlParameter("@OutOfRange", System.Data.SqlDbType.VarChar, 0, "OutOfRange"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.VarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@DataLogKey", System.Data.SqlDbType.Int, 0, "Key")})
        End With
        My_DA.UpdateCommand = MyUpdateCmd

        '*****************************************************************
        '************************Genral***********************************
        '*****************************************************************
        My_DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_LogData", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("AreaKey", "AreaKey"), New System.Data.Common.DataColumnMapping("Inputs", "Inputs"), New System.Data.Common.DataColumnMapping("OutOfRange", "OutOfRange"), New System.Data.Common.DataColumnMapping("Date", "Date"), New System.Data.Common.DataColumnMapping("Operator", "Operator"), New System.Data.Common.DataColumnMapping("Shift", "Shift")})}) 'the fields that are dynamically generated
        My_DA.Fill(My_DS)

        My_DR = My_DS.Tables(0).Rows(0)
        My_DR.AcceptChanges()
        My_DR.BeginEdit()
        My_DR("Operator") = LogOperator
        My_DR("Inputs") = InputsFieldValue
        My_DR("OutOfRange") = My_DR("OutOfRange")
        'My_DR("Date") = System.DateTime.Now.ToShortTimeString
        My_DR.EndEdit()
        My_DA.Update(My_DS, "T_LogData")

        Connection.Close()
    End Sub

End Class
