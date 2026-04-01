<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Text.Json
Imports SatiDotNet2.Library

Public Class StreamData
    Implements IHttpHandler, IReadOnlySessionState

    Private _Format As New Format()
    Private _PmInput As New PmInput()

    Public Sub ProcessRequest(Context As HttpContext) Implements IHttpHandler.ProcessRequest
        Context.Response.ContentType = "application/json"

        ' GET data for tabulator instance as json
        Dim HttpRequestVars As Object = Context.Request.QueryString
        Dim GroupKey As String = HttpRequestVars("groupkey")
        Dim PmKeys As Object = ParseHttpArr(HttpRequestVars("pmKeys"))
        Dim InputKeys As Object = ParseHttpArr(HttpRequestVars("inputKeys"))
        Dim StartDateAt As String = _Format.DateNoTime(HttpRequestVars("startDateAt"))
        Dim EndDateAt As String = _Format.DateNoTime(HttpRequestVars("endDateAt"))

        Dim TabulatorConfig As OrderedDictionary = Context.Session("Report").GetTabulatorConfig()
        Context.Response.Write(JsonSerializer.Serialize(TabulatorConfig))
        'Context.Response.Write(GetTabulatorConfigFake()) 'for troubleshooting/debugging
    End Sub

    Private Function GetTabulatorConfigFake() As String
        Dim Data As New Dictionary(Of String, List(Of Dictionary(Of String, Object))) From {
            {
                "pm/checklist 1",
                New List(Of Dictionary(Of String, Object)) From {
                    New Dictionary(Of String, Object) From {{"checklist", "pm/checklist 1"}, {"input", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"}, {"datakey", 2051}, {"labelkey", 446}, {"fieldtype", "Checkbox"}, {"value", 8.33}, {"startDateAt", "10/01/2025"}, {"inputDateAt", "10/01/2025 06:42:57 AM"}, {"operator", "Andrew Williams"}},
                    New Dictionary(Of String, Object) From {{"checklist", "pm/checklist 1"}, {"input", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"}, {"datakey", 2051}, {"labelkey", 447}, {"fieldtype", "Checkbox"}, {"value", 7.0}, {"startDateAt", "10/02/2025"}, {"inputDateAt", "10/02/2025 12:37:39 PM"}, {"operator", "Mark Kiser"}}
                }
            },
            {
                "pm/checklist 2",
                New List(Of Dictionary(Of String, Object)) From {
                    New Dictionary(Of String, Object) From {{"checklist", "pm/checklist 2"}, {"input", "Inlet Prefilter Pressure | >45 psi"}, {"datakey", 2033}, {"labelkey", 556}, {"fieldtype", "Checkbox"}, {"value", 52}, {"startDateAt", "11/01/2025"}, {"inputDateAt", "11/01/2025 08:12:00 AM"}, {"operator", "Jane Smith"}},
                    New Dictionary(Of String, Object) From {{"checklist", "pm/checklist 2"}, {"input", "Outlet Prefilter Pressure | >40 psi"}, {"datakey", 2033}, {"labelkey", 557}, {"fieldtype", "Checkbox"}, {"value", 41}, {"startDateAt", "11/02/2025"}, {"inputDateAt", "11/02/2025 09:15:00 AM"}, {"operator", "Jane Smith"}}
                }
            },
            {
                "pm/checklist 3",
                New List(Of Dictionary(Of String, Object)) From {
                    New Dictionary(Of String, Object) From {{"checklist", "pm/checklist 3"}, {"input", "Acid Level"}, {"datakey", 4051}, {"labelkey", 556}, {"fieldtype", "Checkbox"}, {"value", 1}, {"startDateAt", "12/01/2025"}, {"inputDateAt", "12/01/2025 10:00:00 AM"}, {"operator", "Sam Lee"}},
                    New Dictionary(Of String, Object) From {{"checklist", "pm/checklist 3"}, {"input", "Caustic Level"}, {"datakey", 4106}, {"labelkey", 557}, {"fieldtype", "Checkbox"}, {"value", 0}, {"startDateAt", "12/02/2025"}, {"inputDateAt", "12/02/2025 11:00:00 AM"}, {"operator", "Sam Lee"}}
                }
            }
        }

        Return JsonSerializer.Serialize(Data)
    End Function

    Private Function ParseHttpArr(HttpArrStringified As String) As Object
        Dim Res As Object

        Try
            Res = JsonSerializer.Deserialize(Of List(Of Integer))(HttpArrStringified)
        Catch ex As Exception
            Res = Nothing
        End Try

        Return Res
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class