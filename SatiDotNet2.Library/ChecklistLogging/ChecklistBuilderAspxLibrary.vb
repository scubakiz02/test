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

    Function ModifyOrder(Key As Integer, Action As String, Marker As String) As String
        Dim Table As String = "[ALTS].[dbo]."
        Dim FieldOfInterest As String

        Select Case Marker
            Case "Label"
                Table += "[T_LogLabel]"
                FieldOfInterest = "LabelOrder"
            Case Else
                Table += "[T_LogCommentList]"
                FieldOfInterest = "CommentOrder"
        End Select


        Dim UpdateQueryTemplate As String = "UPDATE " & Table & " SET " & FieldOfInterest & "="
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        QueryConfig("@Key") = New Dictionary(Of String, String) From {
            {"value", Key},
            {"typeOf", "int"}
        }
        Dim KeyDS As Data.DataSet = Sql.GetMyDataSetParamQuery("SELECT (SELECT TOP(1) [Key] FROM " & Table & " Frst WHERE Frst.AreaKey=Curr.AreaKey ORDER BY Frst." & FieldOfInterest & ") As FirstKey, (SELECT TOP(1) [Key] FROM " & Table & " Prev WHERE Prev.AreaKey=Curr.AreaKey AND Prev." & FieldOfInterest & " < Curr." & FieldOfInterest & " ORDER BY Prev." & FieldOfInterest & " DESC) As PrevKey, (SELECT TOP(1) [Key] FROM " & Table & " Nxt WHERE Nxt.AreaKey=Curr.AreaKey AND " & FieldOfInterest & " > Curr." & FieldOfInterest & " ORDER BY Nxt." & FieldOfInterest & ") As NextKey, (SELECT TOP(1) [Key] FROM " & Table & " Lst WHERE Lst.AreaKey=Curr.AreaKey ORDER BY Lst." & FieldOfInterest & " DESC) As LastKey FROM " & Table & " Curr WHERE [Key]=@Key GROUP BY " & FieldOfInterest & ", [Key], AreaKey", QueryConfig)
        Dim KeyDR As Data.DataRow = KeyDS.Tables(0).Rows(0)
        Dim FirstKey As Integer = KeyDR("FirstKey")
        Dim PrevKey As Integer = If(IsDBNull(KeyDR("PrevKey")), -1, KeyDR("PrevKey")) 'ternary operator in case value is DBNull
        Dim NextKey As Integer = If(IsDBNull(KeyDR("NextKey")), -1, KeyDR("NextKey")) 'ternary operator in case value is DBNull
        Dim LastKey As Integer = KeyDR("LastKey")
        Dim FirstOrder As Integer
        Dim PrevOrder As String 'string variable type in case PrevKey is -1, which would mean this variable could be null (Nothing)
        Dim Order As Integer
        Dim NextOrder As String 'string variable type in case NextKey is -1, which would mean this variable could be null (Nothing)
        Dim LastOrder As Integer

        QueryConfig("@Key")("value") = FirstKey
        FirstOrder = GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = PrevKey
        PrevOrder = GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = Key
        Order = GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = NextKey
        NextOrder = GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = LastKey
        LastOrder = GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        If Action = "up" Then
            If PrevKey = -1 Then 'if true, this means the label is already the top/up most label
                Return ""
            Else
                Return UpdateQueryTemplate & PrevOrder & " WHERE [Key]=" & Key & "; " & UpdateQueryTemplate & Order & " WHERE [Key]=" & PrevKey
            End If
        Else
            If NextKey = -1 Then 'if true, this means the label is already the bottom/down most label
                Return ""
            Else
                Return UpdateQueryTemplate & NextOrder & " WHERE [Key]=" & Key & "; " & UpdateQueryTemplate & Order & " WHERE [Key]=" & NextKey
            End If
        End If
    End Function

End Class
