
Imports System.Web.Services

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page

    <WebMethod()>
    Public Shared Function DbWrite(SenderID As String, SenderValue As String) As String
        Dim ExecuteSqlQuery As ExecuteSqlQueryDelegate = HttpContext.Current.Session("ExecuteSqlQuery")
        ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogCell] SET Value='" & SenderValue & "' WHERE [Key]=" & SenderID)
        Return True ' Return a response back to the JavaScript function
    End Function

    Public Delegate Sub ExecuteSqlQueryDelegate(SqlQuery As String)
    Sub ExecuteSqlQuery(SqlQuery As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim MySQLCommand As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()
        With MySQLCommand
            .CommandText = SqlQuery
            .Connection = Connection
        End With
        MySQLCommand.ExecuteNonQuery()
        Connection.Close()
    End Sub

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ExecuteSqlQueryDelegate As ExecuteSqlQueryDelegate = AddressOf ExecuteSqlQuery
        Session("ExecuteSqlQuery") = ExecuteSqlQueryDelegate

        ScriptManager.GetCurrent(Me.Page).EnablePageMethods = True

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetDBConnection", "SetDBConnection('Label_1');", True)
    End Sub

End Class


