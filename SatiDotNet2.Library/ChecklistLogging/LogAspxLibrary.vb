Public Class LogAspx
    'return value of: true is valid; false is invalid; null is out of range
    Public Function ValidateByBackColor(NumOfNotes As Integer, BackColor As String) As Boolean?
        Dim Res As Boolean = Nothing

        If NumOfNotes > 0 OrElse BackColor = "#F5F5F5" Then
            Res = True
        ElseIf BackColor = "red" Then
            Res = False
        ElseIf BackColor = "#E6E600" Then 'yellow
            Return Nothing 'cannot assign Nothing to variable Res for some reason
        End If

        Return Res
    End Function

    Public Function ReturnTrue() As Boolean
        Return True
    End Function

End Class
