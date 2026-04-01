Imports Owin

Public Class SseStartup
    Public Sub Configuration(app As IAppBuilder)
        app.MapSignalR()

        'the method below is static. It only needs to be called once to start status board server side event pinging to all connected clients
        SseStatusBoardHub.StartPing()
    End Sub
End Class