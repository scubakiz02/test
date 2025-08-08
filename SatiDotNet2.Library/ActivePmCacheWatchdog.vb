Imports System.Runtime.Caching
Imports System.Threading

Public Class ActivePmCacheWatchdog
    Private ReadOnly _Cache As MemoryCache = MemoryCache.Default
    Private ReadOnly _Key As String
    Private ReadOnly _PollInterval As TimeSpan
    Private _LastValue As Object
    Private _Timer As Timer

    Public Sub New(CacheKey As String, Optional PollIntervalSeconds As Integer = 5)
        _Key = CacheKey
        _PollInterval = TimeSpan.FromSeconds(PollIntervalSeconds)
        _LastValue = _Cache.Get(_Key)

        Start()
    End Sub

    Public Sub Start()
        _Timer = New Timer(AddressOf CheckCache, Nothing, TimeSpan.Zero, _PollInterval)
    End Sub

    Public Sub [Stop]()
        _Timer?.Dispose()
    End Sub

    Private Sub CheckCache(State As Object)
        Dim CurrentValue = _Cache.Get(_Key)
        If Not Object.Equals(CurrentValue, _LastValue) Then
            _LastValue = CurrentValue
            OnCacheChanged(CurrentValue)
        End If
    End Sub

    ' Event or callback when cache changes
    Public Event CacheChanged(NewValue As Object)

    Protected Overridable Sub OnCacheChanged(NewValue As Object)
        RaiseEvent CacheChanged(NewValue)
    End Sub
End Class
