<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CustomerAddressEdit.aspx.vb" Inherits="CustomerMaintenance_CustomerAddressEdit" title="Untitled Page" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel3" runat="server" Width="968px">
                <br />
                Select Customer
                <asp:DropDownList ID="CustomerDropDownList" runat="server" OnSelectedIndexChanged="CustomerDropDownList_SelectedIndexChanged"
                    Width="224px" DataSourceID="CustomerSqlDataSource" DataTextField="Business_Name" DataValueField="Business_Name" AutoPostBack="True">
                </asp:DropDownList><br />
                Select Fab &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:DropDownList ID="FabDropDownList"
                    runat="server" Width="224px" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="FabsOnlySqlDataSource" DataTextField="CustomerID" OnSelectedIndexChanged="FabDropDownList_SelectedIndexChanged">
                </asp:DropDownList><br />
                Select ID &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:DropDownList ID="IDDropDownList" runat="server" Width="224px" OnSelectedIndexChanged="IDDropDownList_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="MainSqlDataSource" DataTextField="MainID">
                </asp:DropDownList><br />
                <br />
                <asp:Panel ID="AddressContentPanel" runat="server" BackColor="LightBlue" Visible="False"
                    Width="848px">
                    <br />
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Large" Text="Shipping Address"></asp:Label>
                    <asp:Label ID="ShipKeyLabel" runat="server" Text="Key"></asp:Label><br />
                    <asp:Panel ID="Panel1" runat="server" Width="824px">
                        <asp:Button ID="AddressShippingEditButton" runat="server" OnClick="AddressShippingEditButton_Click"
                            Text="New" /><br />
                        <asp:Panel ID="AdressAddShippingPanel" runat="server" Visible="False" Width="808px">
                            <asp:RadioButton ID="AddressShippingStreetRadioButton" runat="server" AutoPostBack="True"
                                Checked="True" GroupName="ASType" OnCheckedChanged="AddressShippingStreetRadioButton_CheckedChanged"
                                Text="Street Address" />&nbsp;<asp:RadioButton ID="AddressShippingPOBoxRadioButton"
                                    runat="server" AutoPostBack="True" GroupName="ASType" OnCheckedChanged="AddressShippingPOBoxRadioButton_CheckedChanged"
                                    Text="PO Box Address" />
                            <br />
                            <br />
                            <asp:Label ID="Label6" runat="server" Text="Attn: "></asp:Label>
                            <asp:TextBox ID="ASAddAttnTextBox" runat="server" BackColor="White"></asp:TextBox>
                            &nbsp; &nbsp;
                            <asp:Label ID="Label7" runat="server" Text="Building: "></asp:Label>
                            <asp:TextBox ID="ASAddBuildingTextBox" runat="server" BackColor="White"></asp:TextBox>&nbsp;&nbsp;<br />
                            <br />
                            <asp:Label ID="ShipPOLabel" runat="server" Text="PO Box"></asp:Label>
                            &nbsp;<asp:TextBox ID="ASAddPoBoxTextBox" runat="server" BackColor="White"></asp:TextBox>
                            &nbsp;<asp:Label ID="ASAddStreetNumberLabel" runat="server" Text="Street#: "></asp:Label>
                            <asp:TextBox ID="ASAddStreetNumberTextBox" runat="server" BackColor="White" Width="56px"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Label ID="ASAddDirectionLabel" runat="server" Text="Direction:"></asp:Label>
                            <asp:TextBox ID="ASAddDirectionTextBox" runat="server" BackColor="White" Width="40px"></asp:TextBox>&nbsp;
                            <asp:Label ID="ASAddStreetNameLabel" runat="server" Text="Street Name:"></asp:Label>
                            <asp:TextBox ID="ASAddStreetNameTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <asp:Label ID="ASAddStreetTypeLabel" runat="server" Text="Type:"></asp:Label>
                            <asp:TextBox ID="ASAddStreetTypeTextBox" runat="server" BackColor="White" Width="40px"></asp:TextBox><br />
                            <br />
                            <asp:Label ID="Label12" runat="server" Text="City:"></asp:Label>
                            <asp:TextBox ID="ASAddCityTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <asp:Label ID="Label13" runat="server" Text="State:"></asp:Label>
                            <asp:TextBox ID="ASAddStateTextBox" runat="server" BackColor="White" Width="40px"></asp:TextBox>
                            <asp:Label ID="Label14" runat="server" Text="Zip Code:"></asp:Label>
                            <asp:TextBox ID="ASAddZipCodeTextBox" runat="server" BackColor="White" Width="40px"></asp:TextBox>
                            <asp:Label ID="Label15" runat="server" Text="Country:"></asp:Label>
                            <asp:TextBox ID="ASAddCountryTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Label ID="Label16" runat="server" Text="Phone# "></asp:Label>
                            <asp:TextBox ID="ASAddPhoneTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <asp:Label ID="Label17" runat="server" Text="Fax#"></asp:Label>
                            <asp:TextBox ID="ASAddFaxTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <br />
                            <asp:Button ID="ASSaveButton" runat="server" OnClick="AddressShippingSaveButton_Click"
                                Text="Save" />
                            <br />
                        </asp:Panel>
                        <br />
                        <asp:Label ID="AddressShip1Label" runat="server" Text="Label"></asp:Label>
                        <br />
                        <asp:Label ID="AddressShip2Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressShip3Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressShip4Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressShip5Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressShip6Label" runat="server" Text="Label"></asp:Label><br />
                        <br />
                        Linked ID's<br />
                        <asp:ListBox ID="ListBox1" runat="server" Width="104px"></asp:ListBox></asp:Panel>
                    <br />
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Large" Text="Billing Address"></asp:Label>
                    <asp:Label ID="BillKeyLabel" runat="server" Text="Key"></asp:Label><br />
                    <asp:Panel ID="Panel2" runat="server" Width="648px">
                        <asp:Button ID="AddressBillingEditButton" runat="server" OnClick="AddressBillingEditButton_Click"
                            Text="New" /><br />
                        <asp:Panel ID="AdressAddBillingPanel" runat="server" Visible="False" Width="816px">
                            <asp:RadioButton ID="AddressBillingStreetRadioButton" runat="server" GroupName="ASType"
                                Text="Street Address" OnCheckedChanged="AddressBillingStreetRadioButton_CheckedChanged" AutoPostBack="True" />&nbsp;
                                <asp:RadioButton ID="AddressBillingPOBoxRadioButton"
                                    runat="server" GroupName="ASType" Text="PO Box Address" OnCheckedChanged="AddressBillingPOBoxRadioButton_CheckedChanged" AutoPostBack="True" />
                            <br />
                            <br />
                            <asp:Label ID="Label8" runat="server" Text="Attn: "></asp:Label>
                            <asp:TextBox ID="ABAddAttnTextBox" runat="server" BackColor="White"></asp:TextBox>
                            &nbsp; &nbsp;
                            <asp:Label ID="Label9" runat="server" Text="Building: "></asp:Label>
                            <asp:TextBox ID="ABAddBuildingTextBox" runat="server" BackColor="White"></asp:TextBox>&nbsp;
                            <br />
                            <br />
                            &nbsp;<asp:Label ID="ABAddPOBoxLabel" runat="server" Text="PO Box"></asp:Label>
                            <asp:TextBox ID="ABAddPoBoxTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <asp:Label
                                ID="ABAddStreetNumberLabel" runat="server" Text="Street#: "></asp:Label>
                            <asp:TextBox ID="ABAddStreetNumberTextBox" runat="server" BackColor="White" Width="32px"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Label ID="ABAddDirectionLabel" runat="server" Text="Direction:"></asp:Label>
                            <asp:TextBox ID="ABAddDirectionTextBox" runat="server" BackColor="White" Width="40px"></asp:TextBox>&nbsp;
                            <asp:Label ID="ABAddStreetNameLabel" runat="server" Text="Street Name:"></asp:Label>
                            <asp:TextBox ID="ABAddStreetNameTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <asp:Label ID="ABAddStreetTypeLabel" runat="server" Text="Type:"></asp:Label>
                            <asp:TextBox ID="ABAddStreetTypeTextBox" runat="server" BackColor="White" Width="40px"></asp:TextBox><br />
                            <br />
                            <asp:Label ID="Label10" runat="server" Text="City:"></asp:Label>
                            <asp:TextBox ID="ABAddCityTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <asp:Label ID="Label11" runat="server" Text="State:"></asp:Label>
                            <asp:TextBox ID="ABAddStateTextBox" runat="server" BackColor="White" Width="40px"></asp:TextBox>
                            <asp:Label ID="Label18" runat="server" Text="Zip Code:"></asp:Label>
                            <asp:TextBox ID="ABAddZipCodeTextBox" runat="server" BackColor="White" Width="40px"></asp:TextBox>
                            <asp:Label ID="Label19" runat="server" Text="Country:"></asp:Label>
                            <asp:TextBox ID="ABAddCountryTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Label ID="Label20" runat="server" Text="Phone# "></asp:Label>
                            <asp:TextBox ID="ABAddPhoneTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <asp:Label ID="Label21" runat="server" Text="Fax#"></asp:Label>
                            <asp:TextBox ID="ABAddFaxTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <br />
                            <asp:Button ID="ABSaveButton" runat="server" OnClick="ABSaveButton_Click" Text="Save" />
                            <br />
                        </asp:Panel>
                        <br />
                        <asp:Label ID="AddressBill1Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill2Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill3Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill4Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill5Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill6Label" runat="server" Text="Label"></asp:Label><br />
                        <br />
                        Linked ID's<br />
                        <asp:ListBox ID="ListBox2" runat="server" Width="104px"></asp:ListBox></asp:Panel>
                    <asp:SqlDataSource ID="CustomerSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT Business_Name, Customer_Name FROM dbo.Customer GROUP BY Business_Name, Customer_Name ORDER BY Business_Name">
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="MainSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT MainID FROM dbo.MainID WHERE (CustomerID = N'')"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="FabsOnlySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT CustomerID FROM dbo.Customer WHERE (Business_Name = N'') ORDER BY CustomerID">
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="AddressGetSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT MainID, Type, Row1, Row2, Row3, Row4, Row5, Row6, AddressKey FROM dbo.fctn_q_Customer_Address() AS fctn_q_Customer_Address_1 WHERE (MainID = N'2386')">
                    </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    &nbsp; &nbsp;
    <br />
</asp:Content>

