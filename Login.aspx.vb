Imports System.Data.SqlClient

Partial Class SatiUsers_Login
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Login1_LoginError(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoginError

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckLogInAuthenication(Page, User, Server)
    End Sub

    Protected Sub Login1_Authenticate(sender As Object, e As AuthenticateEventArgs)
        Dim username As String = Login1.UserName
        Dim password As String = Login1.Password
        Dim DS As Data.DataSet
        Dim UserID As String

        If Membership.ValidateUser(username, password) Then 'updates the "LastActivityDate" datetime field
            UserID = Membership.GetUser(username).ProviderUserKey.ToString()
            DS = SatiCode.GetMyDataSet("SELECT IsAnonymous As InactiveUser FROM [SatiUsers].[dbo].[aspnet_Users] WHERE UserID = '" & UserID & "'")

            'check if user is active
            If Not DS.Tables(0).Rows(0)("InactiveUser") Then
                e.Authenticated = True 'log in user
                Exit Sub
            End If
        End If
        e.Authenticated = False 'do NOT log in user
    End Sub

End Class
