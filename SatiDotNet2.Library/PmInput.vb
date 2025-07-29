Imports System.Text.Json

Public Class PmInput
    Inherits Security

    Sub New()

    End Sub

    Public Function GetArea(LabelKey As String) As Integer
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@LabelKey", GetParamVarHash(LabelKey, "int")}
        }

        Return GetSingleDbField("SELECT AreaKey FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey", QueryConfig, "AreaKey")
    End Function

    Public Function Delete(LabelKey As String, Optional InvocateAsTest As Boolean = False) As Dictionary(Of String, String)
        'Delete relevant record in T_LogLabel
        Dim Res As New Dictionary(Of String, String)
        Dim SqlQuery As String
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))

        If LabelKey Is Nothing Then
            Res("Success") = False
            Return Res
        End If

        QueryConfig("@LabelKey") = GetParamVarHash(LabelKey, "int")
        SqlQuery = "DELETE FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey;"

        If InvocateAsTest Then
            Res("QueryConfig") = JsonSerializer.Serialize(QueryConfig)
            Res("SqlQuery") = SqlQuery
        End If
        Res("Success") = If(ExecuteSqlParamQuery(SqlQuery, QueryConfig) Is Nothing, False, True)

        Return Res
    End Function

    Public Function ReportValidity(FieldType As String, Range As String, Value As String) As Dictionary(Of String, Object)
        Dim Res As New Dictionary(Of String, Object)

        Try
            Dim UserInputDec As Decimal = Decimal.Parse(Value) 'FormatException can be thrown here

            If Value <> "000" Then '"000" is a valid input by default
                If Range.Contains("-") Then 'span range (Ex: 1-2; 3-10.5, etc.)
                    Dim DelimitArr() As String = Range.Split("-")
                    Dim LowerBound As Decimal = Decimal.Parse(Trim(DelimitArr(0)))
                    Dim UpperBound As Decimal = Decimal.Parse(Trim(DelimitArr(1)))

                    If UserInputDec < LowerBound Or UserInputDec > UpperBound Then
                        Throw New ArgumentOutOfRangeException()
                    End If
                ElseIf Range.Contains("<") Then 'threshold range (Ex: <5, <10.2, etc.)
                    Dim LimitDec As Decimal = Decimal.Parse(Trim(Range.Replace("<", "")))

                    If UserInputDec >= LimitDec Then
                        Throw New ArgumentOutOfRangeException()
                    End If
                ElseIf Range.Contains(">") Then 'threshold range (Ex: >5, >10.2, etc.)
                    Dim LimitDec As Decimal = Decimal.Parse(Trim(Range.Replace(">", "")))

                    If UserInputDec <= LimitDec Then
                        Throw New ArgumentOutOfRangeException()
                    End If
                End If
            End If

            Res("state") = "valid"
            Res("message") = ""
        Catch ex As ArgumentOutOfRangeException 'if 'Value' is out of range (determined by code above)
            Res("state") = "outOfScope"
            Res("message") = "*CAUTION: OUT OF RANGE*"
        Catch ex As FormatException 'if 'Value' arg is Not a number
            Res("state") = "invalid"
            Res("message") = "*ERROR: NOT A NUMBER*"
        End Try

        Return Res
    End Function
End Class
