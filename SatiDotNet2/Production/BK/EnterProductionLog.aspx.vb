Imports System.Activities.Expressions
Imports Class1
Partial Class Production_EnterProductionLog
    Inherits System.Web.UI.Page
    Dim AddBottonHid As Boolean
    Dim AddBottonHid1 As Boolean
    Dim AddBottonHid2 As Boolean
    Dim SatiCode As Class1 = New Class1

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If Not Me.IsPostBack Then
            Load_Main()
        End If
    End Sub
    Sub Load_Main()
        Me.ShiftPanel.Visible = False

        If ViewState("GrayboxHotFix") = False Then
            Me.ErrorMessagePanel.Visible = False
            Me.BottemPanel.Visible = True
            ViewState.Add("GrayboxHotFix", True)
        End If
    End Sub

    Protected Sub ShiftDropDown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ShiftDropDown.SelectedIndexChanged
        If Me.ShiftDropDown.SelectedItem.Text = "Select A Shift..." Then
            AddBottonHid = False
            Me.ViewState.Add("AddBottonHid", AddBottonHid)
        Else
            AddBottonHid = True
            Me.ViewState.Add("AddBottonHid", AddBottonHid)
        End If


        If Me.ErrorMessagePanel.Visible = True Then
            Me.ErrorMessagePanel.Visible = False
            Me.BottemPanel.Visible = True
        End If

        AddBottonHidden()
    End Sub
    Protected Sub DepartmentDropDown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DepartmentDropDown.SelectedIndexChanged
        If Me.DepartmentDropDown.SelectedItem.Text = "Select A Department..." Then
            AddBottonHid1 = False
            Me.ViewState.Add("AddBottonHid1", AddBottonHid1)
        Else
            AddBottonHid1 = True
            Me.ViewState.Add("AddBottonHid1", AddBottonHid1)
        End If

        If Me.ErrorMessagePanel.Visible = True Then
            Me.ErrorMessagePanel.Visible = False
            Me.BottemPanel.Visible = True
        End If

        AddBottonHidden()
    End Sub
    Protected Sub DateTextBox_SelectionChanged(sender As Object, e As EventArgs) Handles DateTextBox.SelectionChanged
        AddBottonHid2 = True
        Me.ViewState.Add("AddBottonHid2", AddBottonHid2)

        If Me.ErrorMessagePanel.Visible = True Then
            Me.ErrorMessagePanel.Visible = False
            Me.BottemPanel.Visible = True
        End If

        AddBottonHidden()
    End Sub

    Sub AddBottonHidden()
        If Me.ViewState("AddBottonHid") And Me.ViewState("AddBottonHid1") And Me.ViewState("AddBottonHid2") Then
            Me.ShiftPanel.Visible = True

            If ShiftDropDown.SelectedItem.Text = "D1" Or ShiftDropDown.SelectedItem.Text = "D2" Then
                SetPreLogNight()
                SetProLogDTNight()
            Else
                SetPreLogDay()
                SetProLogDTDay()
            End If

            Me.ProLogTableCurrent.DataBind()
            Me.ProLogTablePre.DataBind()
            Me.ProLogDTCurrent.DataBind()
            Me.ProLogDTPre.DataBind()

            PDL5.Text = DateTextBox.SelectedDate.ToString("yyyy")
            PDL10.Text = DateTextBox.SelectedDate.ToString("yyyy")
            DDL5.Text = DateTextBox.SelectedDate.ToString("yyyy")
            DDL10.Text = DateTextBox.SelectedDate.ToString("yyyy")

            LogStartMN.SelectedIndex = DateTextBox.SelectedDate.Month()
            LogStartDD.SelectedIndex = DateTextBox.SelectedDate.Day()
            LogEndMN.SelectedIndex = DateTextBox.SelectedDate.Month()
            LogEndDD.SelectedIndex = DateTextBox.SelectedDate.Day()

            DTStartMN.SelectedIndex = DateTextBox.SelectedDate.Month()
            DTStartDD.SelectedIndex = DateTextBox.SelectedDate.Day()
            DTEndMN.SelectedIndex = DateTextBox.SelectedDate.Month()
            DTEndDD.SelectedIndex = DateTextBox.SelectedDate.Day()

            LN.Focus()
        End If
    End Sub

    Protected Sub SetPreLogNight()
        Dim Yesterday As Date = DateTextBox.SelectedDate.AddDays(-1)

        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Shift, ReportDate FROM T_ProLogLots WHERE (ReportDate = '" & Yesterday & "')"

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim ShiftDB As String = ""
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If (checker(0) = "N1" Or checker(0) = "N2") Then
                ShiftDB = checker(0)
            End If
        End While
        connect.Close()

        conString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim query As String = "SELECT [Key], Shift, Department, LotNumber, QtyCompleat, QtyPass, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) As Hours FROM T_ProLogLots"
        Dim where As String = " WHERE (ReportDate = '" & Yesterday & "') AND (Shift = '" & ShiftDB & "') AND (Department = @Department)"
        Dim order As String = " ORDER BY ReportDate, StartTime"

        Me.SqlDataSourceProLogLotsPre.SelectCommand = query & where & order
        Me.ProLogTablePre.DataBind()
    End Sub
    Protected Sub SetPreLogDay()
        Dim Yesterday As Date = DateTextBox.SelectedDate.AddDays(-1)

        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Shift, ReportDate FROM T_ProLogLots WHERE (ReportDate = '" & Yesterday & "')"

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim ShiftDB As String = ""
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If (checker(0) = "D1" Or checker(0) = "D2") Then
                ShiftDB = checker(0)
            End If
        End While
        connect.Close()

        conString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim query As String = "SELECT [Key], Shift, Department, LotNumber, QtyCompleat, QtyPass, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) As Hours FROM T_ProLogLots"
        Dim where As String = " WHERE (ReportDate = '" & Yesterday & "') AND (Shift = '" & ShiftDB & "') AND (Department = @Department)"
        Dim order As String = " ORDER BY ReportDate, StartTime"

        Me.SqlDataSourceProLogLotsPre.SelectCommand = query & where & order
        Me.ProLogTablePre.DataBind()
    End Sub

    Protected Sub SetProLogDTNight()
        Dim Yesterday As Date = DateTextBox.SelectedDate.AddDays(-1)

        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Shift, ReportDate FROM T_ProLogDT WHERE (ReportDate = '" & Yesterday & "')"

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim ShiftDB As String = ""
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If (checker(0) = "N1" Or checker(0) = "N2") Then
                ShiftDB = checker(0)
            End If
        End While
        connect.Close()

        conString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim query As String = "SELECT [Key], Shift, Department, Event, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) As Hours FROM T_ProLogDT"
        Dim where As String = " WHERE (ReportDate = '" & Yesterday & "') AND (Shift = '" & ShiftDB & "') AND (Department = @Department)"
        Dim order As String = " ORDER BY ReportDate, StartTime"

        Me.SqlDataSourceProLogDTPre.SelectCommand = query & where & order
        Me.ProLogDTPre.DataBind()
    End Sub
    Protected Sub SetProLogDTDay()
        Dim Yesterday As Date = DateTextBox.SelectedDate.AddDays(-1)

        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Shift, ReportDate FROM T_ProLogDT WHERE (ReportDate = '" & Yesterday & "')"

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim ShiftDB As String = ""
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If (checker(0) = "D1" Or checker(0) = "D2") Then
                ShiftDB = checker(0)
            End If
        End While
        connect.Close()

        conString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim query As String = "SELECT [Key], Shift, Department, Event, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) As Hours FROM T_ProLogDT"
        Dim where As String = " WHERE (ReportDate = '" & Yesterday & "') AND (Shift = '" & ShiftDB & "') AND (Department = @Department)"
        Dim order As String = " ORDER BY ReportDate, StartTime"

        Me.SqlDataSourceProLogDTPre.SelectCommand = query & where & order
        Me.ProLogDTPre.DataBind()
    End Sub

    Protected Sub ProLogTable_AddRowCommand(sender As Object, e As GridViewCommandEventArgs) Handles ProLogTableCurrent.RowCommand
        If e.CommandName = "Delete" Then
            Dim index As Integer = e.CommandArgument
            Dim row As GridViewRow = ProLogTableCurrent.Rows(index)
            Dim keyTable As Integer = ProLogTableCurrent.DataKeys(row.RowIndex)(0).ToString()

            Me.ViewState.Add("KeyDTable", keyTable)
        End If

        If e.CommandName = "Edit" Then
            Dim index As Integer = e.CommandArgument
            Dim row As GridViewRow = ProLogTableCurrent.Rows(index)
            Dim keyTable As Integer = ProLogTableCurrent.DataKeys(row.RowIndex)(0).ToString()

            Me.ViewState.Add("KeyETable", keyTable)
        End If
    End Sub

    Protected Sub SqlDataSourceProLogLots_Inserting(ByVal sender As Object, ByVal e As SqlDataSourceCommandEventArgs) Handles SqlDataSourceProLogLots.Inserting

        Dim AddCompleteQty As TextBox
        Dim AddQtyPass As TextBox
        Dim AddLotNumber As TextBox

        Dim AddStartDate As String
        Dim AddStartMN As DropDownList
        Dim AddStartDD As DropDownList

        Dim AddStartTime As String
        Dim AddStartHH As DropDownList
        Dim AddStartMM As DropDownList
        Dim AddStartTZ As DropDownList

        Dim AddEndDate As String
        Dim AddEndMN As DropDownList
        Dim AddEndDD As DropDownList

        Dim AddEndTime As String
        Dim AddEndHH As DropDownList
        Dim AddEndMM As DropDownList
        Dim AddEndTZ As DropDownList


        AddCompleteQty = CType(Me.QC, TextBox)
        AddQtyPass = CType(Me.QP, TextBox)
        AddLotNumber = CType(Me.LN, TextBox)
        AddStartMN = CType(Me.LogStartMN, DropDownList)
        AddStartDD = CType(Me.LogStartDD, DropDownList)
        AddStartDate = AddStartMN.SelectedItem.Text & "/" & AddStartDD.SelectedItem.Text & "/" & Date.Today.Year.ToString
        AddStartHH = CType(Me.LogStartHH, DropDownList)
        AddStartMM = CType(Me.LogStartMM, DropDownList)
        AddStartTZ = CType(Me.LogStartTZ, DropDownList)
        AddStartTime = AddStartHH.SelectedItem.Text & ":" & AddStartMM.SelectedItem.Text & ":00 " & AddStartTZ.SelectedItem.Text

        AddEndMN = CType(Me.LogEndMN, DropDownList)
        AddEndDD = CType(Me.LogEndDD, DropDownList)
        AddEndDate = AddEndMN.SelectedItem.Text & "/" & AddEndDD.SelectedItem.Text & "/" & Date.Today.Year.ToString
        AddEndHH = CType(Me.LogEndHH, DropDownList)
        AddEndMM = CType(Me.LogEndMM, DropDownList)
        AddEndTZ = CType(Me.LogEndTZ, DropDownList)
        AddEndTime = AddEndHH.SelectedItem.Text & ":" & AddEndMM.SelectedItem.Text & ":00 " & AddEndTZ.SelectedItem.Text

        If AddStartMN.SelectedItem.Text = "02" Or AddStartMN.SelectedItem.Text = "04" Or AddStartMN.SelectedItem.Text = "06" Or AddStartMN.SelectedItem.Text = "09" Or AddStartMN.SelectedItem.Text = "11" Then
            If AddStartDD.SelectedItem.Text = "31" Then
                AddStartDate = AddStartMN.SelectedItem.Text & "/30/" & Date.Today.Year.ToString
            End If

            If AddStartMN.SelectedItem.Text = "02" And (AddStartDD.SelectedItem.Text = "29" Or AddStartDD.SelectedItem.Text = "30" Or AddStartDD.SelectedItem.Text = "31") Then
                If Date.Today.Year Mod 4 = 0 Then
                    AddStartDate = AddStartMN.SelectedItem.Text & "/29/" & Date.Today.Year.ToString
                Else
                    AddStartDate = AddStartMN.SelectedItem.Text & "/28/" & Date.Today.Year.ToString
                End If
            End If
        End If

        If AddEndMN.SelectedItem.Text = "02" Or AddEndMN.SelectedItem.Text = "04" Or AddEndMN.SelectedItem.Text = "06" Or AddEndMN.SelectedItem.Text = "09" Or AddEndMN.SelectedItem.Text = "11" Then
            If AddEndDD.SelectedItem.Text = "31" Then
                AddEndDate = AddEndMN.SelectedItem.Text & "/30/" & Date.Today.Year.ToString
            End If
            If AddEndMN.SelectedItem.Text = "02" And (AddEndDD.SelectedItem.Text = "29" Or AddEndDD.SelectedItem.Text = "30" Or AddEndDD.SelectedItem.Text = "31") Then
                If Date.Today.Year Mod 4 = 0 Then
                    AddEndDate = AddEndMN.SelectedItem.Text & "/29/" & Date.Today.Year.ToString
                Else
                    AddStartDate = AddEndMN.SelectedItem.Text & "/28/" & Date.Today.Year.ToString
                End If
            End If
        End If

        Dim CQTest As Integer
        Dim QPTest As Integer

        Dim TempSDT As String = AddStartDate + " " + AddStartTime
        Dim TempEDT As String = AddEndDate + " " + AddEndTime

        Dim SDT As DateTime
        Dim EDT As DateTime

        Dim RDT As Date
        Date.TryParse(Me.DateTextBox.SelectedDate, RDT)
        Dim TempRDT As String = RDT.Date.ToString
        Dim DatePartsArray() As String = TempRDT.Split(" ")
        TempRDT = DatePartsArray(0)

        If AddStartDate = "MM/DD/" & Today.Year.ToString Or AddStartMN.SelectedItem.Text = "MM" Or AddStartDD.SelectedItem.Text = "DD" Then
            TempSDT = TempRDT + " " + AddStartTime
            SDT = DateTime.Parse(TempSDT)
        Else
            SDT = DateTime.Parse(TempSDT)
        End If

        If AddEndDate = "MM/DD/" & Today.Year.ToString Or AddEndMN.SelectedItem.Text = "MM" Or AddEndDD.SelectedItem.Text = "DD" Then
            TempEDT = TempRDT + " " + AddEndTime
            EDT = DateTime.Parse(TempEDT)
        Else
            EDT = DateTime.Parse(TempEDT)
        End If

        If Integer.TryParse(AddCompleteQty.Text, CQTest) Then
            e.Command.Parameters("@QtyPass").Value = AddQtyPass.Text
        End If

        If Integer.TryParse(AddQtyPass.Text, QPTest) Then
            e.Command.Parameters("@QtyCompleat").Value = AddCompleteQty.Text
        End If

        e.Command.Parameters("@Shift").Value = ShiftDropDown.SelectedItem.Text
        e.Command.Parameters("@Department").Value = DepartmentDropDown.SelectedItem.Text
        e.Command.Parameters("@ReportDate").Value = RDT
        e.Command.Parameters("@LotNumber").Value = AddLotNumber.Text
        e.Command.Parameters("@StartTime").Value = SDT
        e.Command.Parameters("@EndTime").Value = EDT
        e.Command.Parameters("@OP").Value = User.Identity.Name.ToString

    End Sub

    Protected Sub CreateRecordButton_Click(sender As Object, e As EventArgs) Handles CreateRecordButton.Click
        CreateRecord()
        AddBottonHidden()

        CType(Me.QC, TextBox).Text = String.Empty
        CType(Me.QP, TextBox).Text = String.Empty
        CType(Me.LN, TextBox).Text = String.Empty
        CType(Me.LogStartMN, DropDownList).SelectedIndex = DateTextBox.SelectedDate.Month
        CType(Me.LogStartDD, DropDownList).SelectedIndex = DateTextBox.SelectedDate.Day
        CType(Me.LogStartHH, DropDownList).SelectedIndex = 0
        CType(Me.LogStartMM, DropDownList).SelectedIndex = 0
        CType(Me.LogStartTZ, DropDownList).SelectedIndex = 0
        CType(Me.LogEndMN, DropDownList).SelectedIndex = DateTextBox.SelectedDate.Month
        CType(Me.LogEndDD, DropDownList).SelectedIndex = DateTextBox.SelectedDate.Day
        CType(Me.LogEndHH, DropDownList).SelectedIndex = 0
        CType(Me.LogEndMM, DropDownList).SelectedIndex = 0
        CType(Me.LogEndTZ, DropDownList).SelectedIndex = 0
    End Sub

    Sub CreateRecord()

        If Me.ErrorMessagePanel.Visible = True Then
            Me.ErrorMessagePanel.Visible = False
            Me.BottemPanel.Visible = True
        End If

        Dim AddCompleteQty As TextBox
        Dim AddQtyPass As TextBox
        Dim AddLotNumber As TextBox

        Dim AddStartDate As String
        Dim AddStartMN As DropDownList
        Dim AddStartDD As DropDownList

        Dim AddStartTime As String
        Dim AddStartHH As DropDownList
        Dim AddStartMM As DropDownList
        Dim AddStartTZ As DropDownList

        Dim AddEndDate As String
        Dim AddEndMN As DropDownList
        Dim AddEndDD As DropDownList

        Dim AddEndTime As String
        Dim AddEndHH As DropDownList
        Dim AddEndMM As DropDownList
        Dim AddEndTZ As DropDownList


        AddCompleteQty = CType(Me.QC, TextBox)
        AddQtyPass = CType(Me.QP, TextBox)
        AddLotNumber = CType(Me.LN, TextBox)
        AddStartMN = CType(Me.LogStartMN, DropDownList)
        AddStartDD = CType(Me.LogStartDD, DropDownList)
        AddStartDate = AddStartMN.SelectedItem.Text & "/" & AddStartDD.SelectedItem.Text & "/" & Date.Today.Year.ToString
        AddStartHH = CType(Me.LogStartHH, DropDownList)
        AddStartMM = CType(Me.LogStartMM, DropDownList)
        AddStartTZ = CType(Me.LogStartTZ, DropDownList)
        AddStartTime = AddStartHH.SelectedItem.Text & ":" & AddStartMM.SelectedItem.Text & ":00 " & AddStartTZ.SelectedItem.Text

        AddEndMN = CType(Me.LogEndMN, DropDownList)
        AddEndDD = CType(Me.LogEndDD, DropDownList)
        AddEndDate = AddEndMN.SelectedItem.Text & "/" & AddEndDD.SelectedItem.Text & "/" & Date.Today.Year.ToString
        AddEndHH = CType(Me.LogEndHH, DropDownList)
        AddEndMM = CType(Me.LogEndMM, DropDownList)
        AddEndTZ = CType(Me.LogEndTZ, DropDownList)
        AddEndTime = AddEndHH.SelectedItem.Text & ":" & AddEndMM.SelectedItem.Text & ":00 " & AddEndTZ.SelectedItem.Text

        If AddStartMN.SelectedItem.Text = "02" Or AddStartMN.SelectedItem.Text = "04" Or AddStartMN.SelectedItem.Text = "06" Or AddStartMN.SelectedItem.Text = "09" Or AddStartMN.SelectedItem.Text = "11" Then
            If AddStartDD.SelectedItem.Text = "31" Then
                AddStartDate = AddStartMN.SelectedItem.Text & "/30/" & Date.Today.Year.ToString
            End If

            If AddStartMN.SelectedItem.Text = "02" And (AddStartDD.SelectedItem.Text = "29" Or AddStartDD.SelectedItem.Text = "30" Or AddStartDD.SelectedItem.Text = "31") Then
                If Date.Today.Year Mod 4 = 0 Then
                    AddStartDate = AddStartMN.SelectedItem.Text & "/29/" & Date.Today.Year.ToString
                Else
                    AddStartDate = AddStartMN.SelectedItem.Text & "/28/" & Date.Today.Year.ToString
                End If
            End If
        End If

        If AddEndMN.SelectedItem.Text = "02" Or AddEndMN.SelectedItem.Text = "04" Or AddEndMN.SelectedItem.Text = "06" Or AddEndMN.SelectedItem.Text = "09" Or AddEndMN.SelectedItem.Text = "11" Then
            If AddEndDD.SelectedItem.Text = "31" Then
                AddEndDate = AddEndMN.SelectedItem.Text & "/30/" & Date.Today.Year.ToString
            End If
            If AddEndMN.SelectedItem.Text = "02" And (AddEndDD.SelectedItem.Text = "29" Or AddEndDD.SelectedItem.Text = "30" Or AddEndDD.SelectedItem.Text = "31") Then
                If Date.Today.Year Mod 4 = 0 Then
                    AddEndDate = AddEndMN.SelectedItem.Text & "/29/" & Date.Today.Year.ToString
                Else
                    AddStartDate = AddEndMN.SelectedItem.Text & "/28/" & Date.Today.Year.ToString
                End If
            End If
        End If

        Dim EDT As DateTime
        Dim SDT As DateTime
        Dim TempSDT As String = AddStartDate + " " + AddStartTime
        Dim TempEDT As String = AddEndDate + " " + AddEndTime

        Dim Error0 As Boolean = True
        Dim Error1 As Boolean = True
        Dim Error2 As Boolean = True
        Dim ErrorT As Boolean = True

        Dim Warning0 As Boolean = True
        Dim Warning1 As Boolean = True

        Dim CQTest As Integer
        Dim QPTest As Integer

        If SatiCode.IsLotNumberReal(AddLotNumber.Text) = False Then
            Error0 = False
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error: LOT NUMBER was invalid in the system."
            Me.ErrorMessage.ForeColor = Drawing.Color.Red
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
        End If
        If QualCheck.Checked = True Then
            If LN.Text.Length <= 20 Then
                If LN.Text = "" Then
                    Error0 = False
                    Me.ErrorMessagePanel.Visible = True
                    Me.BottemPanel.Visible = False
                    Me.ErrorMessage.Text = "Error: QUALITY LOT NUMBER can be blank."
                    Me.ErrorMessage.ForeColor = Drawing.Color.Red
                    Me.ErrorMessage.Font.Size = 15
                    Me.ErrorMessage.Font.Bold = True
                Else
                    Error0 = True
                    Me.ErrorMessagePanel.Visible = False
                    Me.BottemPanel.Visible = True
                End If
            Else
                Error0 = False
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Error: QUALITY LOT NUMBER was too long (20 Charaacters or less)."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
        End If

        If Integer.TryParse(AddCompleteQty.Text, CQTest) = True Or Integer.TryParse(AddCompleteQty.Text, CQTest) = False Then
            If CQTest < 0 Then
                Error1 = False
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Error: COMPLETED QUANTITY has to be a positive number."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True

            ElseIf Integer.TryParse(AddCompleteQty.Text, CQTest) = False Then
                Error1 = False
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Error: COMPLETED QUANTITY has to be a number."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
        End If

        If Integer.TryParse(AddQtyPass.Text, QPTest) = True Or Integer.TryParse(AddQtyPass.Text, QPTest) = False Then
            If QPTest < 0 Then
                Error2 = False
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Error: PASSED QUANTITY has to be a positive number."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True

            ElseIf Integer.TryParse(AddQtyPass.Text, QPTest) = False Then
                Error2 = False
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Error: PASSED QUANTITY has to be a number."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
        End If

        Dim RDT As Date
        Date.TryParse(Me.DateTextBox.SelectedDate, RDT)
        Dim TempRDT As String = RDT.Date.ToString
        Dim DatePartsArray() As String = TempRDT.Split(" ")
        TempRDT = DatePartsArray(0)

        If AddStartDate = "MM/DD/" & Today.Year.ToString Or AddStartMN.SelectedItem.Text = "MM" Or AddStartDD.SelectedItem.Text = "DD" Then
            TempSDT = TempRDT + " " + AddStartTime
            SDT = DateTime.Parse(TempSDT)

            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Warning: START DATE was set to " & RDT & ". No date was enter."
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            Warning0 = False
        Else
            SDT = DateTime.Parse(TempSDT)
        End If

        If AddEndDate = "MM/DD/" & Today.Year.ToString Or AddEndMN.SelectedItem.Text = "MM" Or AddEndDD.SelectedItem.Text = "DD" Then
            TempEDT = TempRDT + " " + AddEndTime
            EDT = DateTime.Parse(TempEDT)

            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Warning: END DATE was set to " & RDT & ". No date was enter."
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            Warning1 = False
        Else
            EDT = DateTime.Parse(TempEDT)
        End If

        'MULTI ERROR CASES
        If Error0 = False Or Error1 = False Or Error2 = False Or Warning0 = False Or Warning1 = False Then
            If AddLotNumber.Text = "" And AddCompleteQty.Text = "" And AddQtyPass.Text = "" And AddStartDate = "" And AddEndDate = "" Then
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.ForeColor = Drawing.Color.Black
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
                Me.ErrorMessage.Text = "Warning: Nothing was entered"
            End If
            If Error0 = False And (Error1 = False Or Error2 = False) And (Warning0 = False Or Warning1 = False) Then
                Error0 = False
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Error/Warning: LOT NUMBER and QUANTITIES were invalid / No DATES found. Would be set to " & RDT & "."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If


            If Error1 = False And Error2 = False Then
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Error: Both COMPLETED QUANTITY and PASSED QUANTITY are invalid."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
            If Warning0 = False And Warning1 = False Then
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Warning: START and END DATE were both set to " & RDT & ". No dates were enter."
                Me.ErrorMessage.ForeColor = Drawing.Color.Black
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
            If Error0 = False And (Error1 = False Or Error2 = False) Then
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
                If Error1 = False And Error2 = False Then
                    Me.ErrorMessage.Text = "Error: LOT NUMBER and Both QUANTITIES are invalid."
                Else
                    If Error0 = False And Error1 = False Then
                        Me.ErrorMessage.Text = "Error: LOT NUMBER and COMPLETE QUANTITIES are invalid."
                    Else
                        Me.ErrorMessage.Text = "Error: LOT NUMBER and PASSED QUANTITIES are invalid."
                    End If
                End If
            End If
            If Error0 = False And (Warning0 = False Or Warning1 = False) Then
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
                If Warning0 = False And Warning1 = False Then
                    Me.ErrorMessage.Text = "Error/Warning: LOT NUMBER was invalid / No DATES found. Both would be set to " & RDT & "."
                Else
                    If Error0 = False And Warning0 = False Then
                        Me.ErrorMessage.Text = "Error/Warning: LOT NUMBER was invalid / No Date found. START DATE would have been " & RDT & "."
                    Else
                        Me.ErrorMessage.Text = "Error/Warning: LOT NUMBER was invalid / No Date found. END DATE would have been " & RDT & "."
                    End If
                End If
            End If
            If Error1 = False And (Warning0 = False Or Warning1 = False) Then
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
                If Warning0 = False And Warning1 = False Then
                    Me.ErrorMessage.Text = "Error/Warning: COMPLETE QUANTITY was invalid / No DATES found. Both would be set to " & RDT & "."
                Else
                    If Error1 = False And Warning0 = False Then
                        Me.ErrorMessage.Text = "Error/Warning: COMPLETE QUANTITY was invalid / No Date found. START DATE would have been " & RDT & "."
                    Else
                        Me.ErrorMessage.Text = "Error/Warning: COMPLETE QUANTITY was invalid / No Date found. END DATE would have been " & RDT & "."
                    End If
                End If
                If Error2 = False And (Warning0 = False Or Warning1 = False) Then
                    Me.ErrorMessagePanel.Visible = True
                    Me.BottemPanel.Visible = False
                    Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                    Me.ErrorMessage.Font.Size = 15
                    Me.ErrorMessage.Font.Bold = True
                    If Warning0 = False And Warning1 = False Then
                        Me.ErrorMessage.Text = "Error/Warning: PASSED QUANTITY was invalid / No DATES found. Both would be set to " & RDT & "."
                    Else
                        If Error2 = False And Warning0 = False Then
                            Me.ErrorMessage.Text = "Error/Warning: PASSED QUANTITY was invalid / No Date found. START DATE would have been " & RDT & "."
                        Else
                            Me.ErrorMessage.Text = "Error/Warning: PASSED QUANTITY was invalid / No Date found. END DATE would have been " & RDT & "."
                        End If
                    End If

                End If

            End If
        End If



        Dim TotalDailyHours As Decimal = DateDiff(DateInterval.Minute, SDT, EDT)
        TotalDailyHours /= 60

        If TotalDailyHours > 12.5 Then
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error: Your START and END TIMES can not give you over 12 and a half hours of work."
            Me.ErrorMessage.ForeColor = Drawing.Color.Red
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            ErrorT = False
        ElseIf TotalDailyHours = 0 Then
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error: Your START and END TIMES can not give you 0 hours of work."
            Me.ErrorMessage.ForeColor = Drawing.Color.Red
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            ErrorT = False
        ElseIf TotalDailyHours < 0 Then
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error: END DATE OR TIME has to be later than the START DATE OR TIME. Cannot have negative hours"
            Me.ErrorMessage.ForeColor = Drawing.Color.Red
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            ErrorT = False
        End If

        If Error0 = True And Error1 = True And Error2 = True And ErrorT = True Then
            SqlDataSourceProLogLots.Insert()
        End If
    End Sub

    Protected Sub ProLogDT_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles ProLogDTCurrent.RowCommand
        If e.CommandName = "Delete" Then
            Dim index As Integer = e.CommandArgument
            Dim row As GridViewRow = ProLogDTCurrent.Rows(index)
            Dim keyDDT As Integer = ProLogDTCurrent.DataKeys(row.RowIndex)(0).ToString()

            Me.ViewState.Add("KeyDDT", keyDDT)
        End If
        If e.CommandName = "Edit" Then
            Dim index As Integer = e.CommandArgument
            Dim row As GridViewRow = ProLogDTCurrent.Rows(index)
            Dim keyEDT As Integer = ProLogDTCurrent.DataKeys(row.RowIndex)(0).ToString()

            Me.ViewState.Add("KeyEDT", keyEDT)
        End If
    End Sub

    Protected Sub SqlDataSourceProLogDT_Inserting(ByVal sender As Object, ByVal e As SqlDataSourceCommandEventArgs) Handles SqlDataSourceProLogDT.Inserting
        Dim AddEvent As TextBox

        Dim AddStartTime As String
        Dim AddStartHH As DropDownList
        Dim AddStartMM As DropDownList
        Dim AddStartTZ As DropDownList

        Dim AddStartDate As String
        Dim AddStartMN As DropDownList
        Dim AddStartDD As DropDownList

        Dim AddEndDate As String
        Dim AddEndMN As DropDownList
        Dim AddEndDD As DropDownList

        Dim AddEndTime As String
        Dim AddEndHH As DropDownList
        Dim AddEndMM As DropDownList
        Dim AddEndTZ As DropDownList



        AddEvent = CType(Me.AddEvent, TextBox)

        AddStartMN = CType(Me.DTStartMN, DropDownList)
        AddStartDD = CType(Me.DTStartDD, DropDownList)
        AddStartDate = AddStartMN.SelectedItem.Text & "/" & AddStartDD.SelectedItem.Text & "/" & Date.Today.Year.ToString
        AddStartHH = CType(Me.DTStartHH, DropDownList)
        AddStartMM = CType(Me.DTStartMM, DropDownList)
        AddStartTZ = CType(Me.DTStartTT, DropDownList)
        AddStartTime = AddStartHH.SelectedItem.Text & ":" & AddStartMM.SelectedItem.Text & ":00 " & AddStartTZ.SelectedItem.Text

        AddEndMN = CType(Me.DTEndMN, DropDownList)
        AddEndDD = CType(Me.DTEndDD, DropDownList)
        AddEndDate = AddEndMN.SelectedItem.Text & "/" & AddEndDD.SelectedItem.Text & "/" & Date.Today.Year.ToString
        AddEndHH = CType(Me.DTEndHH, DropDownList)
        AddEndMM = CType(Me.DTEndMM, DropDownList)
        AddEndTZ = CType(Me.DTEndTT, DropDownList)
        AddEndTime = AddEndHH.SelectedItem.Text & ":" & AddEndMM.SelectedItem.Text & ":00 " & AddEndTZ.SelectedItem.Text

        If AddStartMN.SelectedItem.Text = "02" Or AddStartMN.SelectedItem.Text = "04" Or AddStartMN.SelectedItem.Text = "06" Or AddStartMN.SelectedItem.Text = "09" Or AddStartMN.SelectedItem.Text = "11" Then
            If AddStartDD.SelectedItem.Text = "31" Then
                AddStartDate = AddStartMN.SelectedItem.Text & "/30/" & Date.Today.Year.ToString
            End If

            If AddStartMN.SelectedItem.Text = "02" And (AddStartDD.SelectedItem.Text = "29" Or AddStartDD.SelectedItem.Text = "30" Or AddStartDD.SelectedItem.Text = "31") Then
                If Date.Today.Year Mod 4 = 0 Then
                    AddStartDate = AddStartMN.SelectedItem.Text & "/29/" & Date.Today.Year.ToString
                Else
                    AddStartDate = AddStartMN.SelectedItem.Text & "/28/" & Date.Today.Year.ToString
                End If
            End If
        End If

        If AddEndMN.SelectedItem.Text = "02" Or AddEndMN.SelectedItem.Text = "04" Or AddEndMN.SelectedItem.Text = "06" Or AddEndMN.SelectedItem.Text = "09" Or AddEndMN.SelectedItem.Text = "11" Then
            If AddEndDD.SelectedItem.Text = "31" Then
                AddEndDate = AddEndMN.SelectedItem.Text & "/30/" & Date.Today.Year.ToString
            End If
            If AddEndMN.SelectedItem.Text = "02" And (AddEndDD.SelectedItem.Text = "29" Or AddEndDD.SelectedItem.Text = "30" Or AddEndDD.SelectedItem.Text = "31") Then
                If Date.Today.Year Mod 4 = 0 Then
                    AddEndDate = AddEndMN.SelectedItem.Text & "/29/" & Date.Today.Year.ToString
                Else
                    AddStartDate = AddEndMN.SelectedItem.Text & "/28/" & Date.Today.Year.ToString
                End If
            End If
        End If

        Dim TempSDT As String = AddStartDate + " " + AddStartTime
        Dim TempEDT As String = AddEndDate + " " + AddEndTime

        Dim SDT As DateTime
        Dim EDT As DateTime

        Dim RDT As Date
        Date.TryParse(Me.DateTextBox.SelectedDate, RDT)
        Dim TempRDT As String = RDT.Date.ToString
        Dim DatePartsArray() As String = TempRDT.Split(" ")
        TempRDT = DatePartsArray(0)

        If AddStartDate = "MM/DD/" & Today.Year.ToString Or AddStartMN.SelectedItem.Text = "MM" Or AddStartDD.SelectedItem.Text = "DD" Then
            TempSDT = TempRDT + " " + AddStartTime
            SDT = DateTime.Parse(TempSDT)
        Else
            SDT = DateTime.Parse(TempSDT)
        End If

        If AddEndDate = "MM/DD/" & Today.Year.ToString Or AddEndMN.SelectedItem.Text = "MM" Or AddEndDD.SelectedItem.Text = "DD" Then
            TempEDT = TempRDT + " " + AddEndTime
            EDT = DateTime.Parse(TempEDT)
        Else
            EDT = DateTime.Parse(TempEDT)
        End If


        e.Command.Parameters("@Shift").Value = ShiftDropDown.SelectedItem.Text
        e.Command.Parameters("@Department").Value = DepartmentDropDown.SelectedItem.Text
        e.Command.Parameters("@ReportDate").Value = RDT
        e.Command.Parameters("@Event").Value = AddEvent.Text
        e.Command.Parameters("@StartTime").Value = SDT
        e.Command.Parameters("@EndTime").Value = EDT
        e.Command.Parameters("@OP").Value = User.Identity.Name.ToString
    End Sub

    Sub CreateDTRecord()
        If Me.ErrorMessagePanel.Visible = True Then
            Me.ErrorMessagePanel.Visible = False
            Me.BottemPanel.Visible = True
        End If

        Dim AddEvent As TextBox

        Dim AddStartMN As DropDownList
        Dim AddStartDD As DropDownList
        Dim AddStartHH As DropDownList
        Dim AddStartMM As DropDownList
        Dim AddStartTZ As DropDownList

        Dim AddEndMN As DropDownList
        Dim AddEndDD As DropDownList
        Dim AddEndHH As DropDownList
        Dim AddEndMM As DropDownList
        Dim AddEndTZ As DropDownList

        Dim AddStartDate As String
        Dim AddStartTime As String
        Dim AddEndDate As String
        Dim AddEndTime As String


        AddEvent = CType(Me.AddEvent, TextBox)

        AddStartMN = CType(Me.DTStartMN, DropDownList)
        AddStartDD = CType(Me.DTStartDD, DropDownList)
        AddStartDate = AddStartMN.SelectedItem.Text & "/" & AddStartDD.SelectedItem.Text & "/" & Date.Today.Year.ToString
        AddStartHH = CType(Me.DTStartHH, DropDownList)
        AddStartMM = CType(Me.DTStartMM, DropDownList)
        AddStartTZ = CType(Me.DTStartTT, DropDownList)
        AddStartTime = AddStartHH.SelectedItem.Text & ":" & AddStartMM.SelectedItem.Text & ":00 " & AddStartTZ.SelectedItem.Text

        AddEndMN = CType(Me.DTEndMN, DropDownList)
        AddEndDD = CType(Me.DTEndDD, DropDownList)
        AddEndDate = AddEndMN.SelectedItem.Text & "/" & AddEndDD.SelectedItem.Text & "/" & Date.Today.Year.ToString
        AddEndHH = CType(Me.DTEndHH, DropDownList)
        AddEndMM = CType(Me.DTEndMM, DropDownList)
        AddEndTZ = CType(Me.DTEndTT, DropDownList)
        AddEndTime = AddEndHH.SelectedItem.Text & ":" & AddEndMM.SelectedItem.Text & ":00 " & AddEndTZ.SelectedItem.Text

        If AddStartMN.SelectedItem.Text = "02" Or AddStartMN.SelectedItem.Text = "04" Or AddStartMN.SelectedItem.Text = "06" Or AddStartMN.SelectedItem.Text = "09" Or AddStartMN.SelectedItem.Text = "11" Then
            If AddStartDD.SelectedItem.Text = "31" Then
                AddStartDate = AddStartMN.SelectedItem.Text & "/30/" & Date.Today.Year.ToString
            End If

            If AddStartMN.SelectedItem.Text = "02" And (AddStartDD.SelectedItem.Text = "29" Or AddStartDD.SelectedItem.Text = "30" Or AddStartDD.SelectedItem.Text = "31") Then
                If Date.Today.Year Mod 4 = 0 Then
                    AddStartDate = AddStartMN.SelectedItem.Text & "/29/" & Date.Today.Year.ToString
                Else
                    AddStartDate = AddStartMN.SelectedItem.Text & "/28/" & Date.Today.Year.ToString
                End If
            End If
        End If

        If AddEndMN.SelectedItem.Text = "02" Or AddEndMN.SelectedItem.Text = "04" Or AddEndMN.SelectedItem.Text = "06" Or AddEndMN.SelectedItem.Text = "09" Or AddEndMN.SelectedItem.Text = "11" Then
            If AddEndDD.SelectedItem.Text = "31" Then
                AddEndDate = AddEndMN.SelectedItem.Text & "/30/" & Date.Today.Year.ToString
            End If
            If AddEndMN.SelectedItem.Text = "02" And (AddEndDD.SelectedItem.Text = "29" Or AddEndDD.SelectedItem.Text = "30" Or AddEndDD.SelectedItem.Text = "31") Then
                If Date.Today.Year Mod 4 = 0 Then
                    AddEndDate = AddEndMN.SelectedItem.Text & "/29/" & Date.Today.Year.ToString
                Else
                    AddStartDate = AddEndMN.SelectedItem.Text & "/28/" & Date.Today.Year.ToString
                End If
            End If
        End If

        Dim TempSDT As String = AddStartDate + " " + AddStartTime
        Dim TempEDT As String = AddEndDate + " " + AddEndTime

        Dim SDT As DateTime
        Dim EDT As DateTime

        Dim Error0 As Boolean = True
        Dim ErrorT As Boolean = True
        Dim Warning0 As Boolean = True
        Dim Warning1 As Boolean = True

        If AddEvent.Text = "" Then
            Error0 = False
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error: EVENT DESCRIPTION can not be empty."
            Me.ErrorMessage.ForeColor = Drawing.Color.Red
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
        End If

        Dim RDT As Date
        Date.TryParse(Me.DateTextBox.SelectedDate, RDT)
        Dim TempRDT As String = RDT.Date.ToString
        Dim DatePartsArray() As String = TempRDT.Split(" ")
        TempRDT = DatePartsArray(0)

        If AddStartDate = "MM/DD/" & Today.Year.ToString Or AddStartMN.SelectedItem.Text = "MM" Or AddStartDD.SelectedItem.Text = "DD" Then
            TempSDT = TempRDT + " " + AddStartTime
            SDT = DateTime.Parse(TempSDT)

            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Warning: START DATE was set to " & RDT & ". No date was enter."
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            Warning0 = False
        Else
            SDT = DateTime.Parse(TempSDT)
        End If

        If AddEndDate = "MM/DD/" & Today.Year.ToString Or AddEndMN.SelectedItem.Text = "MM" Or AddEndDD.SelectedItem.Text = "DD" Then
            TempEDT = TempRDT + " " + AddEndTime
            EDT = DateTime.Parse(TempEDT)

            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Warning: END DATE was set to " & RDT & ". No date was enter."
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            Warning1 = False
        Else
            EDT = DateTime.Parse(TempEDT)
        End If

        If Warning0 = False And Warning1 = False Then
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Warning: START and END DATE were both set to " & RDT & ". No dates were enter."
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
        End If

        If Error0 = False And (Warning0 = False Or Warning1 = False) Then
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error/Warning: EVENT DESCRIPTION can not be empty / DATES would have been set to " & RDT & "."
            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
        End If

        If Error0 = False And (Warning0 = False Or Warning1 = False) Then
            If AddEvent.Text = "" And AddStartDate = "" And AddEndDate = "" Then
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.ForeColor = Drawing.Color.Black
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
                Me.ErrorMessage.Text = "Warning: Nothing was entered"
            End If


            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            If Warning0 = False And Warning1 = False Then
                Me.ErrorMessage.Text = "Error/Warning: EVENT DESCRIPTION can not be empty / No DATES found. Both would be set to " & RDT & "."
            Else
                If Error0 = False And Warning0 = False Then
                    Me.ErrorMessage.Text = "Error/Warning: EVENT DESCRIPTION can not be empty / No Date found. START DATE would have been " & RDT & "."
                Else
                    Me.ErrorMessage.Text = "Error/Warning: EVENT DESCRIPTION can not be empty / No Date found. END DATE would have been " & RDT & "."
                End If
            End If
        End If


        Dim TotalDailyHours As Decimal = DateDiff(DateInterval.Minute, SDT, EDT)
        TotalDailyHours /= 60

        If TotalDailyHours > 12.5 Then
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error: Your START and END TIMES can not give you over 12 and a half hours of work."
            Me.ErrorMessage.ForeColor = Drawing.Color.Red
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            ErrorT = False
        ElseIf TotalDailyHours = 0 Then
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error: Your START and END TIMES can not give you 0 hours of work."
            Me.ErrorMessage.ForeColor = Drawing.Color.Red
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            ErrorT = False
        ElseIf TotalDailyHours < 0 Then
            Me.ErrorMessagePanel.Visible = True
            Me.BottemPanel.Visible = False
            Me.ErrorMessage.Text = "Error: END DATE OR TIME has to be later than the START DATE OR TIME. Cannot have negative hours"
            Me.ErrorMessage.ForeColor = Drawing.Color.Red
            Me.ErrorMessage.Font.Size = 15
            Me.ErrorMessage.Font.Bold = True
            ErrorT = False
        End If

        If Error0 = True And ErrorT = True Then
            SqlDataSourceProLogDT.Insert()
        End If
    End Sub

    Protected Sub CreateNewDT_Click(sender As Object, e As EventArgs) Handles CreateNewDT.Click
        CreateDTRecord()
        AddBottonHidden()

        CType(Me.AddEvent, TextBox).Text = String.Empty
        CType(Me.DTStartMN, DropDownList).SelectedIndex = DateTextBox.SelectedDate.Month
        CType(Me.DTStartDD, DropDownList).SelectedIndex = DateTextBox.SelectedDate.Day
        CType(Me.DTStartHH, DropDownList).SelectedIndex = 0
        CType(Me.DTStartMM, DropDownList).SelectedIndex = 0
        CType(Me.DTStartTT, DropDownList).SelectedIndex = 0
        CType(Me.DTEndMN, DropDownList).SelectedIndex = DateTextBox.SelectedDate.Month
        CType(Me.DTEndDD, DropDownList).SelectedIndex = DateTextBox.SelectedDate.Day
        CType(Me.DTEndHH, DropDownList).SelectedIndex = 0
        CType(Me.DTEndMM, DropDownList).SelectedIndex = 0
        CType(Me.DTEndTT, DropDownList).SelectedIndex = 0
    End Sub

    Protected Sub CancelRecordButton_Click(sender As Object, e As EventArgs) Handles CancelRecordButton.Click
        CType(Me.QC, TextBox).Text = String.Empty
        CType(Me.QP, TextBox).Text = String.Empty
        CType(Me.LN, TextBox).Text = String.Empty
        CType(Me.LogStartMN, DropDownList).SelectedIndex = 0
        CType(Me.LogStartDD, DropDownList).SelectedIndex = 0
        CType(Me.LogStartHH, DropDownList).SelectedIndex = 0
        CType(Me.LogStartMM, DropDownList).SelectedIndex = 0
        CType(Me.LogStartTZ, DropDownList).SelectedIndex = 0
        CType(Me.LogEndMN, DropDownList).SelectedIndex = 0
        CType(Me.LogEndDD, DropDownList).SelectedIndex = 0
        CType(Me.LogEndHH, DropDownList).SelectedIndex = 0
        CType(Me.LogEndMM, DropDownList).SelectedIndex = 0
        CType(Me.LogEndTZ, DropDownList).SelectedIndex = 0
    End Sub
    Protected Sub CancelADT_Click(sender As Object, e As EventArgs) Handles CancelADT.Click
        CType(Me.AddEvent, TextBox).Text = String.Empty
        CType(Me.DTStartMN, DropDownList).SelectedIndex = 0
        CType(Me.DTStartDD, DropDownList).SelectedIndex = 0
        CType(Me.DTStartHH, DropDownList).SelectedIndex = 0
        CType(Me.DTStartMM, DropDownList).SelectedIndex = 0
        CType(Me.DTStartTT, DropDownList).SelectedIndex = 0
        CType(Me.DTEndMN, DropDownList).SelectedIndex = 0
        CType(Me.DTEndDD, DropDownList).SelectedIndex = 0
        CType(Me.DTEndHH, DropDownList).SelectedIndex = 0
        CType(Me.DTEndMM, DropDownList).SelectedIndex = 0
        CType(Me.DTEndTT, DropDownList).SelectedIndex = 0
    End Sub

    Sub ProLogTable_RowEditing(sender As Object, ee As GridViewEditEventArgs) Handles ProLogTableCurrent.RowEditing
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Op, ReportDate, Shift FROM T_ProLogLots WHERE [Key] = " & Me.ViewState("KeyETable") & ""

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim Yesterday As Date = Date.Today.AddDays(-1)
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If checker(0) = User.Identity.Name.ToString And (checker(1) = DateTextBox.SelectedDate And DateTextBox.SelectedDate = Date.Today) Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            ElseIf Roles.IsUserInRole("Office") Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            ElseIf checker(0) = User.Identity.Name.ToString And (checker(1) = DateTextBox.SelectedDate And DateTextBox.SelectedDate = Yesterday) And (checker(2) = "N1" Or checker(2) = "N2") Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While

            Else
                ee.Cancel = True
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Permission Denied: You may only EDIT your own record for Today."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
        End While
        connect.Close()
    End Sub

    Sub ProLogTable_RowDeleting(sender As Object, de As GridViewDeleteEventArgs) Handles ProLogTableCurrent.RowDeleting
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Op, ReportDate, Shift FROM T_ProLogLots WHERE [Key] = " & Me.ViewState("KeyDTable") & ""

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim Yesterday As Date = Date.Today.AddDays(-1)
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If checker(0) = User.Identity.Name.ToString And (checker(1) = DateTextBox.SelectedDate And DateTextBox.SelectedDate = Date.Today) Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            ElseIf checker(0) = User.Identity.Name.ToString And (checker(1) = DateTextBox.SelectedDate And DateTextBox.SelectedDate = Yesterday) And (checker(2) = "N1" Or checker(2) = "N2") Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            ElseIf Roles.IsUserInRole("Office") Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            Else
                de.Cancel = True
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Permission Denied: You may only DELETE your own record for Today."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
        End While
        connect.Close()

        If Me.ViewState("FieldsT") Then
            ProLogTableCurrent.DataBind()
            If ProLogTableCurrent.Rows.Count = 1 Then
                Me.AddNewRecord.Visible = True
            Else
                Me.ProLogTableCurrent.ShowFooter = True
            End If
        End If
    End Sub

    Sub ProLogDT_RowEditing(sender As Object, ee As GridViewEditEventArgs) Handles ProLogDTCurrent.RowEditing
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Op, ReportDate, Shift FROM T_ProLogDT WHERE [Key] = " & Me.ViewState("KeyEDT") & ""

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim Yesterday As Date = Date.Today.AddDays(-1)
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If checker(0) = User.Identity.Name.ToString And (checker(1) = DateTextBox.SelectedDate And DateTextBox.SelectedDate = Date.Today) Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            ElseIf checker(0) = User.Identity.Name.ToString And (checker(1) = DateTextBox.SelectedDate And DateTextBox.SelectedDate = Yesterday) And (checker(2) = "N1" Or checker(2) = "N2") Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            ElseIf Roles.IsUserInRole("Office") Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            Else
                ee.Cancel = True
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Permission Denied: You may only EDIT your own record for Today."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
        End While
        connect.Close()
    End Sub

    Sub ProLogDT_RowDeleting(sender As Object, de As GridViewDeleteEventArgs) Handles ProLogDTCurrent.RowDeleting
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Op, ReportDate, Shift FROM T_ProLogDT WHERE [Key] = " & Me.ViewState("KeyDDT") & ""

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim Yesterday As Date = Date.Today.AddDays(-1)
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If checker(0) = User.Identity.Name.ToString And (checker(1) = DateTextBox.SelectedDate And DateTextBox.SelectedDate = Date.Today) Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            ElseIf checker(0) = User.Identity.Name.ToString And (checker(1) = DateTextBox.SelectedDate And DateTextBox.SelectedDate = Yesterday) And (checker(2) = "N1" Or checker(2) = "N2") Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            ElseIf Roles.IsUserInRole("Office") Then
                Me.ErrorMessagePanel.Visible = False
                Me.BottemPanel.Visible = True
                Exit While
            Else
                de.Cancel = True
                Me.ErrorMessagePanel.Visible = True
                Me.BottemPanel.Visible = False
                Me.ErrorMessage.Text = "Permission Denied: You may only DELETE your own record for Today."
                Me.ErrorMessage.ForeColor = Drawing.Color.Red
                Me.ErrorMessage.Font.Size = 15
                Me.ErrorMessage.Font.Bold = True
            End If
        End While
        connect.Close()

        If Me.ViewState("FieldsDT") Then
            ProLogDTCurrent.DataBind()
            If ProLogDTCurrent.Rows.Count = 1 Then
                Me.AddNewDTRecord.Visible = True
            Else
                Me.ProLogDTCurrent.ShowFooter = True
            End If
        End If
    End Sub
End Class