<%@ Application Language="VB" %>

<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup

        Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12

    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        Session.Add("Lot", "")
        Session.Add("ID", "")
        Session.Add("Run", "")
        Session.Add("WL", "")
        Session.Add("Stage", "")
        Session.Add("PathName", "")
        Session.Add("View", "Yes")
        Session.Add("Step", "")
        Session.Add("MovementRecord", "")
        Session.Add("Qty", "0")
        Session.Add("PreStep", "0")
        Session.Add("SAR_IDs", "0")
        Session.Add("SAR_ID_Count", "0")
        Session.Add("Customer", "0")
        Session.Add("SpecialPartial", "No")
        'Session.Add("MRTicket", "")
        'Session.Add("MRTicketOpen", "")
        Session.Add("InvType", "Normal")
        Session.Add("EmailAddress", "")
        Session.Add("Export", "")

        'Redirects
        Session.Add("CheckPoint", "~/PC/CheckPoint.aspx")
        Session.Add("RunSheet", "~/Production/SelectedLotStageToWork.aspx")
        Session.Add("MakeLotPart2", "~/PC/MakeLotPart2.aspx")
        Session.Add("MakeLotPart1", "~/PC/MakeFirstPassLot.aspx")
        Session.Add("BackTo", "Home.aspx")
        Session.Add("SRN", "~/PC/SRN.aspx")

        'Servers & Directorys        
        Session.Add("EmailServerIP", "PWI-2010R")
        Session.Add("EmailServerPort", "25") '52

        'Session.Add("SatiMapsDir", "\\PWI-40\sp2maps$")
        Session.Add("SatiMapsDir", "http://57.201.101.139:81/Maps")
        'Session.Add("SP2Files", "\\PWI-40\sp2files$")
        Session.Add("SP2Files", "\\57.201.101.139\sp2files$")
        Session.Add("SDS_View", "http://57.201.101.139:81/SDS/")
        Session.Add("SDS", "\\57.201.101.139\sds$\")
        Session.Add("WI_View", "http://57.201.101.139:81/WI/")
        Session.Add("WI", "\\57.201.101.139\WI$\")
        Session.Add("ReportFolder", "http://57.201.101.139:81/TempImageWebFiles/")
        Session.Add("CustomerData", "http://57.201.101.139:81/CustomerData/")
        Session.Add("SUP_VD", "http://57.201.101.139:81/SUP/")
        Session.Add("SUP_IO", "\\57.201.101.139\SATI_Upload_Pics$\")

        'Session("CustomerData")

        'Session.Add("ReportDir", "\\PWI-40\reports$") Cant use. all referances are url hyperlinks
        'Session.Add("DocShare", "\\PWI-40\docshare") Cant use. all referances are url hyperlinks

        'Printers
        Session.Add("Printer1", "Label1")
        Session.Add("Printer2", "Label2")
        Session.Add("Printer3", "Label3")
        Session.Add("Printer4", "Label4")
        Session.Add("Printer5", "Label5")
        Session.Add("Printer6", "Label6")
        Session.Add("Printer7", "Label7")


        'pm/checklist webpages
        Session.Add("StartDateCutoffAt", "08/15/2025")
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

</script>
