<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Test300mmMetals.aspx.vb" Inherits="TestArea_Test300mmMetals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server">
                Test 300mm Metals <br />
                Instance Number:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> &nbsp;<br /><br />
                L&nbsp;<asp:TextBox ID="TextBoxL" runat="server" Text="1" Width="25px"></asp:TextBox>&nbsp;
                U&nbsp;<asp:TextBox ID="TextBoxU" runat="server" Text="9" Width="25px"></asp:TextBox>&nbsp;
                <asp:Button ID="Button1" runat="server" Text="Get Random" />&nbsp;
                <asp:TextBox ID="TextBoxRandom" runat="server"></asp:TextBox>
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                    <Columns>
                        <asp:BoundField DataField="InstanceKey" HeaderText="InstanceKey" SortExpression="InstanceKey" />
                        <asp:BoundField DataField="Date/Time" HeaderText="Date/Time" SortExpression="Date/Time" />
                        <asp:BoundField DataField="Source" HeaderText="Source" SortExpression="Source" />
                        <asp:BoundField DataField="Test Type" HeaderText="Test Type" SortExpression="Test Type" />
                        <asp:BoundField DataField="Idenyification" HeaderText="Idenyification" SortExpression="Idenyification" />
                        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                        <asp:BoundField DataField="Ca" HeaderText="Ca" SortExpression="Ca" />
                        <asp:BoundField DataField="Ma" HeaderText="Ma" SortExpression="Ma" />
                        <asp:BoundField DataField="Ni" HeaderText="Ni" SortExpression="Ni" />
                        <asp:BoundField DataField="Zn" HeaderText="Zn" SortExpression="Zn" />
                        <asp:BoundField DataField="Al" HeaderText="Al" SortExpression="Al" />
                        <asp:BoundField DataField="Fe" HeaderText="Fe" SortExpression="Fe" />
                        <asp:BoundField DataField="Cr" HeaderText="Cr" SortExpression="Cr" />
                        <asp:BoundField DataField="Cu" HeaderText="Cu" SortExpression="Cu" />
                        <asp:BoundField DataField="Na" HeaderText="Na" SortExpression="Na" />
                        <asp:BoundField DataField="K" HeaderText="K" SortExpression="K" />
                        <asp:BoundField DataField="Co" HeaderText="Co" SortExpression="Co" />
                        <asp:BoundField DataField="Mn" HeaderText="Mn" SortExpression="Mn" />
                        <asp:BoundField DataField="Mo" HeaderText="Mo" SortExpression="Mo" />
                        <asp:BoundField DataField="W" HeaderText="W" SortExpression="W" />
                        <asp:BoundField DataField="Ti" HeaderText="Ti" SortExpression="Ti" />
                    </Columns>
                </asp:GridView>

                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT T_FGI_Boxes.InstanceKey, [GFAAS Data].[Date/Time], [GFAAS Data].Source, [GFAAS Data].[Test Type], [GFAAS Data].Idenyification, [GFAAS Data].Location, [GFAAS Data].Ca, [GFAAS Data].Ma, [GFAAS Data].Ni, [GFAAS Data].Zn, [GFAAS Data].Al, [GFAAS Data].Fe, [GFAAS Data].Cr, [GFAAS Data].Cu, [GFAAS Data].Na, [GFAAS Data].K, [GFAAS Data].Co, [GFAAS Data].Mn, [GFAAS Data].Mo, [GFAAS Data].W, [GFAAS Data].Ti FROM LabelsMade INNER JOIN T_FGI_Boxes ON LabelsMade.LabelRecordNumber = T_FGI_Boxes.LabelsMadeKey INNER JOIN [GFAAS Data] ON LabelsMade.Lot = [GFAAS Data].Idenyification WHERE (T_FGI_Boxes.InstanceKey = 1)"></asp:SqlDataSource>

            </asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>
    
     
</asp:Content>

