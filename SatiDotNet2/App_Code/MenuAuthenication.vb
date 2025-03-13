Public Class MenuAuthenication

    ''' <summary>
    ''' This method is designed to check the logged in status of a user on the site. This is only
    ''' used for the Login page's VB code.
    ''' You will have to pass it the Page object, User object, Server object, these should be already 
    ''' made when you construct a new code behind class
    ''' </summary>
    ''' <param name="Page"></param>
    ''' <param name="User"></param>
    ''' <param name="Server"></param>
    Public Shared Sub CheckLogInAuthenication(Page As Page, User As System.Security.Principal.IPrincipal, Server As HttpServerUtility)
        If User.Identity.IsAuthenticated = True Then
            Server.Transfer("~/main1.aspx")
            UserMenuSetting(Page)
        End If
    End Sub

    ''' <summary>
    ''' This method is designed to check and ensure that the user is still login to the site. If they
    ''' are not logged in then it will send the user to the login page to have them login. 
    ''' You will have to pass it the Page object, User object, Server object to this method, these should be already 
    ''' made when you construct a new code behind class
    ''' </summary>
    ''' <param name="Page"></param>
    ''' <param name="User"></param>
    ''' <param name="Server"></param>
    Public Shared Sub CheckPageAuthenication(Page As Page, User As System.Security.Principal.IPrincipal, Server As HttpServerUtility)
        If User.Identity.IsAuthenticated = False Then
            Server.Transfer("~/Login.aspx")
        Else
            UserMenuSetting(Page)
        End If
    End Sub

    ''' <summary>
    ''' This method is designed to pass the page information to the server to build the drop down menu
    ''' This does not check Authenication, this just passes the data for the menu. Its purpose to to
    ''' allow the menu to be build without needing a user logged in.
    ''' You will have to pass it the Page object, this should be already be made when you construct a 
    ''' new code behind class
    ''' </summary>
    ''' <param name="Page"></param>
    Public Shared Sub AuthenicationByPass(Page As Page)
        UserMenuSetting(Page)
    End Sub

    ''' <summary>
    ''' This method is designed to work allong side the CheckPageAuthenication method, this method
    ''' does not check if a user is logged in, hence why it needs the other method with it. However,
    ''' this method is designed to check if a user belongs to a passed specified group.
    ''' You will pass a specified group as a string to this class and then the Server object.
    ''' </summary>
    ''' <param name="GroupAuthenication"></param>
    ''' <param name="Server"></param>
    Public Shared Sub CheckGroupAuthenication(GroupAuthenication As String, Server As HttpServerUtility)
        If Roles.IsUserInRole(GroupAuthenication) = False Then
            Server.Transfer("~/UnAuthorized.aspx?GroupName=" + GroupAuthenication)
        End If
    End Sub

    ''' <summary>
    ''' This method is designed to work allong side the CheckPageAuthenication method, this method
    ''' does not check if a user is logged in, hence why it needs the other method with it. However,
    ''' this method is designed to check if a user belongs to a list of specified groups (PLURAL).
    ''' You will pass specified groups as a list to this class and then the Server object.
    ''' </summary>
    ''' <param name="GroupAuthenication"></param>
    ''' <param name="Server"></param>
    Public Shared Sub CheckGroupsAuthenication(GroupsAuthenication As String(), Server As HttpServerUtility)
        Dim UserHas1Role As Boolean = False
        Dim ConcatGroups As String
        Dim LastGroupAuthIdx As Integer = GroupsAuthenication.Count - 1

        For I As Integer = 0 To LastGroupAuthIdx
            Dim GroupAuthenication As String = GroupsAuthenication(I)

            If Roles.IsUserInRole(GroupAuthenication) = True Then
                UserHas1Role = True
            End If

            If I < LastGroupAuthIdx Then
                ConcatGroups += GroupAuthenication & "OR"
            Else
                ConcatGroups += GroupAuthenication
            End If
        Next

        If UserHas1Role = False Then
            Server.Transfer("~/UnAuthorized.aspx?GroupName=" + ConcatGroups)
        End If
    End Sub



    ''' <summary>
    ''' This method is designed to take in the current webpage information and uses it to build the 
    ''' drop down menu on the current webpage. These method is meant to be used with in the MenuAuthenication 
    ''' class. It is a helpper function.
    ''' </summary>
    ''' <param name="Page"></param>
    Protected Shared Sub UserMenuSetting(Page As Page)
        Dim RolesList() As String = System.Web.Security.Roles.GetRolesForUser()
        Dim Roles As String = "None "
        For i As Integer = 0 To RolesList.Length - 1
            Roles += RolesList(i) + " "
        Next

        Page.ClientScript.RegisterHiddenField("Roles", Roles)
    End Sub
End Class
