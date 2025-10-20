Imports Owin
Imports Microsoft.AspNet.SignalR

Public Class SseStartup
    Public Sub Configuration(app As IAppBuilder)
        app.MapSignalR()
    End Sub
End Class