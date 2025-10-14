Imports System.Globalization
Imports System.Text.Json
Imports System.Collections.Specialized

Public Class Report
    Inherits Security

    Private GroupDS As Data.DataSet
    Private EmptyGroupDS As New Data.DataSet()
    Private QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Dim LogAspx As New LogAspxLibrary
    Private DbFormatting As New Format()
    Public ReadOnly InvalidDateMessage As String = "Error: Invalid date"
    Public ReadOnly OutOfRangeDateMessage As String = "Error: Out of range"
    Private LabelsToExclude As New List(Of Integer)
    Private AreasToExclude As New List(Of Integer)
    Private LabelKeysList As New List(Of Integer)
    Private AreaKeysList As New List(Of Integer)
    Private DS_OrderedByDate As Boolean = False

    'Private ConstructorQuery As String = "SELECT A.[Key] As AreaKey, A.Area, A.[Key] As AreaKey, " &
    '    "D.[Key] As DataKey, D.Operator, " &
    '    "L.[Key] As LabelKey, L.FieldType, Case WHEN L.Range Is Not NULL THEN L.Label + ' | ' + L.Range + Case WHEN L.UnitKey Is Not NULL THEN ' ' + U.Unit Else '' End Else L.Label End As Label, " &
    '    "P.Phase, " &
    '    "FORMAT(D.Date, 'MM/dd/yyyy') As Date, D.Inputs, '' As Value, '' As StartDate, '' As InputDate, '' As InputOperator " &
    '    "FROM [ALTS].[dbo].[T_LogLabel] L " &
    '    "INNER JOIN [ALTS].[dbo].[T_LogArea] A On L.AreaKey=A.[Key] " &
    '    "RIGHT JOIN [ALTS].[dbo].[T_LogData] D On D.AreaKey=L.AreaKey " &
    '    "LEFT JOIN [ALTS].[dbo].[T_LogUnit] U On L.UnitKey=U.[Key] " &
    '    "LEFT JOIN [ALTS].[dbo].[T_LogPhase] P On L.PhaseKey=P.[Key] " &
    '    "WHERE (A.[Key]=@AreaKey0 Or @AreaKey0=0) And (A.GroupKey=@GroupKey Or (@GroupKey=0 And A.GroupKey Is Not NULL)) " &
    '    "ORDER BY A.Area, P.PhaseOrder, L.LabelOrder, D.Date"
    Private _ConstructorQuery As String
    Private __ConstructorQueryShell As String = "SELECT A.[Key] As AreaKey, A.Area, " &
        "D.[Key] As DataKey, D.Operator, " &
        "L.[Key] As LabelKey, L.Range, L.FieldType, Case WHEN L.Range Is Not NULL THEN L.Label + ' | ' + L.Range + Case WHEN L.UnitKey Is Not NULL THEN ' ' + U.Unit Else '' End Else L.Label End As Label, " &
        "U.Unit, " &
        "P.Phase, " &
        "FORMAT(D.Date, 'MM/dd/yyyy') As StartDate, FORMAT(D.Date, 'MM/dd/yyyy') As Date, D.Inputs, '' As Value, '' As InputDate, '' As InputOperator " &
        "FROM [ALTS].[dbo].[T_LogLabel] L " &
        "INNER JOIN [ALTS].[dbo].[T_LogArea] A On L.AreaKey=A.[Key] " &
        "RIGHT JOIN [ALTS].[dbo].[T_LogData] D On D.AreaKey=L.AreaKey " &
        "LEFT JOIN [ALTS].[dbo].[T_LogUnit] U On L.UnitKey=U.[Key] " &
        "LEFT JOIN [ALTS].[dbo].[T_LogPhase] P On L.PhaseKey=P.[Key] "

    Private _TabulatorConfig As OrderedDictionary 'using OrderedDictionary to preserve order of insertion
    Private _PmInput As New PmInput()

    Public Sub New()

    End Sub

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

        If Config.ContainsKey("GroupKey") AndAlso Config.ContainsKey("StartDate") AndAlso Config.ContainsKey("EndDate") Then
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
        Dim DT As New Data.DataTable
        DT.Columns.Add("Operator", GetType(String))
        Dim Ds As New Data.DataSet
        Ds.Tables.Add(DT)

        Dim OperatorsHashset As New HashSet(Of String) 'using hashset b/c they do not store duplicate values
        For Each Dr As Data.DataRow In GroupDS.Tables(0).Rows
            'iterate through each row in report dataset and look for unique operators
            Dim InputOperator As Object = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Dr("InputOperator").ToLower())
            If OperatorsHashset.Contains(InputOperator) = False Then
                'encountered a new operator
                If InputOperator = String.Empty Then Continue For 'skip blank operators

                Dim NewDr As Data.DataRow = DT.NewRow()
                NewDr("Operator") = InputOperator
                DT.Rows.Add(NewDr)
            End If

            OperatorsHashset.Add(InputOperator)
        Next

        Return Ds
    End Function

    Public Function SetDateRange(StartDate As String, EndDate As String) As Data.DataSet
        Dim DateBoundsQueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        DateBoundsQueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", GetVar("AreaKey0")},
            {"typeOf", "int"}
        }

        SetVar("StartDate", StartDate, "string")
        SetVar("EndDate", EndDate, "string")

        Try
            If GetVar("GroupKey") = 0 Then Throw New Exception("")
            If String.IsNullOrEmpty(StartDate) OrElse String.IsNullOrEmpty(EndDate) Then Throw New Exception("")
            PullAndStripDS()
        Catch ex As Exception
            GroupDS = EmptyGroupDS
        End Try

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

    Private Function GetLabelKeys(AreaKey As Integer) As List(Of Integer)
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@AreaKey", GetParamVarHash(AreaKey, "int")}
        }
        Dim Ds As Data.DataSet = GetMyDataSetParamQuery("SELECT [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey", SqlConfig)

        Dim LabelKeys As New List(Of Integer)
        For Each DR As Data.DataRow In Ds.Tables(0).Rows
            LabelKeys.Add(DR("Key"))
        Next
        Return LabelKeys
    End Function

    Public Function SetAreas(AreasList As List(Of Integer)) As Data.DataSet
        If AreasList Is Nothing OrElse AreasList.Count = 0 Then
            GroupDS = EmptyGroupDS
        Else
            If AreasList.Count = 1 Then
                Dim AreaKey As Integer = AreasList(0)
                LabelKeysList = GetLabelKeys(AreaKey)
            Else
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

    Private Function GetAreaKeys(GroupKey As Integer) As List(Of Integer)
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@GroupKey", GetParamVarHash(GroupKey, "int")}
        }
        Dim Ds As Data.DataSet = GetMyDataSetParamQuery("SELECT [Key] FROM [ALTS].[dbo].[T_LogArea] WHERE GroupKey=@GroupKey", SqlConfig)

        Dim AreaKeys As New List(Of Integer)
        For Each DR As Data.DataRow In Ds.Tables(0).Rows
            AreaKeys.Add(DR("Key"))
        Next
        Return AreaKeys
    End Function

    Public Function SetGroup(GroupKey As String) As Data.DataSet
        SetVar("AreaKey0", 0, "int") 'reset AreaKey back to 0, which means All pm/checklists associated with the new GroupKey are available
        SetVar("GroupKey", GroupKey, "int")

        AreasToExclude.Clear()
        AreaKeysList = GetAreaKeys(CInt(GroupKey))

        PullAndStripDS()

        Return GroupDS
    End Function

    Public Function BuildWhereClause(Config As Dictionary(Of String, Object), ByRef SqlConfig As Dictionary(Of String, Dictionary(Of String, String))) As String
        Dim SqlWhereClause As String = ""

        'this function should never be called if Config does not have values for 'GroupKey', 'StartDate', and 'EndDate' kvp
        Dim GroupKey As Integer = If(Config.ContainsKey("GroupKey"), Config("GroupKey"), 0)
        Dim StartDate As Object = If(Config.ContainsKey("StartDate"), Config("StartDate"), DBNull.Value)
        Dim EndDate As Object = If(Config.ContainsKey("EndDate"), Config("EndDate"), DBNull.Value)

        'GroupKey
        SqlConfig("@GroupKey") = GetParamVarHash(GroupKey, "int")
        SqlWhereClause += "WHERE A.GroupKey=@GroupKey "

        'StartDate and EndDate
        SqlConfig("@StartDate") = GetParamVarHash(StartDate, "string")
        SqlConfig("@EndDate") = GetParamVarHash(EndDate, "string")
        SqlWhereClause += "And D.Date >= @StartDate And D.Date <= @EndDate "

        'AreaKey(s)
        Dim AreasToExclude2 As List(Of Integer) = If(Config.ContainsKey("AreasToExclude"), Config("AreasToExclude"), New List(Of Integer))
        SqlWhereClause += BuildNotInClause(AreasToExclude2, "And A.[Key] Not In (", "@AreaKey", SqlConfig)

        'LabelKey(s)
        Dim LabelsToExclude2 As List(Of Integer) = If(Config.ContainsKey("LabelsToExclude"), Config("LabelsToExclude"), New List(Of Integer))
        SqlWhereClause += BuildNotInClause(LabelsToExclude2, "And L.[Key] Not In (", "@LabelKey", SqlConfig)

        Return SqlWhereClause
    End Function

    Private Function BuildNotInClause(KeysToExclude As List(Of Integer), SqlPrefix As String, ParamKeyPrefix As String, ByRef SqlConfig As Dictionary(Of String, Dictionary(Of String, String))) As String
        Dim SqlNotInClause As String = ""

        If KeysToExclude.Count > 0 Then
            SqlNotInClause += SqlPrefix
            For I As Integer = 0 To KeysToExclude.Count - 1
                Dim Key As Integer = KeysToExclude(I)
                Dim ParamKey As String = ParamKeyPrefix & Key.ToString()

                'update sql config passed by reference to function
                SqlConfig(ParamKey) = GetParamVarHash(Key, "int")

                'add to sql not in clause that will be returned by this function
                SqlNotInClause += ParamKey
                If I = KeysToExclude.Count - 1 Then
                    SqlNotInClause += ") "
                Else
                    SqlNotInClause += ", "
                End If
            Next
        End If

        Return SqlNotInClause
    End Function

    Private Function BuildOrderByClause(IsDatasetOrderedByDate As Boolean) As String
        If IsDatasetOrderedByDate Then Return "ORDER BY A.Area, P.PhaseOrder, D.Date, L.LabelOrder"
        Return "ORDER BY A.Area, P.PhaseOrder, L.LabelOrder, D.Date"
    End Function

    Public Sub RefreshDS()
        PullAndStripDS()
    End Sub

    Private Sub PullAndStripDS()
        If GetVar("StartDate") Is Nothing OrElse GetVar("EndDate") Is Nothing Then Exit Sub

        Dim WhereClauseConfig As Dictionary(Of String, Object) = New Dictionary(Of String, Object) From {
            {"GroupKey", CInt(GetVar("GroupKey"))},
            {"StartDate", GetVar("StartDate")},
            {"EndDate", GetVar("EndDate")},
            {"AreasToExclude", AreasToExclude},
            {"LabelsToExclude", LabelsToExclude}
        }
        _ConstructorQuery = __ConstructorQueryShell & BuildWhereClause(WhereClauseConfig, QueryConfig) & BuildOrderByClause(DS_OrderedByDate)
        GroupDS = GetMyDataSetParamQuery(_ConstructorQuery, QueryConfig)

        Dim GroupRC As Integer = GroupDS.Tables(0).Rows.Count - 1
        _TabulatorConfig = New OrderedDictionary()
        For I As Integer = 0 To GroupRC
            Dim GroupDR As Data.DataRow = GroupDS.Tables(0).Rows(I)
            Dim AreaKey As Integer = GroupDR("AreaKey")
            Dim StartDate As String = GroupDR("StartDate")
            Dim Area As String = GroupDR("Area")
            Dim Label As String = GroupDR("Label")
            Dim LabelKey As Integer = GroupDR("LabelKey")
            Dim InputDate As String
            Dim Value As String
            Dim InputOperator As String
            Try
                'in case there is no instance of a log for a certain date (grouped reports are not guarenteed the same start and end dates)
                Try
                    'records before 04/2025 follow a different structure than Dictionary(Of Integer, Dictionary(Of String, String))
                    'records before 04/2025 are simply test records, so this try catch loop ensures they are not processed in the report dataset
                    Dim InputsStringified As String = GroupDR("Inputs")
                    Dim Inputs As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(InputsStringified)
                    Dim ObjOfInterest As Dictionary(Of String, String) = Inputs(LabelKey)
                    InputDate = DbFormatting.DateField(ObjOfInterest("Date"))
                    Value = ObjOfInterest("Value")
                    InputOperator = ObjOfInterest("Operator")
                Catch ex As Exception
                    Continue For
                End Try
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

            '========= tabulator config =========
            If _TabulatorConfig.Contains(Area) = False Then
                _TabulatorConfig.Add(Area, New List(Of Dictionary(Of String, Object))())
            End If

            Dim TabulatorRowConfig As New Dictionary(Of String, Object)
            TabulatorRowConfig("checklist") = Area
            TabulatorRowConfig("input") = Label
            TabulatorRowConfig("datakey") = GroupDR("DataKey")
            TabulatorRowConfig("labelkey") = LabelKey
            TabulatorRowConfig("startDateAt") = StartDate
            TabulatorRowConfig("inputDateAt") = InputDate
            TabulatorRowConfig("operator") = InputOperator

            Dim FieldType As String = _PmInput.GetFieldType(LabelKey)
            TabulatorRowConfig("fieldtype") = FieldType
            TabulatorRowConfig("value") = GetFieldTypeValue(Value, FieldType)

            _TabulatorConfig(Area).Add(TabulatorRowConfig)
        Next
    End Sub

    Public Function GetTabulatorConfig() As OrderedDictionary
        Return _TabulatorConfig
    End Function

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
            PullAndStripDS()
            Return GroupDS
        Else
            Return EmptyGroupDS
        End If
    End Function

    Public Function UndoOrderDSByDate() As Data.DataSet
        DS_OrderedByDate = False

        If GetVar("StartDate") IsNot Nothing AndAlso GetVar("EndDate") IsNot Nothing Then
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

    Private Sub AddRowBreak(ListOfArrays As List(Of String()))
        ListOfArrays.Add({"", "", "", "", "", "default"})
    End Sub

    Private Function IsValueInMatrix(Value As Object, ActiveSheetData As List(Of String())) As Boolean
        Return ActiveSheetData.Any(Function(Row) Row.Any(Function(Cell) Cell.Contains(Value)))
    End Function

    Private Sub WriteExcelBase(A1 As String, ByRef MasterCollection As Dictionary(Of String, List(Of String())), ByRef SheetName As String)
        Dim DataMatrix As List(Of String()) = New List(Of String())
        DataMatrix.Add({A1, "", "", "", "", "A1"})
        AddRowBreak(DataMatrix)
        AddRowBreak(DataMatrix)
        DataMatrix.Add({"Item", "Value", "Start Date", "Input Date", "Operator", "bold"})

        MasterCollection(SheetName) = DataMatrix
    End Sub

    Public Function GetFieldTypeValue(Value As Object, FieldType As Object) As String
        Try
            'try catch block in case fieldtype is db null
            If FieldType = "Checkbox" Then
                Return BitToSymbol(Value)
            ElseIf FieldType = "DP" Then
                'what is DP? Great question!
                'DP is distribution pumps. They are paired. Only 1 needs to be running at a time
                'the interface is 2 checkbox controls next to one another
                Dim DpValues As String()
                Dim Dp1 As String
                Dim Dp2 As String
                Try
                    'in case arg 1 is an empty string
                    DpValues = Value.Split("/")
                    Dp1 = BitToSymbol(DpValues(0))
                    Dp2 = BitToSymbol(DpValues(1))
                Catch ex As Exception
                    Dp1 = BitToSymbol("0")
                    Dp2 = BitToSymbol("0")
                End Try

                Return Dp1 & "/" & Dp2
            End If
        Catch ex As Exception
            Return Value
        End Try

        Return Value
    End Function

    Private Function BitToSymbol(Bit As Object) As String
        If Bit = "1" Then Return "✔"
        Return "✘"
    End Function

    Public Function GetExcelData(ReportInst As Report) As Dictionary(Of String, List(Of String()))
        Dim IsDatasetOrderedByDate As Boolean = ReportInst.OrderedByDate()
        Dim ExcelCollection As New Dictionary(Of String, List(Of String()))

        Dim DS_Final As Data.DataSet = ReportInst.GetDS()
        Dim DsRc As Integer = DS_Final.Tables(0).Rows.Count
        For I As Integer = 0 To DsRc - 1
            Dim DR_Final As Data.DataRow = DS_Final.Tables(0).Rows(I)
            Dim Area As String = DR_Final("Area")
            Dim FieldType As Object = DR_Final("FieldType")
            Dim DrInput As String = DR_Final("Label")
            Dim Phase As Object = DR_Final("Phase")
            Dim Value As Object = GetFieldTypeValue(DR_Final("Value"), FieldType)
            Dim StartDate As Object = DR_Final("StartDate")
            Dim InputDate As Object = DR_Final("InputDate")
            Dim InputOperator As Object = DR_Final("InputOperator")

            If IsDBNull(InputDate) Then
                InputDate = String.Empty
            End If

            'row break logic
            If IsDatasetOrderedByDate Then
                'dataset is ordered by date
                Dim ActiveSheetName As String = GenerateActiveSheetName(Area, StartDate)
                Dim ActiveSheetNames As List(Of String) = ExcelCollection.Keys.ToList()
                If ActiveSheetNames.Any(Function(SheetName) SheetName.Contains(StartDate)) = False Then
                    'discovered a new start date
                    WriteExcelBase(Area, ExcelCollection, ActiveSheetName)
                End If

                Dim DataMatrixOfFocus As List(Of String()) = ExcelCollection(ActiveSheetName)
                If IsDBNull(Phase) = False Then
                    If IsValueInMatrix(Phase, DataMatrixOfFocus) = False Then
                        'discovered a new phase
                        AddRowBreak(DataMatrixOfFocus)
                        DataMatrixOfFocus.Add({Phase, "", "", "", "", "bold"})
                    End If
                End If
                DataMatrixOfFocus.Add({DrInput, Value, StartDate, InputDate, InputOperator, "default"})
            Else
                'dataset is ordered by input
                Dim ActiveSheetName As String = GenerateActiveSheetName(Area)
                Dim ActiveSheetNames As List(Of String) = ExcelCollection.Keys.ToList()
                If ActiveSheetNames.Contains(ActiveSheetName) = False Then
                    'discovered a new pm/checklist
                    WriteExcelBase(Area, ExcelCollection, ActiveSheetName)
                End If

                Dim DataMatrixOfFocus As List(Of String()) = ExcelCollection(ActiveSheetName)
                Try
                    If IsDBNull(Phase) Then Throw New SamePhaseException()

                    If IsValueInMatrix(Phase, DataMatrixOfFocus) = False Then
                        'discovered a new phase
                        AddRowBreak(DataMatrixOfFocus)
                        DataMatrixOfFocus.Add({Phase, "", "", "", "", "bold"})
                    Else
                        Throw New SamePhaseException()
                    End If
                Catch ex As SamePhaseException
                    Dim IsExcelBase As Boolean = If(DataMatrixOfFocus.Count = 4, True, False)
                    If IsExcelBase = False AndAlso IsValueInMatrix(DrInput, DataMatrixOfFocus) = False Then
                        'discovered a new input beyond the 1st one
                        AddRowBreak(DataMatrixOfFocus)
                    End If
                End Try
                DataMatrixOfFocus.Add({DrInput, Value, StartDate, InputDate, InputOperator, "default"})
            End If
        Next

        Return ExcelCollection
    End Function

    Public Function GenerateActiveSheetName(ActiveSheetName As String, Optional DateNoTime As String = Nothing) As String
        'In Excel, an active sheet name (Or any worksheet name) can be up to 31 characters long.
        'Other restrictions:
        '   Cannot contain:  \ / ? * [ ] 
        '   Cannot be blank.
        '   Cannot be the same as another sheet name in the same workbook (case-insensitive).
        '   Cannot start Or end with an apostrophe (') — although apostrophes can be inside the name.

        Dim AdjustedSheetName As String

        If DateNoTime Is Nothing Then
            If ActiveSheetName.Length > 31 Then
                AdjustedSheetName = ActiveSheetName.Substring(0, 28) + "..."
            Else
                AdjustedSheetName = ActiveSheetName
            End If
        Else
            Dim DateNoTimeExtension As String = " (" & DateNoTime & ")"
            Dim MaxCharCount As Integer = 31 - DateNoTimeExtension.Length

            If ActiveSheetName.Length > MaxCharCount Then
                'cut off pm or checklist name with ellipses to add date on end (Ex: SC1 Fume Sc... (08/14/2025))
                Dim SheetNameWithoutDate As String = ActiveSheetName.Substring(0, (MaxCharCount - 3)) & "..."
                AdjustedSheetName = SheetNameWithoutDate + DateNoTimeExtension
            Else
                AdjustedSheetName = ActiveSheetName + DateNoTimeExtension
            End If

        End If

        Return AdjustedSheetName
    End Function

    Private Sub ConfigBounds(DbRange As Object, ByRef Config As Dictionary(Of String, Object))
        Dim LowerBound As String = Nothing
        Dim UpperBound As String = Nothing
        Try
            'in case range is db null or not formatted correctly
            If DbRange.Contains("-") Then
                Dim Ranges As String() = DbRange.Split("-")
                LowerBound = Ranges(0)
                UpperBound = Ranges(1)
            ElseIf DbRange.Contains("<") Then
                LowerBound = Nothing
                UpperBound = DbRange.Substring(1) 'remove the first char ('<')
            ElseIf DbRange.Contains(">") Then
                LowerBound = DbRange.Substring(1) 'remove the first char ('>')
                UpperBound = Nothing
            End If
        Catch ex As Exception
            LowerBound = Nothing
            UpperBound = Nothing
        End Try
        Config("lowerBound") = LowerBound
        Config("upperBound") = UpperBound
    End Sub


    Private Sub ConfigChartTitles(DbUnit As Object, ByRef Config As Dictionary(Of String, Object))
        Dim xAxisTitle As String = Nothing
        Dim yAxisTitle As String = Nothing
        Try
            'in case unit is db null or not formatted correctly
            xAxisTitle = "Input Date"
            yAxisTitle = DbUnit
        Catch ex As Exception
            xAxisTitle = Nothing
            yAxisTitle = Nothing
        End Try
        Config("xAxisTitle") = xAxisTitle
        Config("yAxisTitle") = yAxisTitle
    End Sub

    Public Function GetLineChartConfig(LabelKey As Integer, Optional FakeDs As Data.DataSet = Nothing) As Dictionary(Of String, Object)
        Dim LineChartConfig As New Dictionary(Of String, Object)
        Dim Ds As Data.DataSet

        If FakeDs Is Nothing Then
            Ds = GetDS()
        Else
            Ds = FakeDs
        End If

        Dim xAxisLabelsList As New List(Of String)
        Dim DataPointsList As New List(Of String)
        For I As Integer = 0 To Ds.Tables(0).Rows.Count - 1
            Dim Dr As Data.DataRow = Ds.Tables(0).Rows(I)

            If Dr("LabelKey") = LabelKey Then
                'set config values
                Dim DateNoTimeAt As String = Dr("StartDate")
                xAxisLabelsList.Add(DateNoTimeAt)

                Dim InputValue As Object = Dr("Value")
                If InputValue = "" Then InputValue = Nothing
                DataPointsList.Add(InputValue)

                If LineChartConfig.ContainsKey("graphTitle") = False Then
                    'these config kvp only need to be set once
                    LineChartConfig("graphTitle") = Dr("Label")
                    ConfigChartTitles(Dr("Unit"), LineChartConfig)
                    ConfigBounds(Dr("Range"), LineChartConfig)
                End If
            End If
        Next
        LineChartConfig("xAxisLabels") = xAxisLabelsList.ToArray()
        LineChartConfig("data") = DataPointsList.ToArray()
        LineChartConfig("graphTitle") += " (" & xAxisLabelsList(0) & " - " & xAxisLabelsList(xAxisLabelsList.Count - 1) & ")" 'this assumes xAxisLabels is sorted from earliest to latest date no time value

        Return LineChartConfig
    End Function

    'use these functions below for testing purposes only!!!!!!!!!
    Public Sub RebindGroupDS(DS As Data.DataSet)
        SetVar("StartDate", "googoo gaagaa", "string")
        SetVar("EndDate", "i'm going to cry", "string")
        GroupDS = DS
    End Sub

    Public Sub RebindIsOrderedByDate(TestValue As Boolean)
        DS_OrderedByDate = TestValue
    End Sub
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

Public Class SamePhaseException
    Inherits Exception
End Class

