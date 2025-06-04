Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json
Imports System.Collections
Imports System.Collections.Generic

Public Class PartClassAddPartFunctionTests
    Inherits Part
    Dim SqlParameters As New SqlParameters()
    Private TestDataShell As New Dictionary(Of String, String) From {
        {"PartDescription", "Gigamat 5000 pin ring"},
        {"Qty", "1"}
    }
    Private ExpectedSqlQuery As String = "INSERT INTO [ALTS].[dbo].[T_SMR_PartToOrder] (SMR_Key, ManufacturerOrVendor, PartDescription, PW_PartNum, Vendor_PartNum, Qty, Procured) VALUES (@SMR_Key, @ManufacturerOrVendor, @PartDescription, @PW_PartNum, @Vendor_PartNum, @Qty, @Procured)"

    Private Function SuccessAndMessageExist(Hash As Dictionary(Of String, String)) As Boolean
        Dim Res As Boolean

        If Hash.Count <> 2 Then
            Res = False
        ElseIf Hash.ContainsKey("Success") = False Then
            Res = False
        ElseIf Hash.ContainsKey("Message") = False Then
            Res = False
        Else
            Res = True
        End If

        Return Res
    End Function

    <Fact>
    Public Sub NothingAsArg()
        Dim Res As Dictionary(Of String, String) = AddPart(Nothing)

        Assert.True(SuccessAndMessageExist(Res))
        Assert.False(Boolean.Parse(Res("Success")))
        Assert.Equal("Error: Name & Quantity -- required", Res("Message"))
        'Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), AddPart(Nothing))
    End Sub

    <Fact>
    Public Sub BlankHashAsArg()
        Dim Res As Dictionary(Of String, String) = AddPart(New Dictionary(Of String, String))

        Assert.True(SuccessAndMessageExist(Res))
        Assert.False(Boolean.Parse(Res("Success")))
        Assert.Equal("Error: Name & Quantity -- required", Res("Message"))

        'Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), AddPart(New Dictionary(Of String, String)))
    End Sub

    <Theory>
    <InlineData(3, "Part A")>
    <InlineData(1, "Part B")>
    <InlineData(5, "Part C")>
    <InlineData(10, "Part D")>
    <InlineData(12, "Part E")>
    Public Sub ArgWithRequiredValues(Qty As Integer, PartName As String)
        'required values are:
        '1) SMR_Key
        '2) Procured
        '3) Qty
        '4) Part Description
        '****expecting Procured and SMR_Key from client side to ALWAYS NOT be NULL, so am NOT testing for it
        Dim CreateHash As New Dictionary(Of String, String)(TestDataShell)
        Dim CreateFuncRes As Dictionary(Of String, String)

        CreateHash("Qty") = Qty
        CreateHash("PartName") = PartName

        CreateFuncRes = AddPart(CreateHash)

        Assert.True(SqlParameters.ValidParameterizedValues(CreateHash, CreateFuncRes))
        Assert.Equal(ExpectedSqlQuery, CreateFuncRes("SqlQuery"))
    End Sub

    <Fact>
    Public Sub NoPartDescription()
        'T_SMR_Parts PartDesciption field does NOT allow nulls
        'ensure Part.AddPart() function accounts for this
        Dim CreateHash As New Dictionary(Of String, String)(TestDataShell)
        Dim CreateFuncRes As Dictionary(Of String, String)

        CreateHash("PartDescription") = String.Empty

        CreateFuncRes = AddPart(CreateHash)

        Assert.True(SuccessAndMessageExist(CreateFuncRes))
        Assert.False(Boolean.Parse(CreateFuncRes("Success")))
        Assert.Equal("Error: Name & Quantity -- required", CreateFuncRes("Message"))
    End Sub

    <Fact>
    Public Sub NoQty()
        Dim CreateHash As New Dictionary(Of String, String)(TestDataShell)
        Dim CreateFuncRes As New Dictionary(Of String, String)

        CreateHash("Qty") = String.Empty
        CreateFuncRes = AddPart(CreateHash)

        Assert.True(SuccessAndMessageExist(CreateFuncRes))
        Assert.False(Boolean.Parse(CreateFuncRes("Success")))
        Assert.Equal("Error: Name & Quantity -- required", CreateFuncRes("Message"))
    End Sub

    <Theory>
    <InlineData("1e")>
    <InlineData("e")>
    <InlineData(Nothing)>
    <InlineData("")>
    Public Sub InvalidQtyValues(Qty As String)
        Dim CreateHash As New Dictionary(Of String, String)(TestDataShell)
        Dim CreateFuncRes As Dictionary(Of String, String)

        CreateHash("Qty") = Qty
        CreateFuncRes = AddPart(CreateHash)

        Assert.True(SuccessAndMessageExist(CreateFuncRes))
        Assert.False(Boolean.Parse(CreateFuncRes("Success")))
        Assert.Equal("Error: Name & Quantity -- required", CreateFuncRes("Message"))
    End Sub

End Class