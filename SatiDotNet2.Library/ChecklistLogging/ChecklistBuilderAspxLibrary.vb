Imports System.Drawing
Imports SatiDotNet2.Library

Public Class ChecklistBuilderAspxLibrary
    Dim Sql As New Security

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

        QueryConfig("@Key")("value") = NextLabelKey
        NextLabelOrder = GetSingleDbField("SELECT LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@Key", QueryConfig, "LabelOrder")

        QueryConfig("@Key")("value") = LastLabelKey
        LastLabelOrder = GetSingleDbField("SELECT LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@Key", QueryConfig, "LabelOrder")

        If Action = "up" Then
            If PrevLabelKey = -1 Then 'if true, this means the label is already the top/up most label
                Return ""
            Else
                Return UpdateQueryTemplate & PrevLabelOrder & " WHERE [Key]=" & LabelKey & "; " & UpdateQueryTemplate & LabelOrder & " WHERE [Key]=" & PrevLabelKey
            End If
        Else
            If NextLabelKey = -1 Then 'if true, this means the label is already the bottom/down most label
                Return ""
            Else
                Return UpdateQueryTemplate & NextLabelOrder & " WHERE [Key]=" & LabelKey & "; " & UpdateQueryTemplate & LabelOrder & " WHERE [Key]=" & NextLabelKey
            End If
        End If
    End Function
End Class
