
Partial Class TestArea_MakeSwanseaCofA
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim Path As String = "\\PWI-40\software$\LabelTemplates\LabelArchive\CofA Files\Sati CofA Files 2015\CofA-3265_6359-048.xls"
        Flex.Open(Path)

        SatiCode.MakeSwanseaCofA(Flex)


    End Sub
End Class
