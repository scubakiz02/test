Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WS_test
     Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function SayHello(ByVal Name As String) As String
        Return "Hello : " + Name
    End Function

End Class
