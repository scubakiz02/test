
Partial Class PC_SRN
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Private Sub PC_SRN_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim SRN As String
        If Page.IsPostBack = False Then
            If Not Request.QueryString("SRN") = "" Then
                SRN = Request.QueryString("SRN")
                Me.TextBoxSRN.Text = SRN
                LoadMe(SRN)
            End If
        End If

    End Sub
    Protected Sub ButtonLoadSRN_Click(sender As Object, e As EventArgs) Handles ButtonLoadSRN.Click
        LoadMe(Me.TextBoxSRN.Text)
    End Sub

    Sub LoadMe(SRN As String)
        'SELECT [Key], SRN, CartonNumber FROM dbo.T_Sati_SRN_Items WHERE (SRN = 0)
        Dim DS As New Data.DataSet
        DS = SatiCode.GetMyDataSet("SELECT [Key], SRN, CartonNumber FROM dbo.T_Sati_SRN_Items WHERE (SRN = " & Me.TextBoxSRN.Text & ")")




    End Sub




End Class
