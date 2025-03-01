<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="LotMove.aspx.vb" Inherits="Production_LotMove" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>         
            <asp:Panel ID="Panel1" runat="server">
            <br />
            
            
            
            
            
            
            <br />   
            </asp:Panel>  

            <asp:UpdateProgress id="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <IMG src="../Color/Animated_LoadingBigger.gif" />Working...
                </ProgressTemplate>
            </asp:UpdateProgress>
            
        </contenttemplate>
    </asp:UpdatePanel>   
</asp:Content>