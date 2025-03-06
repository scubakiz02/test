Public Class LogAspx
    'return value of: true is valid; false is invalid; null is out of range
    Public Function ValidateByBackColor(NumOfNotes As Integer, BackColor As String) As Boolean?
        'If NumOfNotes = 0 Then
        '    If Value = "" OrElse Not All_InputsAreValid Then
        '        MessageUserLabel.Text = "Error: Incomplete or invalid logs. Add a comment to proceed."
        '        Exit Sub
        '    End If
        'ElseIf Not All_InputsAreValid Then
        '    'display verify interface
        '    DoneButton.Enabled = False
        '    MarkAsDoneCheckBox.Visible = True
        '    Return
        'End If

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
