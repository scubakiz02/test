
Partial Class PC_CofA_MetalsPool
    Inherits System.Web.UI.Page
    Sub Highlight()
        Dim MyVal As Double
        For i As Integer = 0 To Me.GridView1.Rows.Count - 1
            For ii As Int16 = 5 To 14
                MyVal = GridView1.Rows(i).Cells(ii).Text
                If MyVal > 0.5 Then
                    GridView1.Rows(i).Cells(ii).BackColor = colored(MyVal) ' Drawing.Color.Red 
                End If
            Next
        Next
    End Sub

    Function colored(Myval As Double) As Drawing.Color
        'Hex={66,CC,FF} light blue
        Select Case Myval
            Case > 3
                colored = Drawing.ColorTranslator.FromHtml("#FF99CC")
            Case > 1
                colored = Drawing.ColorTranslator.FromHtml("#FFE699")
            Case > 0.5
                colored = Drawing.ColorTranslator.FromHtml("#CCFFFF")

        End Select

    End Function

    Private Sub GridView1_Load(sender As Object, e As EventArgs) Handles GridView1.Load
        Highlight()
    End Sub
End Class
