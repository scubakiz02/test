Imports System.Drawing

Public Class LogAspxLibrary
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

    Public Function GetStatusBoardRole(View As String, Department As String, Where As Date) As String
        Dim Res As String = Nothing

        If Where <> Today.Date Then
            Res = "admin"
        ElseIf View = "Full" Then
            If Department <> "Production" Then 'view would then be either maintenance or all
                Res = "FMManagerApproval"
            Else
                Res = "PC"
            End If
        End If

        Return Res
    End Function

End Class
