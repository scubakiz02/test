Imports System.Drawing
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class LogAspxLibrary
    Dim Sql As New Security
    Dim Format As New Format()

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
            DateParseInput = Date.Parse(InputMonth & "/01/" & "20" & InputYear.ToString())

            If DateParseInput.Year < Today.Year OrElse (DateParseInput.Year = Today.Year AndAlso DateParseInput.Month < Today.Month) Then 'ensure user input date is today or later, but do NOT check the day in DateParseInput variable value 
                Throw New Exception("*Error: Date is in the past*")
            End If
        Catch ex As FormatException
            Message = "*Format Error: MM/YY*"
        Catch ex As Exception
            Message = ex.Message.ToString()
        End Try

        Return Message
    End Function

    Function GetRange(T_LogDataKey As String, T_LogDataDR As Data.DataRow, T_LogLabelDR As Data.DataRow) As String
        Dim Res As String
        Dim T_LogLabelRange As String = If(IsDBNull(T_LogLabelDR("Range")), String.Empty, T_LogLabelDR("Range"))

        Try
            If T_LogDataKey Is Nothing OrElse T_LogDataDR("CompleteLog") = False Then ' If T_LogDataKey Is Nothing, that means user is in ChecklistBuilder.aspx
                Res = T_LogLabelRange
            Else
                Dim Ranges As Dictionary(Of String, String) = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(T_LogDataDR("Ranges"))

                If Ranges(T_LogLabelDR("LabelKey")) IsNot Nothing Then
                    Res = Ranges(T_LogLabelDR("LabelKey"))
                Else
                    Res = String.Empty
                End If
            End If
        Catch ex As Exception
            Res = String.Empty
        End Try

        Return Res
    End Function

    'T_LogData Inputs field value was originally a stringified Dictionary(Of Integer, String).
    'As of 03/2025, it is a stringified Dictionary(Of Integer, Dictionary(Of String, String))
    'This is so each user, time, & value of each input can be tracked, for reporting down the road
    'this function exists in case a logsheet is following the original approach (stringified Dictionary(Of Integer, String))
    'the function will restructure the field to Dictionary(Of Integer, Dictionary(Of String, String))
    Function GetInputs(DR As Data.DataRow) As Dictionary(Of Integer, Dictionary(Of String, String))
        Dim Res As New Dictionary(Of Integer, Dictionary(Of String, String))

        Try
            Res = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(DR("Inputs"))
        Catch ex As Exception
            Dim InputsFromDb As Dictionary(Of Integer, String) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, String))(DR("Inputs"))

            For Each kvp As KeyValuePair(Of Integer, String) In InputsFromDb
                Dim T_LogDataKey As Integer = kvp.Key
                Dim UserInput As String = kvp.Value
                Dim InputOperator As String = If(IsDBNull(DR("Operator")) = False, DR("Operator"), String.Empty)
                Dim NewInputConfig As New Dictionary(Of String, String)
                Dim DateValue As String

                If String.IsNullOrEmpty(UserInput) = False Then
                    DateValue = Format.DateField(DR("Date"))
                Else
                    DateValue = String.Empty
                End If

                NewInputConfig("Date") = DateValue
                NewInputConfig("Operator") = InputOperator
                NewInputConfig("Value") = UserInput

                Res(T_LogDataKey) = NewInputConfig
            Next
        End Try

        Return Res
    End Function

    Public Function IsEveryInputEmpty(InputsStringified As String) As Boolean
        Dim InputsJson As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(InputsStringified)

        For Each Input As KeyValuePair(Of Integer, Dictionary(Of String, String)) In InputsJson
            Dim OperatorValue As String = InputsJson(Input.Key)("Value")

            If String.IsNullOrEmpty(OperatorValue) = False Then
                Return False
            End If
        Next

        Return True
    End Function
End Class
