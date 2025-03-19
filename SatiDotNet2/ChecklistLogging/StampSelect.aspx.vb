
Imports System.Text.Json
Imports SatiDotNet2.Library

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim Security As New Security
    Dim AreaFromQueryString As String
    Dim DS As New Data.DataSet
    Dim DR As Data.DataRow
    Dim RC As Integer

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        AreaFromQueryString = Request.QueryString("Area")
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }

        If AreaFromQueryString IsNot Nothing Then
            If Not IsPostBack Then
                AreaLabel.Text = Security.GetSingleDbField("SELECT Area FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=@AreaKey", QueryObject, "Area")
                DS = Security.GetMyDataSetParamQuery("SELECT Stamp.Title As Text, Stamp.[Key] As Value, Stamped.Active AS Selected FROM [ALTS].[dbo].[T_LogStampTitle] Stamp INNER JOIN [ALTS].[dbo].[T_LogStampList] Stamped ON Stamp.[Key]=Stamped.TitleKey AND Stamped.AreaKey=@AreaKey", QueryObject)
                RC = DS.Tables(0).Rows.Count

                For I = 0 To RC - 1
                    Dim listItem As New ListItem()
                    DR = DS.Tables(0).Rows(I)

                    listItem.Text = DR("Text")
                    listItem.Value = DR("Value") 'associated Key within [T_LogStampTitle]
                    listItem.Selected = DR("Selected")

                    StampCheckBoxList.Items.Add(listItem)
                Next
            End If
        End If
    End Sub

    Function GetSingleDbField(SqlQuery As String, Field As String) As String
        Dim Res As String

        'using try catch block in case 'There is no row at position 0.', which means there are no associated record in Table
        Try
            Res = If(IsDBNull(SatiCode.GetMyDataSet(SqlQuery).Tables(0).Rows(0)(Field)), Nothing, SatiCode.GetMyDataSet(SqlQuery).Tables(0).Rows(0)(Field)) 'using ternary operator as a workaround to Null DB field values, which in that case the function will return Nothing
        Catch ex As Exception
            Res = Nothing
        End Try

        Return Res
    End Function

    Sub ExecuteSqlQuery(SqlQuery As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim MySQLCommand As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = Session("DBConnect")
        Connection.Open()
        With MySQLCommand
            .CommandText = SqlQuery
            .Connection = Connection
        End With
        MySQLCommand.ExecuteNonQuery()
        Connection.Close()
    End Sub


    Protected Sub ExitIframeButton_onClick(sender As Object, e As EventArgs)
        If sender.Text = "Update" Then
            For Each ListItem As ListItem In StampCheckBoxList.Items
                ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogStampList] SET Active=" & If(ListItem.Selected, 1, 0) & " WHERE TitleKey=" & ListItem.Value & " AND AreaKey=" & AreaFromQueryString)
            Next
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableIframe", "disableIframe();", True)
    End Sub
End Class
