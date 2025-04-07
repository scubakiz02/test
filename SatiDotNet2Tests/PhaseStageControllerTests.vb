Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

Public Class PhaseStageControllerTests
    Dim PhaseStageController = New PhaseStageController()

    <Fact>
    Public Sub AddNumbersTest1()
        Assert.True(PhaseStageController.ReturnTrue())
    End Sub

End Class