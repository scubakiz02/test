Imports System.Drawing
Imports SatiDotNet2.Library

Public Class LogAspxLibrary
    Dim Sql As New Security

    'return value of: true is valid; false is invalid; null is out of range
    Public Function ValidateByBackColor(NumOfNotes As Integer, BackColor As String) As Boolean?
        Dim Res As Boolean = Nothing

        If NumOfNotes > 0 OrElse BackColor.Contains("f5f5f5") Or BackColor = "WhiteSmoke" Then 'WhiteSmoke in hex is #f5f5f5
            Res = True
        ElseIf BackColor.Contains("Red") Then
            Res = False
        ElseIf BackColor.Contains("e6e600") Then 'yellow
            Return Nothing 'cannot assign Nothing to variable Res for some reason
        End If

        Return Res
    End Function

    Public Function ReturnTrue() As Boolean
        Return True
    End Function

    Public Function GetStatusBoardRole(View As String, Department As String, Where As Date) As String()
        Dim Res As New List(Of String)

        If Where <> Today.Date Then
            Res.Add("admin")
        ElseIf View = "Focus" AndAlso Department = "Production" Then 'if view is focus & department is production, return should be nothing
            Res.Add(Nothing)
        ElseIf View = "Full" Then 'if user wnats to see past issues column, they will need the associated supervisor role
            If Department <> "Production" Then
                Res.Add("FMManagerApproval")
                Res.Add("QSHEManagerApproval")
            Else
                Res.Add("PC")
            End If
        Else 'user will need to at minimum have 'Maintenance' role to view 'All' or 'Maintenance' department logs
            If Department <> "Production" Then
                Res.Add("Maintenance")
            End If
        End If

        Return Res.ToArray()
    End Function

    Function ValidDate(UserInput As String) As String 'valid date must be MM/YY format
        Dim DateParseInput As Date
        Dim DateDelimited As String()
        Dim InputMonth As Integer
        Dim InputYear As Integer
        Dim Message As String = ""

        Try
            If UserInput.Contains("/") = False OrElse UserInput.Length <> 5 Then
                Throw New FormatException("")
            End If

            DateDelimited = UserInput.Split("/")
            InputMonth = Integer.Parse(DateDelimited(0))
            InputYear = Integer.Parse(DateDelimited(1))
            DateParseInput = Date.Parse(InputMonth & "/" & Today.Day & "/" & "20" & InputYear.ToString())

            If DateParseInput.Date < Today.Date Then
                Throw New Exception("*Error: Date is in the past*")
            End If
        Catch ex As FormatException
            Message = "*Format Error: MM/YY*"
        Catch ex As Exception
            Message = ex.Message.ToString()
        End Try

        Return Message
    End Function

    Function GetSingleDbField(SqlQuery As String, QueryConfig As Dictionary(Of String, Dictionary(Of String, String)), Field As String) As String
        Dim Res As String

        'using try catch block in case 'There is no row at position 0.', which means there are no associated record in Table
        Try
            Res = Sql.GetMyDataSetParamQuery(SqlQuery, QueryConfig).Tables(0).Rows(0)(Field)
            Res = If(IsDBNull(Res), Nothing, Res) 'using ternary operator as a workaround to Null DB field values, which in that case the function will return Nothing
        Catch ex As Exception
            Res = Nothing
        End Try

        Return Res
    End Function


    Function ModifyLabelOrder(LabelKey As Integer, Action As String) As String
        Dim UpdateQueryTemplate As String = "UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder="
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        QueryConfig("@Key") = New Dictionary(Of String, String) From {
            {"value", LabelKey},
            {"typeOf", "int"}
        }
        Dim LabelKeyDS As Data.DataSet = Sql.GetMyDataSetParamQuery("SELECT (SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogLabel] Frst WHERE Frst.AreaKey=Curr.AreaKey ORDER BY Frst.LabelOrder) As FirstLabelKey, (SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogLabel] Prev WHERE Prev.AreaKey=Curr.AreaKey AND Prev.LabelOrder < Curr.LabelOrder ORDER BY Prev.LabelOrder DESC) As PrevLabelKey, (SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogLabel] Nxt WHERE Nxt.AreaKey=Curr.AreaKey AND LabelOrder > Curr.LabelOrder ORDER BY Nxt.LabelOrder) As NextLabelKey, (SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogLabel] Lst WHERE Lst.AreaKey=Curr.AreaKey ORDER BY Lst.LabelOrder DESC) As LastLabelKey FROM [ALTS].[dbo].[T_LogLabel] Curr WHERE [Key]=@Key GROUP BY LabelOrder, [Key], AreaKey", QueryConfig)
        Dim LabelKeyDR As Data.DataRow = LabelKeyDS.Tables(0).Rows(0)
        Dim FirstLabelKey As Integer = LabelKeyDR("FirstLabelKey")
        Dim PrevLabelKey As Integer = If(IsDBNull(LabelKeyDR("PrevLabelKey")), -1, LabelKeyDR("PrevLabelKey")) 'ternary operator in case value is DBNull
        Dim NextLabelKey As Integer = If(IsDBNull(LabelKeyDR("NextLabelKey")), -1, LabelKeyDR("NextLabelKey")) 'ternary operator in case value is DBNull
        Dim LastLabelKey As Integer = LabelKeyDR("LastLabelKey")
        Dim FirstLabelOrder As Integer
        Dim PrevLabelOrder As String 'string variable type in case PrevLabelKey is -1
        Dim LabelOrder As Integer
        Dim NextLabelOrder As String 'string variable type in case NextLabelKey is -1
        Dim LastLabelOrder As Integer

        QueryConfig("@Key")("value") = FirstLabelKey
        FirstLabelOrder = GetSingleDbField("SELECT LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@Key", QueryConfig, "LabelOrder")

        QueryConfig("@Key")("value") = PrevLabelKey
        PrevLabelOrder = GetSingleDbField("SELECT LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@Key", QueryConfig, "LabelOrder")

        QueryConfig("@Key")("value") = LabelKey
        LabelOrder = GetSingleDbField("SELECT LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@Key", QueryConfig, "LabelOrder")

        If Action = "up" Then
            If PrevLabelKey = -1 Then 'if true, this means the label is already the top/up most label
                Return ""
            Else
                Return UpdateQueryTemplate & PrevLabelOrder & " WHERE [Key]=" & LabelKey & "; " & UpdateQueryTemplate & LabelOrder & " WHERE [Key]=" & PrevLabelKey
            End If
        Else

        End If
    End Function
End Class
