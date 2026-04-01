
Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        Build()
    End Sub

    Sub Build()
        Dim I As Integer = 0
        Dim II As Integer = 0
        Dim RC As Integer = 0
        Dim Department As String = ""
        Dim SB As New StringBuilder
        Dim TempNote As String
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim DR_Look As Data.DataRow
        Dim CountNotes As Integer


        DS = SatiCode.GetMyDataSet("SELECT TOP (100) PERCENT dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.Q_MR_Open.MR_Key, dbo.Q_MR_Open.[Ticket Type], dbo.Q_MR_Open.NoteType, dbo.Q_MR_Open.Note, dbo.Q_MR_Open.IssueDate FROM dbo.T_Tools LEFT OUTER JOIN dbo.Q_MR_Open ON dbo.T_Tools.[Key] = dbo.Q_MR_Open.Tool WHERE (dbo.T_Tools.UpTimeReport = 1) GROUP BY dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.Q_MR_Open.MR_Key, dbo.Q_MR_Open.[Ticket Type], dbo.Q_MR_Open.NoteType, dbo.Q_MR_Open.Note, dbo.Q_MR_Open.IssueDate  ORDER BY dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.Q_MR_Open.MR_Key")
        RC = DS.Tables(0).Rows.Count

        For I = 0 To RC - 1
            DR = DS.Tables(0).Rows(I)

            If Not DR("Department").ToString = Department Then
                SB.Append("<h2 style=""color: #0000FF""> " & DR("Department").ToString & "</h1> ")
                Department = DR("Department").ToString
            End If

            SB.Append("<input ")
            SB.Append("iD=""Button" & I & """ ")
            SB.Append("type=""button"" ")
            If IsDBNull(DR("IssueDate")) Then
                SB.Append("value=""" & DR("Tool").ToString & """ ")
            Else
                SB.Append("value=""" & DR("Tool").ToString & " (" & DateDiff(DateInterval.Hour, DR("IssueDate"), DateAndTime.Now) & "hr)" & """  ")
            End If
            'DateDiff(DateInterval.Hour, DR("IssueDate"), DateAndTime.Now)

            CountNotes = 1
            Do
                If DR("NoteType").ToString = "" Then
                    SB.Append("title=""Looks Good!"" ")
                    CountNotes = 0
                    Exit Do
                Else
                    If I = RC - 1 Then
                        Exit Do
                    End If
                    DR_Look = DS.Tables(0).Rows(I + CountNotes)
                        If DR("MR_Key").ToString = DR_Look("MR_Key").ToString Then
                            CountNotes += 1
                        Else
                            Exit Do
                        End If
                    End If
            Loop

            If Not CountNotes = 0 Then
                TempNote = ""
                For II = 0 To CountNotes - 1
                    DR_Look = DS.Tables(0).Rows(I + II)
                    Select Case DR_Look("NoteType").ToString
                        Case "Org"
                            TempNote = TempNote & "Org Note: " & DR_Look("Note").ToString & Chr(13) & Chr(13)
                        Case "Tech"
                            TempNote = TempNote & "Tech Note: " & DR_Look("Note").ToString & Chr(13) & Chr(13)
                    End Select
                Next
                TempNote = TempNote.Replace(Chr(34), Chr(39))
                SB.Append("title=""" & TempNote & """ ")
                If CountNotes > 1 Then
                    I = I + II - 1
                End If

            End If

            Select Case DR("Ticket Type").ToString
                Case = ""
                    SB.Append("style=""width: 250px; height: 50px; background-color: #33CC33;"" ") ' green 33CC33
                Case = "Standard"
                    SB.Append("style=""width: 250px; height: 50px; background-color: #FFFF66;"" ") 'Yellow FFFF66
                Case = "Down"
                    SB.Append("style=""width: 250px; height: 50px; background-color: Red;"" ") 'Red
            End Select


            SB.Append(" />")

        Next


        Panel1.Controls.Add(New LiteralControl(SB.ToString))



    End Sub


    Sub hold()
        '<asp:Button ID = "Button1" runat="server" Text="Auto Batch Stock (Polish)" Height="125px" Width="258px" ToolTip="Master drive fault: n4 outer pin ring, n2 lower plate, n3 inner pin ring. Audible grinding noise heard emanating from outer pin ring gearbox/motor assembly area. Grinding most audible in second half of brush cycle when spin direction changes. " BackColor="#33CC33" />
        '<asp:Button ID = "Button2" runat="server" Text="Auto Batch Stock / 3500 (DSP)" Height="50px" Width="300px" BackColor="#FFFF66" />
        '<asp:Button ID = "Button3" runat="server" Text="Button" Height="112px" Width="644px" BackColor="Red" />
        '<asp:Button ID="Button4" runat="server" Text="Button" OnClick="myclick" CommandArgument="themrnumber" />
    End Sub

End Class
