<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MD_Main.aspx.vb" Inherits="MaintenanceDepartment_MD_Main" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        :root {
            --UWhitespace: 0.5em;
            --UFontSize: (calc(var(--UWhitespace) * 2))
        }

        /* ============ maintenance-hyperlink-section ============= */
        #maintenance-hyperlink-section {
            display: flex;
            flex-direction: column;
            gap: var(--UWhitespace);
        }

        .md-main-list {
            margin: 0;
            list-style: circle;
        }

            .md-main-list li {
            }
    </style>
    <section id="maintenance-hyperlink-section">
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Maintenance Department"></asp:Label>

        <div id="mr-container">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/MR/MR_Viewer.aspx">Maintenance Requests</asp:HyperLink>
        </div>

        <div id="checklist-and-pm-container">
            <asp:Label Text="Checklists & PMs" runat="server" />

            <ul id="checklist-and-pm-list" class="md-main-list">
                <li>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ChecklistLogging/ChecklistBuilder.aspx">Build PM</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/ChecklistLogging/ChecklistReport.aspx">Report PM</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/ChecklistLogging/StatusBoard.aspx?Department=Maintenance&View=Focus">PM Status Board, Focus View</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/ChecklistLogging/StatusBoard.aspx?Department=Maintenance&View=Full">PM Status Board, Full View</asp:HyperLink>
                </li>
            </ul>

        </div>

        <div id="smr-container">
            <asp:Label Text="Scheduled Maintenance Requests (SMRs):" runat="server" />

            <ul id="smr-list" class="md-main-list">
                <li>
                    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/SMR/ScheduledMaintReqTicket.aspx">Create SMR</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/SMR/SMR_Viewer.aspx">View SMR</asp:HyperLink>
                </li>
            </ul>

        </div>

    </section>


</asp:Content>
