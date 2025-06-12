Imports System.Text.Json

Public Class PhaseController
    Inherits Security
    Private GlobalAreaKey As Integer
    Private PhaseOrdersToLabels As SortedDictionary(Of Integer, List(Of Integer))
    Private LabelToPhaseInfo As Dictionary(Of Integer, Dictionary(Of String, String))
    Private GlobalPhaseOrder As Integer
    Private DS As Data.DataSet
    Private RC As Integer

    Sub New()

    End Sub

    Sub New(AreaKey As Integer)
        CollectPhases(AreaKey)
    End Sub

    Sub New(AreaKey As Integer, Inputs As Dictionary(Of Integer, Dictionary(Of String, String)))
        CollectPhases(AreaKey)
        SetPhases(Inputs)
    End Sub

    Private Sub CollectPhases(AreaKey As Integer)
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))

        PhaseOrdersToLabels = New SortedDictionary(Of Integer, List(Of Integer))
        LabelToPhaseInfo = New Dictionary(Of Integer, Dictionary(Of String, String))

        GlobalAreaKey = AreaKey

        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaKey},
            {"typeOf", "int"}
        }
        DS = GetMyDataSetParamQuery("SELECT L.[Key] As LabelKey, L.Label, PhaseKey, P.PhaseOrder, P.Phase FROM [ALTS].[dbo].[T_LogLabel] L LEFT JOIN [ALTS].[dbo].[T_LogPhase] P ON P.[Key]=L.PhaseKey WHERE L.AreaKey=@AreaKey ORDER BY P.PhaseOrder", QueryConfig)
        RC = DS.Tables(0).Rows.Count

        For I As Integer = 0 To RC - 1
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim PhaseOrder As Integer
            Dim Phase As String
            Dim LabelKey As Integer
            Dim PhaseInfo As New Dictionary(Of String, String)

            LabelKey = DR("LabelKey")
            Try 'in case Label does NOT have Phase (NULL DB values)
                PhaseOrder = DR("PhaseOrder")
                Phase = DR("Phase")
            Catch ex As Exception
                '1 based indexing, to match associated primary key field value in DB
                'Thus, the question arises, what does phase 0 represent?
                'Phase 0 depicts labels that are not tied to a phase
                PhaseOrder = 0
                Phase = String.Empty
            End Try

            If PhaseOrdersToLabels.ContainsKey(PhaseOrder) = False Then
                PhaseOrdersToLabels(PhaseOrder) = New List(Of Integer)
            End If

            PhaseOrdersToLabels(PhaseOrder).Add(LabelKey)

            PhaseInfo("Phase") = Phase
            PhaseInfo("PhaseOrder") = PhaseOrder

            LabelToPhaseInfo(LabelKey) = PhaseInfo
        Next
    End Sub

    Private Function PhaseOrderForLabel(LabelKey As Integer) As Integer
        'SetPhases() sub takes a dictionary as an argument
        'The keys of the dictionary (LabelKeys) are not guarenteed to be ordered by PhaseOrder
        'Therefore, this function will take a LabelKey as an argument, and return its PhaseOrder
        Return LabelToPhaseInfo(LabelKey)("PhaseOrder")
    End Function

    Private Sub SetPhases(Inputs As Dictionary(Of Integer, Dictionary(Of String, String)))
        If PhaseOrdersToLabels.Count = 0 Then 'this means checklist does NOT contain Phases
            Exit Sub
        End If

        GlobalPhaseOrder = PhaseOrdersToLabels.Keys.First()

        For Each LabelInput As KeyValuePair(Of Integer, Dictionary(Of String, String)) In Inputs
            Dim LabelKey As Integer = LabelInput.Key
            Dim Input As Dictionary(Of String, String) = LabelInput.Value
            Dim InputValue As String = Input("Value")

            If String.IsNullOrEmpty(InputValue) = False Then
                PhaseOrdersToLabels(PhaseOrderForLabel(LabelKey)).Remove(LabelKey)
            End If
        Next

        'if GlobalPhaseOrder were to be incremented in the for loop above, it would be complex to track proper value for GlobalPhaseOrder
        'therefore, iterating through PhaseOrdersToLabels, a SortedDictionary, to increment GlobalPhaseOrder in counting order (1, 2, 3, 4, etc.)
        For Each PhaseOrderToLabels As KeyValuePair(Of Integer, List(Of Integer)) In PhaseOrdersToLabels
            If PhaseOrderToLabels.Value.Count = 0 AndAlso GlobalPhaseOrder < PhaseOrdersToLabels.Count Then
                GlobalPhaseOrder += 1
            End If
        Next
    End Sub

    Public Function GetPhase() As Integer
        Dim Res As Integer

        If PhaseOrdersToLabels Is Nothing Then
            Return Nothing
        ElseIf GlobalPhaseOrder = 0 Then
            'phasing enable/disable logic starts at index 1
            'thus, even though the non phased labels are NOT all filled out, the return should be 1
            Res += 1
        Else
            Res = GlobalPhaseOrder
        End If

        Return Res
    End Function

    Public Function GetPhases() As Dictionary(Of Integer, Dictionary(Of String, String))
        If LabelToPhaseInfo.Count = 0 Then Return Nothing
        Return LabelToPhaseInfo
    End Function

    Public Function GetDetachedLabels(AreaKey As String, Optional TestDS As Data.DataSet = Nothing) As Dictionary(Of Integer, String)
        'what does 'Detached' mean?
        'Great question!
        'it's an input (record in T_LogLabel) with a NULL PhaseKey field value of a PM/Checklist that has bundled inputs
        Dim InputsDS As Data.DataSet
        Dim Res As New Dictionary(Of Integer, String)

        If AreaKey IsNot Nothing Then
            If TestDS Is Nothing Then
                Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
                {"@AreaKey", GetParamVarHash(AreaKey, "int")}
            }
                InputsDS = GetMyDataSetParamQuery("SELECT L.[Key] As LabelKey, Label, PhaseKey FROM [ALTS].[dbo].[T_LogLabel] L LEFT JOIN [ALTS].[dbo].[T_LogPhase] P ON L.PhaseKey=P.[Key] WHERE L.AreaKey=@AreaKey ORDER BY P.PhaseOrder, LabelOrder", QueryConfig)
            Else

                InputsDS = TestDS
            End If

            For Each InputsDR As Data.DataRow In InputsDS.Tables(0).Rows
                If IsDBNull(InputsDR("PhaseKey")) Then
                    Res(InputsDR("LabelKey")) = InputsDR("Label")
                End If
            Next
        End If

        Return Res
    End Function

    Public Function DeletePhaseOrGroup(PhaseOrGroupKey As String, Optional InvocateAsTest As Boolean = False) As Dictionary(Of String, String)
        '1) Delete relevant record in T_LogPhase;
        '2) Update PhaseKey field values to NULL for relevant records in T_LogLabel
        Dim Res As New Dictionary(Of String, String)
        Dim SqlQuery As String
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))

        If PhaseOrGroupKey Is Nothing Then
            Res("Success") = False
            Return Res
        End If

        QueryConfig("@PhaseOrGroupKey") = GetParamVarHash(PhaseOrGroupKey, "int")
        SqlQuery = "UPDATE [ALTS].[dbo].[T_LogLabel] SET PhaseKey=NULL WHERE PhaseKey=@PhaseOrGroupKey; DELETE FROM [ALTS].[dbo].[T_LogPhase] WHERE [Key]=@PhaseOrGroupKey;"

        If InvocateAsTest Then
            Res("QueryConfig") = JsonSerializer.Serialize(QueryConfig)
            Res("SqlQuery") = SqlQuery
            Res("Success") = True
        Else
            Res("Success") = If(ExecuteSqlParamQuery(SqlQuery, QueryConfig) Is Nothing, False, True)
        End If

        Return Res
    End Function

    Public Function AssignPhase(LabelKey As String, PhaseKey As String, Optional InvocateAsTest As Boolean = False) As Dictionary(Of String, String)
        Dim Res As New Dictionary(Of String, String)
        Dim SqlQuery As String
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))

        If LabelKey Is Nothing OrElse Object.ReferenceEquals(PhaseKey, String.Empty) Then 'using Object.ReferenceEquals so when PhaseKey is nothing, the comparison return false
            Res("Success") = False
            Return Res
        End If

        QueryConfig("@LabelKey") = GetParamVarHash(LabelKey, "int")
        QueryConfig("@PhaseKey") = GetParamVarHash(PhaseKey, "int")
        SqlQuery = "UPDATE [ALTS].[dbo].[T_LogLabel] SET PhaseKey=@PhaseKey WHERE [Key]=@LabelKey"

        If InvocateAsTest Then
            Res("QueryConfig") = JsonSerializer.Serialize(QueryConfig)
            Res("SqlQuery") = SqlQuery
            Res("Success") = True
        Else
            Res("Success") = If(ExecuteSqlParamQuery(SqlQuery, QueryConfig) Is Nothing, False, True)
        End If


        Return Res
    End Function

    Public Function GroupsOrPhasesInUse(AreaKey As String) As Boolean
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim LabelsWithPhaseCount As String

        If AreaKey Is Nothing Then Return False

        QueryConfig("@AreaKey") = GetParamVarHash(AreaKey, "int")
        LabelsWithPhaseCount = GetSingleDbField("SELECT COUNT(*) As LabelsWithNullPhaseCount FROM [ALTS].[dbo].[T_LogLabel] GROUP BY PhaseKey, AreaKey HAVING PhaseKey IS NOT NULL And AreaKey=@AreaKey", QueryConfig, "LabelsWithNullPhaseCount")

        If LabelsWithPhaseCount IsNot Nothing Then Return True

        Return False
    End Function

    Public Function GetLabel_Idx(LabelKey As String, Optional TestDS As Data.DataSet = Nothing) As Integer
        Dim Idx As Integer = -1
        Dim DS As Data.DataSet

        If LabelKey Is Nothing Then Return Idx

        If TestDS Is Nothing Then
            Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
                {"@LabelKey", GetParamVarHash(LabelKey, "int")}
            }
            DS = GetMyDataSetParamQuery("SELECT L.[Key] As LabelKey, Label, P.[Key] As PhaseKey, Phase, PhaseOrder, LabelOrder FROM [ALTS].[dbo].[T_LogLabel] L LEFT JOIN [ALTS].[dbo].[T_LogPhase] P On L.PhaseKey=P.[Key] WHERE L.AreaKey=(SELECT AreaKey FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey) ORDER BY P.PhaseOrder, L.LabelOrder", QueryConfig)
        Else
            DS = TestDS
        End If

        For I As Integer = 0 To DS.Tables(0).Rows.Count
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)

            If DR("LabelKey") = LabelKey Then
                Idx = I
                Exit For
            End If
        Next

        Return Idx
    End Function
End Class
