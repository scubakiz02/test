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
    Dim DT As New DataTable()
    Dim DR As Data.DataRow

    Sub New()
        CurrDS = GetMyDataSetParamQuery("SELECT Title, Base64Icon FROM [ALTS].[dbo].[T_LogStampTitle]", New Dictionary(Of String, Dictionary(Of String, String)))
        For Each CurrDR As DataRow In CurrDS.Tables(0).Rows
            TitleToIcon(CurrDR("Title")) = CurrDR("Base64Icon")
        Next

        DT.Columns.Add("Base64Icon", GetType(String))
    End Sub

    Private Sub AddToDataTable(Base64 As String)
        DR = DT.NewRow()
        DR("Base64Icon") = Base64
    End Sub

    Private Function SerializeList(List As List(Of String)) As String
        Return JsonSerializer.Serialize(Of List(Of String))(List)
    End Function

    Private Function SerializeHash(Hash As Dictionary(Of String, String)) As String
        Return JsonSerializer.Serialize(Of Dictionary(Of String, String))(Hash)
    End Function

    <Fact>
    Public Sub IconsTest1()
        '2 stamps required, 2 stamps received (F&M Manager, Q/SHE Manager)
        'Expect a list holding information of needed stamps, which is null
        Assert.Equal(SerializeList(New List(Of String)), SerializeList(StampIndicator.Icons(263)))
    End Sub

    <Fact>
    Public Sub IconsTest2()
        '2 stamps required, 1 stamps received
        'Expect a list holding information of needed stamps, which is F&M Manager
        Dim Res As New List(Of String) From {TitleToIcon("F&M Manager")}
        Assert.Equal(SerializeList(Res), SerializeList(StampIndicator.Icons(526)))
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
        '2 stamps required, 1 stamps received
        'Last arg is true, meaning user is IN full view.
        'As a result, expect 1 child controls in the Panel returned from CreateIcons() function
        Dim Panel As New Panel()
        Panel.ID = "blah_23"
        Dim ResPanel As Panel = StampIndicator.CreateIcons(Panel, 526, AddressOf DummyClickEvent, True)
        Assert.Equal(1, ResPanel.Controls.Count)
    End Sub

    <Fact>
    Public Sub CreateIconsTest2()
        '2 stamps required, 2 stamps received (F&M Manager, Q/SHE Manager)
        'Last arg is false, meaning user is not in full view.
        'As a result, expect 0 child controls in the Panel returned from CreateIcons() function
        Dim Panel As New Panel()
        Panel.ID = "blah_23"
        Dim ResPanel As Panel = StampIndicator.CreateIcons(Panel, 263, AddressOf DummyClickEvent, True)
        Assert.Equal(0, ResPanel.Controls.Count)
    End Sub

    <Fact>
    Public Sub CreateIconsTest3()
        '2 stamps required, 1 stamps received
        'Last arg is true, meaning user is not IN full view.
        'As a result, expect 0 child controls in the Panel returned from CreateIcons() function
        Dim Panel As New Panel()
        Panel.ID = "blah_23"
        Dim ResPanel As Panel = StampIndicator.CreateIcons(Panel, 526, AddressOf DummyClickEvent, False)
        Assert.Equal(0, ResPanel.Controls.Count)
    End Sub
End Class
