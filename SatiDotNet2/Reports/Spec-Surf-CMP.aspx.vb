
Partial Class Reports_Spec_Surf_CMP
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Sub Cal()

        Dim Rows As Integer = Me.GridView1.Rows.Count
        If Rows = 0 Then
            Exit Sub
        End If

        Dim wafers As Integer = 0
        Dim Bin1 As Integer = 0
        Dim Bin2 As Integer = 0
        Dim Bin3 As Integer = 0
        Dim Percentpassed As Double = 0.0


        Dim i As Integer = 0
        'total wafers = 5
        'Bin1 = 6    % = 7
        'Bin2 = 8    % = 9
        'Bin3 = 10   % =11
        'total% = 13
        For i = 0 To Rows - 1
            wafers = CType(Me.GridView1.Rows(i).Cells(5).Text, Integer)
            Bin1 = CType(Me.GridView1.Rows(i).Cells(6).Text, Integer)
            Bin2 = CType(Me.GridView1.Rows(i).Cells(8).Text, Integer)
            Bin3 = CType(Me.GridView1.Rows(i).Cells(10).Text, Integer)
            If Not wafers = 0 Then
                'Bin1 %
                Me.GridView1.Rows(i).Cells(7).Text = Format(Bin1 / wafers, "0.0%")
                'Bin2 %
                Me.GridView1.Rows(i).Cells(9).Text = Format(Bin2 / wafers, "0.0%")
                'Bin3 %
                Me.GridView1.Rows(i).Cells(11).Text = Format(Bin3 / wafers, "0.0%")
                'Total%
                Me.GridView1.Rows(i).Cells(13).Text = Format((Bin2 + Bin3) / wafers, "0.0%")
            End If

        Next




    End Sub

    Private Sub GridView1_Load(sender As Object, e As EventArgs) Handles GridView1.Load
        Cal()
    End Sub
End Class
