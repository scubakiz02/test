Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

Public Class SampleTest
    Dim SampleInstantiantion = New SampleClass()

    <Fact>
    Public Sub AddNumbersTest1()
        Assert.Equal(5, SampleInstantiantion.AddNumbers(2, 3))
    End Sub

    <Fact>
    Public Sub ReturnTrueTest1()
        Assert.True(SampleInstantiantion.ReturnTrue())
    End Sub

End Class