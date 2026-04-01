Imports ReworkINVTableAdapters
Partial Class PC_ReworkInvAdj
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("PC", Server)
    End Sub

    Sub ScanSheet(ByVal Area As String)
        Dim SatiUser As String = User.Identity.Name

        Dim I As Integer = 0
        Dim NewQty As String
        Dim OldQty As String
        Dim ModQty As String
        Dim MainID As String
        Dim InvType As String = ""
        'Dim GridNum As String = ""
        Select Case Area
            Case "SE"
                InvType = "-6"

            Case "Lap"
                InvType = "-4"

            Case "Polish"
                InvType = "-5"

        End Select

        Dim RecordCount As Integer = Me.GridView1.Rows.Count
        For I = 0 To RecordCount - 1
            MainID = Me.GridView1.Rows(I).Cells(0).Text()
            NewQty = CType(Me.GridView1.Rows(I).Cells(2).FindControl("Textbox1"), TextBox).Text
            OldQty = Me.GridView1.Rows(I).Cells(1).Text()
            If Not NewQty = "" Then
                If NewQty <> OldQty Then
                    If NewQty >= 0 Then
                        ModQty = NewQty - OldQty
                        ModInv(MainID, ModQty, InvType, SatiUser, "INV ADJ")
                        Me.InfoTextBox.Text = ModQty
                        CType(Me.GridView1.Rows(I).Cells(2).FindControl("Textbox1"), TextBox).Text = ""
                    Else
                        CType(Me.GridView1.Rows(I).Cells(2).FindControl("Textbox1"), TextBox).BackColor = Drawing.Color.Red
                        Me.InfoTextBox.Text = "Records where not fixed in Red"
                    End If
                End If
            End If
        Next
        bindTo()
    End Sub

    Sub ModInv(ByVal MainID As String, ByVal Qty As String, ByVal InvType As String, ByVal SatiUser As String, ByVal Lot As String)
        Dim ReworkTable As New T_Rework_InvintoryTableAdapter
        ReworkTable.InsertReworkInv(InvType, MainID, Qty, "INV ADJ", Date.Now, SatiUser, Lot, "INV ADJ")
    End Sub
    Sub bindTo()
        Dim type As String = ""

        If Me.StripRadioButton.Checked = True Then
            type = "SE"
        End If
        If Me.LapRadioButton.Checked = True Then
            type = "Lap"
        End If
        If Me.PolishRadioButton.Checked = True Then
            type = "Polish"
        End If


        Select Case Type
            Case "SE"
                Me.RWSqlDataSource.SelectCommand = "SELECT dbo.MainID.MainID, dbo.Q_INV_Rework_SE.Qty FROM dbo.MainID LEFT OUTER JOIN dbo.Q_INV_Rework_SE ON dbo.MainID.MainID = dbo.Q_INV_Rework_SE.ID GROUP BY dbo.MainID.MainID, dbo.Q_INV_Rework_SE.Qty"
                Me.GridView1.DataBind()

            Case "Lap"
                Me.RWSqlDataSource.SelectCommand = "SELECT dbo.MainID.MainID, dbo.Q_Inv_Rework_Lap.Qty FROM dbo.MainID LEFT OUTER JOIN dbo.Q_Inv_Rework_Lap ON dbo.MainID.MainID = dbo.Q_Inv_Rework_Lap.ID GROUP BY dbo.MainID.MainID, dbo.Q_Inv_Rework_Lap.Qty"
                Me.GridView1.DataBind()

            Case "Polish"
                Me.RWSqlDataSource.SelectCommand = "SELECT dbo.MainID.MainID, dbo.Q_Inv_Rework_Polish.Qty FROM dbo.MainID LEFT OUTER JOIN dbo.Q_Inv_Rework_Polish ON dbo.MainID.MainID = dbo.Q_Inv_Rework_Polish.ID GROUP BY dbo.MainID.MainID, dbo.Q_Inv_Rework_Polish.Qty"
                Me.GridView1.DataBind()

        End Select
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Redirect("ReworkInvAdj.aspx")
    End Sub

    Protected Sub SEAdjButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SEAdjButton.Click
        If Me.StripRadioButton.Checked = True Then
            ScanSheet("SE")
        End If
        If Me.LapRadioButton.Checked = True Then
            ScanSheet("Lap")
        End If
        If Me.PolishRadioButton.Checked = True Then
            ScanSheet("Polish")
        End If
    End Sub

    Protected Sub StripRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.StripRadioButton.Checked = True Then
            bindTo()
        End If
    End Sub

    Protected Sub LapRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.LapRadioButton.Checked = True Then
            bindTo()
        End If
    End Sub

    Protected Sub PolishRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.PolishRadioButton.Checked = True Then
            bindTo()
        End If
    End Sub
End Class
