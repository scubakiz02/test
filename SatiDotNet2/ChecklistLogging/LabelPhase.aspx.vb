
Imports System.Text.Json
Imports SatiDotNet2.Library

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim Security As New Security
    Dim AreaFromQueryString As String
    Dim PhaseFromQueryString As String
    Dim DS As New Data.DataSet
    Dim DR As Data.DataRow
    Dim RC As Integer
    Private QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
    Private ChecklistBuilder As New MaintPM()
    Private Shared LabelPhaseAspx As New AspWebpage("/ChecklistLogging/LabelPhase.aspx", New List(Of String) From {"Area", "Phase"})
    Dim FormViewInsert As FormView = Nothing

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        AreaFromQueryString = Request.QueryString("Area")
        PhaseFromQueryString = Request.QueryString("Phase")
        QueryObject("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }

        If AreaFromQueryString IsNot Nothing Then
            If IsPostBack = False Then
                AreaPhase.Text = Security.GetSingleDbField("SELECT Area FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=@AreaKey", QueryObject, "Area") & " : "

                PhaseListBox_SqlDataSource.SelectCommand = "SELECT [Key], [Phase] FROM [T_LogPhase] WHERE [AreaKey]=@AreaKey ORDER BY PhaseOrder"
                PhaseListBox_SqlDataSource.SelectParameters.Clear()
                PhaseListBox_SqlDataSource.SelectParameters.Add("AreaKey", AreaFromQueryString)
            Else
                LabelPhaseAspx.SetUrl("Area", AreaFromQueryString)
            End If
        End If
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        PhaseListBox.SelectedValue = PhaseFromQueryString

        PhaseFormViewConfig()

        If AreaFromQueryString IsNot Nothing Then
            If FormViewInsert IsNot Nothing Then 'if FormViewInsert has a value, that means that FormView control is in insert mode, and IT NEEDS TO STAY THERE
                FormViewInsert.ChangeMode(FormViewMode.Insert)
                FormViewInsert = Nothing
            End If
        End If

    End Sub

    Protected Sub PhaseListBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        LabelPhaseAspx.SetUrl("Phase", PhaseListBox.SelectedValue)
        Response.Redirect(LabelPhaseAspx.GetUrl())
    End Sub

    Protected Sub UpdateButton_onClick(sender As Object, e As EventArgs)
        Dim PhaseKey As String = PhaseListBox.SelectedValue

        QueryObject("@Phase") = New Dictionary(Of String, String) From {
            {"value", sender.Parent.FindControl("PhaseTextBox").Text},
            {"typeOf", "string"}
        }
        QueryObject("@PhaseKey") = New Dictionary(Of String, String) From {
            {"value", PhaseKey},
            {"typeOf", "int"}
        }
        Security.ExecuteSqlParamQuery("UPDATE [T_LogPhase] SET Phase=@Phase WHERE [Key]=@PhaseKey", QueryObject)

        LabelPhaseAspx.SetUrl("Phase", PhaseKey)
        Response.Redirect(LabelPhaseAspx.GetUrl())
    End Sub


    Protected Sub UpdateCancelButton_OnClick(sender As Object, e As EventArgs)
        Response.Redirect(LabelPhaseAspx.GetUrl())
    End Sub

    Protected Sub DeleteButton_OnClick(sender As Object, e As EventArgs)
        QueryObject("@PhaseKey") = New Dictionary(Of String, String) From {
            {"value", PhaseListBox.SelectedValue},
            {"typeOf", "int"}
        }
        'Security.ExecuteSqlParamQuery("DELETE FROM [ALTS].[dbo].[T_LogPhase] WHERE [Key]=@PhaseKey", QueryObject)
        Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogLabel] SET PhaseKey=NULL WHERE PhaseKey=@PhaseKey; DELETE FROM [ALTS].[dbo].[T_LogPhase] WHERE [Key]=@PhaseKey", QueryObject)

        LabelPhaseAspx.SetUrl("Phase", Nothing)
        Response.Redirect(LabelPhaseAspx.GetUrl())
    End Sub

    Private Sub SetEnabledProps(ParentCtrl As Control, ActiveControl As Control)
        Dim Enabled As Boolean
        Dim FormView As FormView = DirectCast(ActiveControl, FormView)

        If FormView.CurrentMode = FormViewMode.Insert Then
            Enabled = True
        Else
            Enabled = False
        End If

        RecursiveTraverse(ParentCtrl, Sub(Ctrl As Control)
                                          If TypeOf Ctrl Is WebControl Then
                                              Dim CastedCtrl As WebControl = DirectCast(Ctrl, WebControl)
                                              Dim Active As Boolean = False

                                              'check to see if control is a part of or is ActiveControl arg
                                              For Each Child As Control In Ctrl.Controls
                                                  If Child Is ActiveControl Then
                                                      Active = True
                                                  End If
                                              Next

                                              If Ctrl Is ActiveControl Then
                                                  Active = True
                                              End If

                                              If Active Then
                                                  CastedCtrl.Enabled = Not Enabled
                                              Else
                                                  CastedCtrl.Enabled = Enabled
                                              End If
                                          End If
                                      End Sub)
    End Sub

    Private Sub RecursiveTraverse(ByVal Root As Control, Callback As Action(Of Control))
        If Root Is Nothing Then Exit Sub

        Callback(Root)

        For Each Child As Control In Root.Controls
            RecursiveTraverse(Child, Callback)
        Next
    End Sub

    Protected Sub NewButton_OnClick(sender As Object, e As EventArgs)
        SetEnabledProps(PhaseInterfacePanel, PhaseFormView)
    End Sub

    Protected Sub InsertButton_onClick(sender As Object, e As EventArgs)
        Dim UserInput As String

        UserInput = sender.Parent.FindControl("PhaseTextBox").Text
        If String.IsNullOrEmpty(UserInput) Then
            FormViewInsert = PhaseFormView 'Page_PreRenderComplete will ensure FormView stays in Insert mode
            Exit Sub
        End If

        QueryObject("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }
        QueryObject("@UserInput") = New Dictionary(Of String, String) From {
            {"value", UserInput},
            {"typeOf", "string"}
        }
        QueryObject("@PhaseOrder") = New Dictionary(Of String, String) From {
            {"value", Security.GetSingleDbField("SELECT TOP(1) PhaseOrder FROM [ALTS].[dbo].[T_LogPhase] WHERE AreaKey=@AreaKey ORDER BY [Key] DESC", QueryObject, "PhaseOrder") + 1},
            {"typeOf", "int"}
        }
        Security.ExecuteSqlParamQuery("INSERT INTO [ALTS].[dbo].[T_LogPhase] (AreaKey, Phase, PhaseOrder) VALUES (@AreaKey, @UserInput, @PhaseOrder);", QueryObject)

        LabelPhaseAspx.SetUrl("Phase", Security.GetSingleDbField("SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogPhase] WHERE AreaKey=@AreaKey And Phase=@UserInput And PhaseOrder=@PhaseOrder ORDER BY [Key] DESC", QueryObject, "Key"))
        Response.Redirect(LabelPhaseAspx.GetUrl())
    End Sub

    Protected Sub InsertCancelButton_onClick(sender As Object, e As EventArgs)
        Response.Redirect(LabelPhaseAspx.GetUrl())
    End Sub

    Protected Sub EditButton_OnClick(sender As Object, e As EventArgs)
        PhaseFormViewConfig()
        SetEnabledProps(PhaseInterfacePanel, PhaseFormView)
    End Sub

    Private Sub PhaseFormViewConfig()
        PhaseFormView_SqlDataSource.SelectCommand = "Select [Key], [Phase] FROM [T_LogPhase] WHERE [Key]=@PhaseKey"
        PhaseFormView_SqlDataSource.SelectParameters.Clear()
        PhaseFormView_SqlDataSource.SelectParameters.Add("PhaseKey", PhaseFromQueryString)
    End Sub


    Protected Sub PhaseOrderInterface_onClick(sender As Object, e As EventArgs)
        Dim Action As String
        Dim UpdateQuery As String
        Dim ModifyOrderConfig As New Dictionary(Of String, String)

        Select Case sender.ID
            Case "UpInOrderPhaseButton"
                Action = "up"
            Case "DownInOrderPhaseButton"
                Action = "down"
        End Select

        ModifyOrderConfig = ChecklistBuilder.ModifyOrder(PhaseListBox.SelectedValue, Action, "Phase")
        UpdateQuery = ModifyOrderConfig("SqlQuery")

        If String.IsNullOrEmpty(UpdateQuery) = False Then
            Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderConfig("ParameterizedValues"))

            Security.ExecuteSqlParamQuery(UpdateQuery, QueryConfig)

            LabelPhaseAspx.SetUrl("Phase", PhaseListBox.SelectedValue)
            Response.Redirect(LabelPhaseAspx.GetUrl())
        End If
    End Sub
End Class
