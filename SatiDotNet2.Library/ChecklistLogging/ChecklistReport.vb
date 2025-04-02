Imports System.Text.Json

Public Class GroupReport
    Inherits Security

    Private ReadOnly GroupKey As Integer
    Private ReadOnly AreaKey As Integer
    Private GroupDS As Data.DataSet
    Private EmptyGroupDS As New Data.DataSet()
    Public ConstructorQuery As String = "SELECT A.[Key] As AreaKey, A.Area, D.[Key] As DataKey, D.Operator, L.[Key] As LabelKey, Case WHEN L.Range Is Not NULL THEN L.Label + ' | ' + L.Range + Case WHEN L.UnitKey Is Not NULL THEN ' ' + U.Unit Else '' End Else L.Label End As Label, FORMAT(D.Date, 'MM-dd-yyyy') As Date, D.Inputs, '' As Value, '' As InputDate, '' As InputOperator FROM [ALTS].[dbo].[T_LogLabel] L INNER JOIN [ALTS].[dbo].[T_LogArea] A On L.AreaKey=A.[Key] RIGHT JOIN [ALTS].[dbo].[T_LogData] D On D.AreaKey=L.AreaKey LEFT JOIN [ALTS].[dbo].[T_LogUnit] U On L.UnitKey=U.[Key] WHERE (A.[Key]=@AreaKey OR @AreaKey=0) AND (A.GroupKey=@GroupKey OR (@GroupKey=0 AND A.GroupKey IS NOT NULL)) ORDER BY A.Area, L.LabelOrder, D.Date"
    Private MaxFieldVals As New Dictionary(Of String, String)
    Private QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Dim LogAspx As New LogAspxLibrary
    Private StartDate As Date?
    Private EndDate As Date?
    Public ReadOnly InvalidDateMessage As String = "Error: Invalid date"
    Public ReadOnly OutOfRangeDateMessage As String = "Error: Out of range"

    Public Sub New(Config As Dictionary(Of String, String))
        Dim DT As New DataTable("Employees")

        DT.Columns.Add("Area", GetType(Integer))
        DT.Columns.Add("Label", GetType(String))
        DT.Columns.Add("Value", GetType(String))
        DT.Columns.Add("Date", GetType(Date))
        DT.Columns.Add("Operator", GetType(String))

        EmptyGroupDS.Tables.Add(DT)

        GroupKey = Config("GroupKey")
        AreaKey = Config("AreaKey")

        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaKey},
            {"typeOf", "int"}
        }
        QueryConfig("@GroupKey") = New Dictionary(Of String, String) From {
            {"value", GroupKey},
            {"typeOf", "int"}
        }

        If Config.ContainsKey("StartDate") AndAlso Config.ContainsKey("EndDate") Then
            SetDateRange(Config("StartDate"), Config("EndDate"))
        End If
    End Sub

    Public Function SetDateRange(Start_Date As String, End_Date As String) As Data.DataSet
        If Start_Date Is Nothing OrElse End_Date Is Nothing Then
            Return Nothing
        Else
            StartDate = Start_Date
            EndDate = End_Date
        End If

        QueryConfig("@StartDate") = New Dictionary(Of String, String) From {
                {"value", StartDate},
                {"typeOf", "date"}
            }
        QueryConfig("@EndDate") = New Dictionary(Of String, String) From {
                {"value", EndDate},
                {"typeOf", "date"}
            }
        ConstructorQuery = ConstructorQuery.Replace("WHERE", "WHERE D.Date >= @StartDate AND D.Date <= @EndDate AND ")

        ConfigureDS()

        Return GroupDS
    End Function

    Public Function DateInRange(UserDate As String) As String
        Dim Message As String = String.Empty
        Dim DateParsed As Date
        Dim DateDelimited As String()

        Try
            DateParsed = Date.Parse(UserDate)
            DateDelimited = UserDate.Split("/")

            If DateDelimited.Count < 3 OrElse DateDelimited(2).Length <> 4 Then
                Throw New Exception("")
            ElseIf DateParsed < Date.Parse("03/16/2025") OrElse DateParsed > Today.Date Then '03/16/2025 is date of first entry
                Message = OutOfRangeDateMessage
            End If
        Catch ex As Exception
            Message = InvalidDateMessage
        End Try

        Return Message
    End Function

    Public Function SetArea(AreaKey As String) As Data.DataSet
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From { 'constructing a value for QueryConfig("@AreaKey"), b/c the @AreaKey key may NOT exist in QueryConfig
            {"value", AreaKey},
            {"typeOf", "int"}
        }

        ConfigureDS()

        Return GroupDS
    End Function

    Public Function SetGroup(GroupKey As String) As Data.DataSet
        QueryConfig("@GroupKey")("value") = GroupKey 'modifying the key value in QueryConfig, b/c the @GroupKey key will ALWAYS exist in QueryConfig

        ConfigureDS()

        Return GroupDS
    End Function

    Public Function GetMaxFieldVals() As Dictionary(Of String, String)
        Return MaxFieldVals
    End Function

    Private Sub SetMaxFieldVals(KeyToTest As String, TestVal As String)
        If TestVal.Length > MaxFieldVals(KeyToTest).Length Then
            MaxFieldVals(KeyToTest) = TestVal
        End If
    End Sub

    Private Sub ConfigureDS()
        Dim GroupRC As Integer

        If StartDate Is Nothing OrElse EndDate Is Nothing Then Exit Sub

        GroupDS = GetMyDataSetParamQuery(ConstructorQuery, QueryConfig)
        GroupRC = GroupDS.Tables(0).Rows.Count - 1

        MaxFieldVals = New Dictionary(Of String, String) From { 'to ensure values in MaxFieldVals pertain to current configured dataset
           {"InputDate", String.Empty},
           {"Value", String.Empty},
           {"InputOperator", String.Empty},
           {"Area", String.Empty},
           {"Label", String.Empty}
        }

        For I As Integer = 0 To GroupRC
            Dim GroupDR As Data.DataRow = GroupDS.Tables(0).Rows(I)
            Dim LabelKey As Integer = GroupDR("LabelKey")
            Dim Inputs As Dictionary(Of Integer, Dictionary(Of String, String)) = LogAspx.GetInputs(GroupDR) 'in case DB Inputs field value is in the old JSON format, run it through LogAspxLibrary Class GetInputs function
            Dim ObjOfInterest As Dictionary(Of String, String)
            Dim InputDate As String
            Dim Value As String
            Dim InputOperator As String
            Dim Area As String
            Dim Label As String

            Try 'in case there is no instance of a log for a certain date (grouped reports are not guarenteed the same start and end dates)
                ObjOfInterest = Inputs(LabelKey)
                InputDate = ObjOfInterest("Date")
                Value = ObjOfInterest("Value")
                InputOperator = ObjOfInterest("Operator")
                Area = GroupDR("Area")
                Label = GroupDR("Label")
            Catch ex As Exception
                InputDate = String.Empty
                Value = String.Empty
                InputOperator = String.Empty
                Area = String.Empty
                Label = String.Empty
            End Try

            GroupDR("InputDate") = InputDate
            GroupDR("Value") = Value
            GroupDR("InputOperator") = InputOperator

            SetMaxFieldVals("Label", Label)
            SetMaxFieldVals("InputDate", InputDate)
            SetMaxFieldVals("Value", Value)
            SetMaxFieldVals("InputOperator", InputOperator)
            SetMaxFieldVals("Area", Area)
        Next
    End Sub

    Public Function GetDS() As Data.DataSet
        If GroupDS IsNot Nothing Then
            Return GroupDS
        Else
            Return EmptyGroupDS
        End If
    End Function
End Class
