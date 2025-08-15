
Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
    End Sub

    Sub hold()
        '<asp:Button ID = "Button1" runat="server" Text="Auto Batch Stock (Polish)" Height="125px" Width="258px" ToolTip="Master drive fault: n4 outer pin ring, n2 lower plate, n3 inner pin ring. Audible grinding noise heard emanating from outer pin ring gearbox/motor assembly area. Grinding most audible in second half of brush cycle when spin direction changes. " BackColor="#33CC33" />
        '<asp:Button ID = "Button2" runat="server" Text="Auto Batch Stock / 3500 (DSP)" Height="50px" Width="300px" BackColor="#FFFF66" />
        '<asp:Button ID = "Button3" runat="server" Text="Button" Height="112px" Width="644px" BackColor="Red" />
        '<asp:Button ID="Button4" runat="server" Text="Button" OnClick="myclick" CommandArgument="themrnumber" />
    End Sub

End Class
