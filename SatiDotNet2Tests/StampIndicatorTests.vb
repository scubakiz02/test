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
    Private IconsConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Dim DT As New DataTable()
    Dim DR As Data.DataRow

    Sub New()
        CurrDS = GetMyDataSetParamQuery("SELECT Title, Base64Icon, IconImgFilePath FROM [ALTS].[dbo].[T_LogStampTitle]", New Dictionary(Of String, Dictionary(Of String, String)))
        For Each CurrDR As DataRow In CurrDS.Tables(0).Rows
            Dim IconConfig As New Dictionary(Of String, String)

            TitleToIcon(CurrDR("Title")) = CurrDR("Base64Icon")
            IconsConfig(CurrDR("Title")) = IconConfig

            IconConfig("Base64Icon") = CurrDR("Base64Icon")
            IconConfig("IconImgFilePath") = CurrDR("IconImgFilePath")
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

    Private Function SerializeHash(Hash As Dictionary(Of String, Dictionary(Of String, String))) As String
        Return JsonSerializer.Serialize(Of Dictionary(Of String, Dictionary(Of String, String)))(Hash)
    End Function

    <Fact>
    Public Sub IconsTest1()
        '2 stamps required, 2 stamps received (F&M Manager, Q/SHE Manager)
        'Expect a list holding information of needed stamps, which is null
        Assert.Equal(SerializeHash(New Dictionary(Of String, Dictionary(Of String, String))), SerializeHash(StampIndicator.Icons(263)))
    End Sub

    <Fact>
    Public Sub IconsTest2()
        '2 stamps required, 1 stamps received
        'Expect a list holding information of needed stamps, which is F&M Manager
        Dim IconsConfigRefactored As New Dictionary(Of String, Dictionary(Of String, String))(IconsConfig)
        IconsConfigRefactored.Remove("Q/SHE Manager")
        IconsConfigRefactored.Remove("Prod Sup")
        IconsConfigRefactored.Remove("Maint Sup")
        Assert.Equal(SerializeHash(IconsConfigRefactored), SerializeHash(StampIndicator.Icons(526)))
    End Sub

    Private Sub DummyClickEvent(Key As Integer)

    End Sub
End Class
