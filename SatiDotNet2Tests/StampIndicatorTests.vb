Imports Xunit
Imports SatiDotNet2.Library

Public Class StampIndicatorTests
    Inherits StampIndicator

    <Theory>
    <InlineData("F&M Manager", "icon-fm-manager")>
    <InlineData("Q/SHE Manager", "icon-qshe-manager")>
    <InlineData("Prod Sup", "icon-prod-sup")>
    <InlineData("Maint Sup", "icon-maint-sup")>
    Public Sub GetCssClassTest(StampTitle As String, ExpectedCssClass As String)
        Assert.Equal(ExpectedCssClass, GetCssClass(StampTitle))
    End Sub
End Class
