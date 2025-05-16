Public Class PhaseController
    Inherits Security
    Private GlobalAreaKey As Integer
    Private PhaseOrdersToLabels As SortedDictionary(Of Integer, List(Of Integer))
    Private LabelToPhaseInfo As Dictionary(Of Integer, Dictionary(Of String, String))
    Private GlobalPhaseOrder As Integer
    Private DS As Data.DataSet
    Private RC As Integer

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
        GlobalPhaseOrder = 1 '1 based indexing, to match associated primary key field value in DB

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
                Continue For
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
        If PhaseOrdersToLabels Is Nothing Then Return Nothing
        Return GlobalPhaseOrder
    End Function

    Public Function GetPhases() As Dictionary(Of Integer, Dictionary(Of String, String))
        If LabelToPhaseInfo.Count = 0 Then Return Nothing
        Return LabelToPhaseInfo
    End Function
End Class
