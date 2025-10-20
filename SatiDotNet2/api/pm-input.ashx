<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.IO
Imports System.Text.Json
Imports SatiDotNet2.Library
Imports System.Data
Imports System.Configuration

Public Class StreamData
    Inherits Security
    Implements IHttpHandler, IReadOnlySessionState

    Private Format As New Format()
    Private LogAspx As New LogAspxLibrary()
    Private ActivePm As New ActivePm()
    Private PmInput As New PmInput()
    Private PhaseController As New PhaseController()
    Private _DataKey As String

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim method As String = context.Request.HttpMethod.ToUpperInvariant()
        Dim Res As New Dictionary(Of String, Object)

        Try
            If method = "GET" Then
                Dim ParamDict As New Dictionary(Of String, Object)()
                For Each Key As String In context.Request.QueryString.AllKeys
                    ParamDict(Key) = context.Request.QueryString(Key)
                Next

                Dim IsPost As Boolean = False
                Res = BuildHttpResponse(ParamDict, IsPost)
            ElseIf method = "POST" Then
                Dim ParamJsonString As String
                Using reader As New StreamReader(context.Request.InputStream)
                    ParamJsonString = reader.ReadToEnd()
                End Using
                Dim ParamJson As Dictionary(Of String, Object) = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(ParamJsonString)

                Dim IsPost As Boolean = True
                Res = BuildHttpResponse(ParamJson, IsPost)

                SseStatusBoardHub.StatusBoardChange(_DataKey)
            End If
        Catch KeyNotFoundException As KeyNotFoundException
            Res = New Dictionary(Of String, Object)
            Res("message") = "*ERROR: FAILED TO RETRIEVE DATA*"
            context.Response.StatusCode = 400
        Catch Exception As Exception
            Res = New Dictionary(Of String, Object)
            Res("message") = "*ERROR: DID NOT SAVE USER INPUT*"
            context.Response.StatusCode = 406
        End Try

        Res("phaseLevel") = PhaseController.GetPhase(_DataKey)

        context.Response.ContentType = "application/json"
        context.Response.Write(JsonSerializer.Serialize(Res))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function BuildHttpResponse(ParamJson As Dictionary(Of String, Object), IsPost As Boolean) As Dictionary(Of String, Object)
        Dim HttpResponse As New Dictionary(Of String, Object)

        Try
            _DataKey = ParamJson("dataId").ToString()
            Dim LabelKey As String = ParamJson("labelId").ToString()
            Dim InputsJson As Dictionary(Of String, Dictionary(Of String, String)) = GetInputs(_DataKey, LabelKey)
            Dim DbValue As String = InputsJson(LabelKey)("Value").ToString()

            If IsPost Then
                'update db value if client side value is different from db value
                Dim ClientSideValue As String = ParamJson("value").ToString()
                If ClientSideValue <> DbValue Then
                    ModifyInput(InputsJson, LabelKey, ClientSideValue)
                    DbValue = ClientSideValue
                End If
            End If

            HttpResponse = GetInputInfo(_DataKey, LabelKey, DbValue)
            HttpResponse("success") = True
        Catch ex As Exception
            HttpResponse("success") = False
        End Try

        Return HttpResponse
    End Function

    Private Sub ModifyInput(InputsJson As Dictionary(Of String, Dictionary(Of String, String)), LabelKey As String, Value As String)
        Dim RecordedUser As String = HttpContext.Current.User.Identity.Name.ToString()
        Dim InputOfInterest As Dictionary(Of String, String) = InputsJson(LabelKey)
        InputOfInterest("Operator") = RecordedUser
        InputOfInterest("Date") = Format.DateField(System.DateTime.Now.ToString())
        InputOfInterest("Value") = Value
        InputsJson(LabelKey) = InputOfInterest 'update input within Inputs field value

        'in case of db upload failure, closing code below in a try catch block
        'if db update query fails, try it again
        Try
            UploadToDataTable(RecordedUser, JsonSerializer.Serialize(InputsJson))
        Catch ex As Exception
            UploadToDataTable(RecordedUser, JsonSerializer.Serialize(InputsJson))
        End Try
    End Sub

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
                    Res("message") = ""

                    Return Res
                Case "HOA"
                    If UserInput.Contains("Hand") OrElse UserInput.Contains("Off") OrElse UserInput.Contains("Auto") Then
                        Res("state") = "valid"
                    Else
                        Res("state") = "invalid"
                    End If
                    Res("message") = ""

                    Return Res
                Case "Text"
                    If String.IsNullOrEmpty(UserInput) Then
                        Res("state") = "invalid"
                    Else
                        Res("state") = "valid"
                    End If
                    Res("message") = ""

                    Return Res
                Case "Date"
                    Dim ResMessage As String = LogAspx.ValidDate(UserInput)

                    If String.IsNullOrEmpty(ResMessage) Then
                        Res("state") = "valid"
                    Else
                        Res("state") = "invalid"
                    End If
                    Res("message") = ResMessage

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
                    Res("message") = ""

                    Return Res

                Case "DP"
                    Dim DPs As String() = UserInput.Split("/")
                    Dim DP1 As Decimal
                    Dim DP2 As Decimal

                    Try 'in case user types in invlaid characters
                        DP1 = Decimal.Parse(DPs(0))
                        DP2 = Decimal.Parse(DPs(1))
                    Catch ex As Exception
                        Res("state") = "invalid"
                    End Try

                    If DP1 = 1 OrElse DP2 = 1 Then
                        Res("state") = "valid"
                    Else
                        Res("state") = "invalid"
                    End If
                    Res("message") = ""

                    Return Res
            End Select
        Else 'use range to validate input
            Return PmInput.ReportValidity("number", Range, UserInput)
        End If

        Return Res
    End Function

    Private Function GetInputs(DataKey As String, LabelKey As String) As Dictionary(Of String, Dictionary(Of String, String))
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@DataKey", GetParamVarHash(DataKey, "int")}
        }
        Dim InputsStringified As String = GetSingleDbField("SELECT Inputs FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@DataKey", QueryConfig, "Inputs")
        Dim InputsAsJson As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(InputsStringified)

        Return InputsAsJson
    End Function

    Private Function GetInputInfo(DataKey As String, LabelKey As String, Value As String) As Dictionary(Of String, Object)
        Dim Res As Dictionary(Of String, Object)

        Res = GetInputValidity(LabelKey, Value)
        If Res("state") = "outOfScope" Then
            Dim OutOfRangeJson As Dictionary(Of String, Object) = ActivePm.GetOutOfRange(DataKey)
            Dim ValueVerified As Boolean

            'in case OutOfRangeJson(LabelKey) is null
            Try
                ValueVerified = Boolean.Parse(OutOfRangeJson(LabelKey).ToString())
            Catch ex As Exception
                ValueVerified = False
            End Try

            Res("valueVerified") = ValueVerified
        End If

        Return Res
    End Function

    Sub UploadToDataTable(LogOperator As String, InputsFieldValue As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
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
            .Parameters.Add("@T_LogDataKey", SqlDbType.Int).Value = _DataKey
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
            .CommandText = "UPDATE T_LogData SET [Inputs] = @Inputs, [OutOfRange] = @OutOfRange, [Operator] = @Operator WHERE [Key]=@DataLogKey; SELECT TOP(1) * FROM T_LogData WHERE [Key]=" & _DataKey & " ORDER BY Date DESC;"
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
        My_DR("Operator") = DBNull.Value 'start phasing this field out. Eventually, delete this column from T_LogData
        My_DR("Inputs") = InputsFieldValue
        My_DR("OutOfRange") = My_DR("OutOfRange")
        'My_DR("Date") = System.DateTime.Now.ToShortTimeString
        My_DR.EndEdit()
        My_DA.Update(My_DS, "T_LogData")

        Connection.Close()
    End Sub

End Class
