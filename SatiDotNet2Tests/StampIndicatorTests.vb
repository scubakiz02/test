Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json
Imports System.Web.UI.WebControls

Public Class StampIndicatorTests
    Inherits Security

    Private StampIndicator As New StampIndicator()
    Private CurrDS As New Data.DataSet
    Private TitleToIcon As New Dictionary(Of String, String)

    Private Sub OgEnvironment()

    End Sub

    Sub New()
        CurrDS = GetMyDataSetParamQuery("SELECT Title, Base64Icon FROM [ALTS].[dbo].[T_LogStampTitle]", New Dictionary(Of String, Dictionary(Of String, String)))
        For Each CurrDR As DataRow In CurrDS.Tables(0).Rows
            TitleToIcon(CurrDR("Title")) = CurrDR("Base64Icon")
        Next
    End Sub

    Public Function SerializeList(List As List(Of String)) As String
        Return JsonSerializer.Serialize(Of List(Of String))(List)
    End Function

    Public Function SerializeHash(Hash As Dictionary(Of String, String)) As String
        Return JsonSerializer.Serialize(Of Dictionary(Of String, String))(Hash)
    End Function

    <Fact>
    Public Sub IconsTest1()
        '2 stamps required, 2 stamps received (F&M Manager, Q/SHE Manager). Expect a list holding this information
        Dim Res As New List(Of String) From {TitleToIcon("F&M Manager"), TitleToIcon("Q/SHE Manager")}
        Assert.Equal(SerializeList(Res), SerializeList(StampIndicator.Icons(263)))
    End Sub

    <Fact>
    Public Sub IconsTest2()
        '1 stamp required, 1 stamp received (F&M Manager). Expect a list holding this information
        Dim Res As New List(Of String) From {TitleToIcon("F&M Manager")}
        Assert.Equal(SerializeList(Res), SerializeList(StampIndicator.Icons(411)))
    End Sub

    <Fact>
    Public Sub GetTitleIconHashTest()
        'ensure StampIndicator.GetTitleIconHash() is a dictionary with T_LogStampTitle Title field values as keys, and Base64 field values as the values
        Assert.Equal(SerializeHash(TitleToIcon), SerializeHash(StampIndicator.GetTitleIconHash()))
    End Sub

    Private Sub DummyClickEvent(Key As Integer)

    End Sub

    <Fact>
    Public Sub CreateIconsTest1()
        '1 stamp required, 1 stamp received for T_LogData record 411.
        'Last arg is true, meaning user is IN full view.
        'As a result, expect 1 child control in the Panel returned from CreateIcons() function
        Dim Panel As New Panel()
        Panel.ID = "blah_23"
        Dim ResPanel As Panel = StampIndicator.CreateIcons(Panel, 411, AddressOf DummyClickEvent, True)
        Assert.Equal(1, ResPanel.Controls.Count)
    End Sub

    <Fact>
    Public Sub CreateIconsTest2()
        '1 stamp required, 1 stamp received for T_LogData record 411.
        'Last arg is false, meaning user is NOT in full view.
        'As a result, expect 1 child control in the Panel returned from CreateIcons() function
        Dim Panel As New Panel()
        Panel.ID = "blah_23"
        Dim ResPanel As Panel = StampIndicator.CreateIcons(Panel, 411, AddressOf DummyClickEvent, False)
        Assert.Equal(0, ResPanel.Controls.Count)
    End Sub
End Class
