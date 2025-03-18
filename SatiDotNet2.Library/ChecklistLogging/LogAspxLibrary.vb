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
End Class
