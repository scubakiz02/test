Imports System.Runtime.Caching
Imports System.Reflection

Public Class ActivePmCache
    Inherits ActivePm

    Private Sub CacheWrite(CacheKey As String, Data As Object)
        Dim Cache = MemoryCache.Default
        Dim Policy As New CacheItemPolicy() With {
            .AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1) ' "LogStateChanges" cache expires after 1 minute
        }

        Cache.Set(CacheKey, Data, Policy)
    End Sub

    Public Sub CacheAdd(DataKey As Integer, Optional StatusBoardDateAt As String = Nothing)
        Dim CachedDataAsObj As Object = CacheRead("LogStateChanges")
        Dim CachedDataAsDict As Dictionary(Of Integer, Object) = DirectCast(CachedDataAsObj, Dictionary(Of Integer, Object))

        If CachedDataAsDict Is Nothing Then
            CachedDataAsDict = New Dictionary(Of Integer, Object)
        End If

        If StatusBoardDateAt Is Nothing Then StatusBoardDateAt = System.DateTime.Now.ToString("MM/dd/yyyy")
        CachedDataAsDict(DataKey) = GetLogConfig(DataKey, StatusBoardDateAt)

        CacheWrite("LogStateChanges", CachedDataAsDict)
    End Sub

    Public Sub CacheDelete(DataKey As Integer)
        Dim CachedDataAsObj As Object = CacheRead("LogStateChanges")
        Dim CachedDataAsDict As Dictionary(Of Integer, Object) = DirectCast(CachedDataAsObj, Dictionary(Of Integer, Object))

        If CachedDataAsDict Is Nothing Then
            CachedDataAsDict = New Dictionary(Of Integer, Object)
        End If

        CachedDataAsDict.Remove(DataKey)

        CacheWrite("LogStateChanges", CachedDataAsDict)
    End Sub

    Public Function CacheRead(CacheKey As String) As Object
        Return MemoryCache.Default.Get(CacheKey)
    End Function
End Class
