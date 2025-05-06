Imports System.Text.Json

Public Class Report
    Inherits Security

    Private GroupDS As Data.DataSet
    Private EmptyGroupDS As New Data.DataSet()
    Private ConstructorQuery As String = "SELECT A.[Key] As AreaKey, A.Area, A.[Key] As AreaKey, D.[Key] As DataKey, D.Operator, L.[Key] As LabelKey, L.FieldType, Case WHEN L.Range Is Not NULL THEN L.Label + ' | ' + L.Range + Case WHEN L.UnitKey Is Not NULL THEN ' ' + U.Unit Else '' End Else L.Label End As Label, FORMAT(D.Date, 'MM/dd/yyyy') As Date, D.Inputs, '' As Value, '' As StartDate, '' As InputDate, '' As InputOperator FROM [ALTS].[dbo].[T_LogLabel] L INNER JOIN [ALTS].[dbo].[T_LogArea] A On L.AreaKey=A.[Key] RIGHT JOIN [ALTS].[dbo].[T_LogData] D On D.AreaKey=L.AreaKey LEFT JOIN [ALTS].[dbo].[T_LogUnit] U On L.UnitKey=U.[Key] WHERE (A.[Key]=@AreaKey0 OR @AreaKey0=0) AND (A.GroupKey=@GroupKey OR (@GroupKey=0 AND A.GroupKey IS NOT NULL)) ORDER BY A.Area, L.LabelOrder, D.Date"
    Private MaxFieldVals As New Dictionary(Of String, String)
    Private QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Dim LogAspx As New LogAspxLibrary
    Private DbFormatting As New Format()
    Public ReadOnly InvalidDateMessage As String = "Error: Invalid date"
    Public ReadOnly OutOfRangeDateMessage As String = "Error: Out of range"
    Private LabelsToExclude As New List(Of Integer)
    Private AreasToExclude As New List(Of Integer)
    Private LabelsHash As New Dictionary(Of Integer, String)
    Private AreasHash As New Dictionary(Of Integer, String)
    Private LabelKeysList As New List(Of Integer)
    Private AreaKeysList As New List(Of Integer)
    Private DS_OrderedByDate As Boolean = False

    Public Sub New(Config As Dictionary(Of String, String))
        Dim DT As New DataTable("Employees")

        DT.Columns.Add("Area", GetType(Integer))
        DT.Columns.Add("LabelKey", GetType(String))
        DT.Columns.Add("Label", GetType(String))
        DT.Columns.Add("Value", GetType(String))
        DT.Columns.Add("Date", GetType(Date))
        DT.Columns.Add("Operator", GetType(String))

        EmptyGroupDS.Tables.Add(DT)

        SetVar("GroupKey", Config("GroupKey"), "int")
        SetVar("AreaKey0", Config("AreaKey"), "int")

        If Config.ContainsKey("StartDate") AndAlso Config.ContainsKey("EndDate") Then
            SetDateRange(Config("StartDate"), Config("EndDate"))
        End If
    End Sub

    Private Function GetVar(VarName As String) As String
        Dim ParameterizedKey As String = "@" & VarName

        If QueryConfig.ContainsKey(ParameterizedKey) Then
            Return QueryConfig(ParameterizedKey)("value")
        Else
            Return Nothing
        End If

    End Function

    Private Sub SetVar(VarName As String, Value As String, DataType As String)
        If Value Is Nothing Then
            'using Contains() method for "AreaKey', b/c there can be several AreaKey parameterized keys (AreaKey0, AreaKey1, etc.)
            If VarName.Contains("AreaKey") OrElse VarName = "GroupKey" Then Value = 0 '0 represents 'All' for Area/Group related parameterized values
        End If

        QueryConfig("@" & VarName) = New Dictionary(Of String, String) From {
            {"value", Value},
            {"typeOf", DataType}
        }
    End Sub

    Private Function StringDelimit(Str As String, Delimiters As String()) As String()
        Return Str.Split(Delimiters, StringSplitOptions.None)
    End Function

    Public Function GetOperators() As Data.DataSet
        Dim ConstructorQueryDelimited As String() = ConstructorQuery.Split({"FROM", "ORDER BY"}, StringSplitOptions.None) 'strip queried fields & ORDER BY clause from ConstructorQuery
        Dim OperatorDS As Data.DataSet = GetMyDataSetParamQuery("SELECT Operator FROM" & ConstructorQueryDelimited(1) & "GROUP BY Operator", QueryConfig)

        For Each OperatorDR As Data.DataRow In OperatorDS.Tables(0).Rows
            If {Nothing, "Basic User", "Szymon Tyburek", "Brett Teets"}.Contains(If(IsDBNull(OperatorDR("Operator")), Nothing, OperatorDR("Operator"))) Then 'Nothing represents a DBNull value
                OperatorDR.Delete() 'mark row for deletion
            End If
        Next
        OperatorDS.Tables(0).AcceptChanges() 'remove DataRow(s) marked for deletion permanently

        Return OperatorDS
    End Function

    Public Function SetDateRange(StartDate As String, EndDate As String) As Data.DataSet
        SetVar("StartDate", StartDate, "string")
        SetVar("EndDate", EndDate, "string")

        ConstructorQuery = ConstructorQuery.Replace("WHERE", "WHERE D.Date >= @StartDate AND D.Date <= @EndDate AND ")

        PullAndStripDS()

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

    Public Function SetArea(AreaKey As Integer) As Data.DataSet
        SetVar("AreaKey0", AreaKey, "int")

        Return SetAreas(New List(Of Integer) From {AreaKey})
    End Function

    Public Function SetAreas(AreasList As List(Of Integer)) As Data.DataSet
        LabelsHash.Clear()
        AreasHash.Clear()

        If AreasList Is Nothing OrElse AreasList.Count = 0 Then
            GroupDS = EmptyGroupDS
        Else
            If AreasList.Count > 1 Then
                SetVar("AreaKey0", 0, "int")
                LabelsToExclude.Clear()
            End If
            AreasToExclude = New List(Of Integer)(AreaKeysList) 'AreaKeysList should contains All Area keys for the Group

            ''remove element from AreasToExclude if arg AreasList contains the element
            ''this ways, AreasToExclude will have the AreaKeys that need to be excluded
            For Each AreaKey As Integer In AreaKeysList
                If AreasList.Contains(AreaKey) Then AreasToExclude.Remove(AreaKey)
            Next

            PullAndStripDS()
        End If

        Return GroupDS
    End Function

    Public Function SetGroup(GroupKey As String) As Data.DataSet
        SetVar("AreaKey0", 0, "int") 'reset AreaKey back to 0, which means All checklists pertaining to the new Group
        SetVar("GroupKey", GroupKey, "int")

        LabelsHash.Clear()
        AreasHash.Clear()
        AreasToExclude.Clear()

        PullAndStripDS()

        Return GroupDS
    End Function

    Public Function GetMaxFieldVals() As Dictionary(Of String, String)
        Return MaxFieldVals
    End Function

    Private Sub SetMaxFieldVals(KeyToTest As String, TestVal As String)
        If TestVal Is Nothing Then TestVal = String.Empty

        If TestVal.Length > MaxFieldVals(KeyToTest).Length Then
            MaxFieldVals(KeyToTest) = TestVal
        End If
    End Sub

    Private Sub PullAndStripDS()
        Dim GroupRC As Integer

        If GetVar("StartDate") Is Nothing OrElse GetVar("EndDate") Is Nothing Then Exit Sub

        GroupDS = GetMyDataSetParamQuery(ConstructorQuery, QueryConfig)
        GroupRC = GroupDS.Tables(0).Rows.Count - 1

        MaxFieldVals = New Dictionary(Of String, String) From { 'to ensure values in MaxFieldVals pertain to current configured dataset
           {"InputDate", String.Empty},
           {"StartDate", String.Empty},
           {"Value", String.Empty},
           {"InputOperator", String.Empty},
           {"Area", String.Empty},
           {"LabelKey", String.Empty},
           {"Label", String.Empty}
        }

        For I As Integer = 0 To GroupRC
            Dim GroupDR As Data.DataRow = GroupDS.Tables(0).Rows(I)
            Dim LabelKey As Integer = GroupDR("LabelKey")
            Dim AreaKey As Integer = GroupDR("AreaKey")
            Dim Inputs As Dictionary(Of Integer, Dictionary(Of String, String)) = LogAspx.GetInputs(GroupDR) 'in case DB Inputs field value is in the old JSON format, run it through LogAspxLibrary Class GetInputs function
            Dim ObjOfInterest As Dictionary(Of String, String)
            Dim InputDate As String
            Dim StartDate As String
            Dim Value As String
            Dim InputOperator As String
            Dim Area As String
            Dim Label As String

            StartDate = DbFormatting.DateField(GroupDR("Date")).Split(" ")(0) 'date only

            'initializing these 2 variables here, rather than the try catch block below
            'this is to ensure rows with labels or areas marked for deletion will be deleted
            Label = GroupDR("Label")
            Area = GroupDR("Area")

            If AreasToExclude.Contains(AreaKey) OrElse LabelsToExclude.Contains(LabelKey) Then
                GroupDR.Delete() 'mark row for deletion
                Continue For
            Else 'the logic below should NOT be ran on deleted rows
                If CInt(GetVar("GroupKey")) <> 0 Then 'If GroupKey has a value and is not 'All' (0), then consider adding to AreasList
                    If AreasHash.ContainsKey(AreaKey) = False Then AreasHash(AreaKey) = Area
                    If AreaKeysList.Contains(AreaKey) = False Then AreaKeysList.Add(AreaKey)

                    If LabelsHash.ContainsKey(LabelKey) = False Then LabelsHash(LabelKey) = Label
                    If LabelKeysList.Contains(LabelKey) = False Then LabelKeysList.Add(LabelKey)
                End If
            End If

            Try 'in case there is no instance of a log for a certain date (grouped reports are not guarenteed the same start and end dates)
                ObjOfInterest = Inputs(LabelKey)
                InputDate = DbFormatting.DateField(ObjOfInterest("Date"))
                Value = ObjOfInterest("Value")
                InputOperator = ObjOfInterest("Operator")
            Catch ex As Exception
                InputDate = String.Empty
                Value = String.Empty
                InputOperator = String.Empty
                Area = String.Empty
                Label = String.Empty
            End Try

            GroupDR("InputDate") = InputDate
            GroupDR("StartDate") = StartDate
            GroupDR("Value") = Value
            GroupDR("InputOperator") = InputOperator

            SetMaxFieldVals("LabelKey", GroupDR("LabelKey"))
            SetMaxFieldVals("Label", Label)
            SetMaxFieldVals("InputDate", InputDate)
            SetMaxFieldVals("StartDate", StartDate)
            SetMaxFieldVals("Value", Value)
            SetMaxFieldVals("InputOperator", InputOperator)
            SetMaxFieldVals("Area", Area)
        Next

        GroupDS.Tables(0).AcceptChanges() 'remove DataRow(s) marked for deletion permanently
    End Sub

    Public Function Override(Config As Dictionary(Of String, String), Mods As Dictionary(Of String, String), Optional UpdateDB As Boolean = False) As String
        Dim UnpackedInputs As New Dictionary(Of Integer, Dictionary(Of String, String))
        Dim DR As Data.DataRow
        Dim NewInputs As String

        QueryConfig("@LabelKey") = New Dictionary(Of String, String) From {
            {"value", Config("LabelKey")},
            {"typeOf", "int"}
        }
        QueryConfig("@Date") = New Dictionary(Of String, String) From {
            {"value", Config("Date")},
            {"typeOf", "string"}
        }

        DR = GetMyDataSetParamQuery("SELECT D.[Key], D.Inputs FROM [ALTS].[dbo].[T_LogLabel] L INNER JOIN [ALTS].[dbo].[T_LogData] D ON L.AreaKey=D.AreaKey WHERE L.[Key]=@LabelKey AND DATEDIFF(DAY, @Date, D.Date)=0", QueryConfig).Tables(0).Rows(0)
        UnpackedInputs = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(DR("Inputs"))

        For Each kvp As KeyValuePair(Of String, String) In Mods
            UnpackedInputs(Config("LabelKey"))(kvp.Key) = kvp.Value
        Next

        NewInputs = JsonSerializer.Serialize(UnpackedInputs)

        If UpdateDB Then
            QueryConfig("@Inputs") = New Dictionary(Of String, String) From {
                {"value", NewInputs},
                {"typeOf", "string"}
            }
            QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
                {"value", DR("Key")},
                {"typeOf", "int"}
            }

            ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] SET Inputs=@Inputs WHERE [Key]=@T_LogDataKey", QueryConfig)
            PullAndStripDS()
        End If

        Return NewInputs
    End Function

    Public Function GetDS() As Data.DataSet
        If GetVar("StartDate") IsNot Nothing AndAlso GetVar("EndDate") IsNot Nothing Then
            Return GroupDS
        Else
            Return EmptyGroupDS
        End If
    End Function

    Public Function OrderDSByDate() As Data.DataSet
        DS_OrderedByDate = True

        If GetVar("StartDate") IsNot Nothing AndAlso GetVar("EndDate") IsNot Nothing Then
            ConstructorQuery = ConstructorQuery.Replace("L.LabelOrder, D.Date", "D.Date, L.LabelOrder") 'prioritize Date over LabelOrder
            PullAndStripDS()
            Return GroupDS
        Else
            Return EmptyGroupDS
        End If
    End Function

    Public Function UndoOrderDSByDate() As Data.DataSet
        DS_OrderedByDate = False

        If GetVar("StartDate") IsNot Nothing AndAlso GetVar("EndDate") IsNot Nothing Then
            ConstructorQuery = ConstructorQuery.Replace("D.Date, L.LabelOrder", "L.LabelOrder, D.Date") 'prioritize LabelOrder over Date, just as og value of ConstructorQuery
            PullAndStripDS()
            Return GroupDS
        Else
            Return EmptyGroupDS
        End If
    End Function

    Public Function OrderedByDate() As Boolean
        Return DS_OrderedByDate
    End Function

    Public Function SetLabels(LabelList As List(Of Integer)) As Data.DataSet
        If LabelList Is Nothing OrElse LabelList.Count = 0 Then
            GroupDS = EmptyGroupDS
        Else
            LabelsToExclude = New List(Of Integer)(LabelKeysList) 'LabelKeysList should contains All label keys for the Area, but ONLY when 1 Area is left in GroupDS

            'remove element from LabelsToExclude2 if arg LabelList contains the element
            'this ways, LabelsToExclude2 will have the LabelKeys that need to be excluded
            For Each LabelKey As Integer In LabelKeysList
                If LabelList.Contains(LabelKey) Then LabelsToExclude.Remove(LabelKey)
            Next

            PullAndStripDS()
        End If

        Return GroupDS
    End Function

    Public Function GetLabels() As Dictionary(Of Integer, String)
        Return LabelsHash
    End Function

    Public Function GetAreas() As Dictionary(Of Integer, String)
        Return AreasHash
    End Function

    Public Function GetReportedAreas() As Dictionary(Of Integer, String)
        Dim AreasReportingHash As New Dictionary(Of Integer, String)(AreasHash)

        For Each AreaKey As Integer In AreasToExclude
            AreasReportingHash.Remove(AreaKey)
        Next

        Return AreasReportingHash
    End Function


    Public Function ColsWithIdenticalValues(Optional DS As Data.DataSet = Nothing) As List(Of String) 'Optional argument for testing purposes
        Dim IdenticalColsList As New List(Of String)
        Dim RC As Integer
        Dim DR As Data.DataRow
        Dim FirstRowValues As New Dictionary(Of String, String)

        If DS Is Nothing Then DS = GroupDS

        Try 'in case function receives empty DataSet
            RC = DS.Tables(0).Rows.Count
        Catch ex As Exception
            Return IdenticalColsList
        End Try

        For I As Integer = 0 To RC - 1
            DR = DS.Tables(0).Rows(I)

            For Each Field In DR.Table.Columns
                Dim FieldStr As String = Field.ToString()
                Dim FieldValue As String = If(IsDBNull(DR(FieldStr)), Nothing, DR(FieldStr))

                If I = 0 Then
                    FirstRowValues(FieldStr) = FieldValue
                    IdenticalColsList.Add(FieldStr)
                ElseIf FieldValue <> FirstRowValues(FieldStr) Then
                    IdenticalColsList.Remove(FieldStr)
                End If
            Next

        Next

        Return IdenticalColsList
    End Function
End Class

Public Class RecordSet
    Private GlobalDS As Data.DataSet

    Sub New(DS As Data.DataSet)
        GlobalDS = DS
    End Sub

    Sub New(Report As Report)
        GlobalDS = Report.GetDS()
    End Sub

    Public Function GetDS() As Data.DataSet
        Return GlobalDS
    End Function

    Private Function CheckDataSets(DS As Data.DataSet) As Boolean
        Dim GlobalDS_RC As Integer
        Dim DS_RC As Integer

        If GlobalDS Is Nothing OrElse DS Is Nothing Then Return False

        Try
            GlobalDS_RC = GlobalDS.Tables(0).Rows.Count
        Catch ex As Exception
            GlobalDS_RC = 0
        End Try

        Try
            DS_RC = DS.Tables(0).Rows.Count
        Catch ex As Exception
            DS_RC = 0
        End Try

        If GlobalDS_RC <> DS_RC Then
            Return False
        End If

        For I As Integer = 0 To DS_RC - 1
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim GlobalDR As Data.DataRow = GlobalDS.Tables(0).Rows(I)

            For Each Column As DataColumn In DR.Table.Columns
                Dim Field As String = Column.ColumnName
                Dim DR_Value As String = If(IsDBNull(DR(Field)), Nothing, DR(Field))
                Dim GlobalDR_Value As String = If(IsDBNull(GlobalDR(Field)), Nothing, GlobalDR(Field))

                If DR_Value <> GlobalDR_Value Then Return False
            Next
        Next

        Return True
    End Function

    Public Function DataSetsMatch(RS As RecordSet) As Boolean
        Dim DS As Data.DataSet = RS.GetDS()

        Return CheckDataSets(DS)
    End Function

    Public Function DataSetsMatch(DS As Data.DataSet) As Boolean
        Return CheckDataSets(DS)
    End Function
End Class

