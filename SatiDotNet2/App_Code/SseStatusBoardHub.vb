Imports Microsoft.AspNet.SignalR
Imports System.Threading
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class SseStatusBoardHub
    Inherits Hub

    Private Shared _KeepAliveTimer As Timer
    Private Shared _TimerStarted As Boolean = False
    Private Shared _LockObj As New Object()
    Private Shared _ActivePm As New ActivePm()

    Public Shared Sub StartPing()
        If _TimerStarted = False Then
            SyncLock _LockObj
                If _TimerStarted = False Then
                    _KeepAliveTimer = New Timer(AddressOf SendPing, Nothing, 0, 30000)
                    _TimerStarted = True
                End If
            End SyncLock
        End If
    End Sub

    Private Shared Sub SendPing(state As Object)
        Dim Context = GlobalHost.ConnectionManager.GetHubContext(Of SseStatusBoardHub)()
        Dim Now As DateTime = DateTime.Now

        'If Now.Minute Mod 2 = 0 Then 'for troubleshooting, debugging
        If Now.Minute = 0 AndAlso Now.Second < 30 Then
            Context.Clients.All.statusBoardPing("refresh", Nothing)
        Else
            Context.Clients.All.statusBoardPing("ping", Now.ToString("MM/dd/yyyy HH:mm:ss tt"))
        End If
    End Sub

    Public Shared Sub StatusBoardChange(DataKey As Integer, Optional StatusBoardDateAt As String = Nothing)
        If StatusBoardDateAt Is Nothing Then StatusBoardDateAt = System.DateTime.Now.ToString("MM/dd/yyyy")
        Dim ChangeConfig As New Dictionary(Of Integer, Object) From {
            {DataKey, _ActivePm.GetLogConfig(DataKey, StatusBoardDateAt)}
        }

        Dim Context = GlobalHost.ConnectionManager.GetHubContext(Of SseStatusBoardHub)()
        Context.Clients.All.statusBoardPing("change", JsonSerializer.Serialize(ChangeConfig))
    End Sub
End Class