Public Class PhaseController
    Inherits Security
    Private GlobalAreaKey As Integer
    Private PhaseOrderToLabels As Dictionary(Of Integer, List(Of Integer))
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

        PhaseOrderToLabels = New Dictionary(Of Integer, List(Of Integer))
        LabelToPhaseInfo = New Dictionary(Of Integer, Dictionary(Of String, String))
        GlobalPhaseOrder = 1

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

            If PhaseOrderToLabels.ContainsKey(PhaseOrder) = False Then
                PhaseOrderToLabels(PhaseOrder) = New List(Of Integer)
            End If

            PhaseOrderToLabels(PhaseOrder).Add(LabelKey)

            PhaseInfo("Phase") = Phase
            PhaseInfo("PhaseOrder") = PhaseOrder

            LabelToPhaseInfo(LabelKey) = PhaseInfo
        Next
    End Sub

    Private Sub SetPhases(Inputs As Dictionary(Of Integer, Dictionary(Of String, String)))
        If PhaseOrderToLabels.Count = 0 Then 'this means checklist does NOT contain Phases
            Exit Sub
        End If

        For Each LabelInput As KeyValuePair(Of Integer, Dictionary(Of String, String)) In Inputs
            Dim LabelKey As Integer = LabelInput.Key
            Dim Input As Dictionary(Of String, String) = LabelInput.Value
            Dim InputValue As String = Input("Value")

            If String.IsNullOrEmpty(InputValue) = False Then
                PhaseOrderToLabels(GlobalPhaseOrder).Remove(LabelKey)

                If PhaseOrderToLabels(GlobalPhaseOrder).Count = 0 Then
                    GlobalPhaseOrder += 1
                End If
            End If
        Next
    End Sub

    Public Function GetPhase() As Integer
        If PhaseOrderToLabels Is Nothing Then Return Nothing
        Return GlobalPhaseOrder
    End Function

    Public Function GetPhases() As Dictionary(Of Integer, Dictionary(Of String, String))
        If LabelToPhaseInfo.Count = 0 Then Return Nothing
        Return LabelToPhaseInfo
    End Function
End Class
