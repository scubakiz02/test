Imports System.Drawing
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class ChecklistBuilderAspxLibrary
    Dim Sql As New Security

    Public Function ModifyOrder(Key As Integer, Action As String, Marker As String) As Dictionary(Of String, String)
        Dim Res As New Dictionary(Of String, String)
        Dim ParameterizedValuesConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim Table As String = "[ALTS].[dbo]."
        Dim FieldOfInterest As String

        Select Case Marker
            Case "Label"
                Return ModifyLabelOrder(Key, Action, Marker)
            Case "Phase"
                Table += "[T_LogPhase]"
                FieldOfInterest = "PhaseOrder"
            Case Else
                Table += "[T_LogCommentList]"
                FieldOfInterest = "CommentOrder"
        End Select

        Dim UpdateQueryTemplate As String = "UPDATE " & Table & " SET " & FieldOfInterest & "="
        Dim SqlQuery As String = UpdateQueryTemplate & "@Order1 WHERE [Key]=@Key1; " & UpdateQueryTemplate & "@Order2 WHERE [Key]=@Key2"
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        QueryConfig("@Key") = New Dictionary(Of String, String) From {
            {"value", Key},
            {"typeOf", "int"}
        }
        Dim KeyDS As Data.DataSet = Sql.GetMyDataSetParamQuery("SELECT (SELECT TOP(1) [Key] FROM " & Table & " Frst WHERE Frst.AreaKey=Curr.AreaKey ORDER BY Frst." & FieldOfInterest & ") As FirstKey, (SELECT TOP(1) [Key] FROM " & Table & " Prev WHERE Prev.AreaKey=Curr.AreaKey AND Prev." & FieldOfInterest & " < Curr." & FieldOfInterest & " ORDER BY Prev." & FieldOfInterest & " DESC) As PrevKey, (SELECT TOP(1) [Key] FROM " & Table & " Nxt WHERE Nxt.AreaKey=Curr.AreaKey AND " & FieldOfInterest & " > Curr." & FieldOfInterest & " ORDER BY Nxt." & FieldOfInterest & ") As NextKey, (SELECT TOP(1) [Key] FROM " & Table & " Lst WHERE Lst.AreaKey=Curr.AreaKey ORDER BY Lst." & FieldOfInterest & " DESC) As LastKey FROM " & Table & " Curr WHERE [Key]=@Key GROUP BY " & FieldOfInterest & ", [Key], AreaKey", QueryConfig)
        Dim KeyDR As Data.DataRow = KeyDS.Tables(0).Rows(0)
        Dim FirstKey As Integer = KeyDR("FirstKey")
        Dim PrevKey As Integer = If(IsDBNull(KeyDR("PrevKey")), -1, KeyDR("PrevKey")) 'ternary operator in case value is DBNull
        Dim NextKey As Integer = If(IsDBNull(KeyDR("NextKey")), -1, KeyDR("NextKey")) 'ternary operator in case value is DBNull
        Dim LastKey As Integer = KeyDR("LastKey")
        Dim FirstOrder As Integer
        Dim PrevOrder As String 'string variable type in case PrevKey is -1, which would mean this variable could be null (Nothing)
        Dim Order As Integer
        Dim NextOrder As String 'string variable type in case NextKey is -1, which would mean this variable could be null (Nothing)
        Dim LastOrder As Integer

        QueryConfig("@Key")("value") = FirstKey
        FirstOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = PrevKey
        PrevOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = Key
        Order = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = NextKey
        NextOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = LastKey
        LastOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        If Action = "up" Then
            If PrevKey = -1 Then 'if true, this means the label is already the top/up most label
                SqlQuery = ""
            Else
                ParameterizedValuesConfig("@Order1") = New Dictionary(Of String, String) From {
                    {"value", PrevOrder},
                    {"typeOf", "int"}
                }
                ParameterizedValuesConfig("@Key2") = New Dictionary(Of String, String) From {
                    {"value", PrevKey},
                    {"typeOf", "int"}
                }
            End If
        Else
            If NextKey = -1 Then 'if true, this means the label is already the bottom/down most label
                SqlQuery = ""
            Else
                ParameterizedValuesConfig("@Order1") = New Dictionary(Of String, String) From {
                    {"value", NextOrder},
                    {"typeOf", "int"}
                }
                ParameterizedValuesConfig("@Key2") = New Dictionary(Of String, String) From {
                    {"value", NextKey},
                    {"typeOf", "int"}
                }
            End If
        End If

        If String.IsNullOrEmpty(SqlQuery) = False Then 'these 2 key value pairs always keep the same value, hence why they're not in the if statement above
            ParameterizedValuesConfig("@Key1") = New Dictionary(Of String, String) From {
                {"value", Key},
                {"typeOf", "int"}
            }
            ParameterizedValuesConfig("@Order2") = New Dictionary(Of String, String) From {
                {"value", Order},
                {"typeOf", "int"}
            }
        End If

        Res("ParameterizedValues") = JsonSerializer.Serialize(ParameterizedValuesConfig)
        Res("SqlQuery") = SqlQuery

        Return Res
    End Function

    Private Function ModifyLabelOrder(Key As Integer, Action As String, Marker As String) As Dictionary(Of String, String)
        Dim Res As New Dictionary(Of String, String)
        Dim ParameterizedValuesConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim Table As String = "[ALTS].[dbo].[T_LogLabel]"
        Dim FieldOfInterest As String = "LabelOrder"
        Dim UpdateQueryTemplate As String = "UPDATE " & Table & " SET " & FieldOfInterest & "="
        Dim SqlQuery As String = UpdateQueryTemplate & "@Order1 WHERE [Key]=@Key1; " & UpdateQueryTemplate & "@Order2 WHERE [Key]=@Key2"
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        QueryConfig("@Key") = New Dictionary(Of String, String) From {
            {"value", Key},
            {"typeOf", "int"}
        }
        Dim KeyDS As Data.DataSet = Sql.GetMyDataSetParamQuery("SELECT (SELECT TOP(1) [Key] FROM " & Table & " Frst WHERE Frst.AreaKey=Curr.AreaKey ORDER BY Frst." & FieldOfInterest & ") As FirstKey, (SELECT TOP(1) [Key] FROM " & Table & " Prev WHERE Prev.AreaKey=Curr.AreaKey AND Prev." & FieldOfInterest & " < Curr." & FieldOfInterest & " ORDER BY Prev." & FieldOfInterest & " DESC) As PrevKey, (SELECT TOP(1) [Key] FROM " & Table & " Nxt WHERE Nxt.AreaKey=Curr.AreaKey AND " & FieldOfInterest & " > Curr." & FieldOfInterest & " ORDER BY Nxt." & FieldOfInterest & ") As NextKey, (SELECT TOP(1) [Key] FROM " & Table & " Lst WHERE Lst.AreaKey=Curr.AreaKey ORDER BY Lst." & FieldOfInterest & " DESC) As LastKey FROM " & Table & " Curr WHERE [Key]=@Key GROUP BY " & FieldOfInterest & ", [Key], AreaKey", QueryConfig)
        Dim KeyDR As Data.DataRow = KeyDS.Tables(0).Rows(0)
        Dim FirstKey As Integer = KeyDR("FirstKey")
        Dim PrevKey As Integer = If(IsDBNull(KeyDR("PrevKey")), -1, KeyDR("PrevKey")) 'ternary operator in case value is DBNull
        Dim NextKey As Integer = If(IsDBNull(KeyDR("NextKey")), -1, KeyDR("NextKey")) 'ternary operator in case value is DBNull
        Dim LastKey As Integer = KeyDR("LastKey")
        Dim FirstOrder As Integer
        Dim PrevOrder As String 'string variable type in case PrevKey is -1, which would mean this variable could be null (Nothing)
        Dim Order As Integer
        Dim NextOrder As String 'string variable type in case NextKey is -1, which would mean this variable could be null (Nothing)
        Dim LastOrder As Integer

        QueryConfig("@Key")("value") = FirstKey
        FirstOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = PrevKey
        PrevOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = Key
        Order = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = NextKey
        NextOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = LastKey
        LastOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        If Action = "up" Then
            If PrevKey = -1 OrElse SamePhase(PrevKey, Key) = False Then 'if label is top most of LabelOrder OR PhaseOrder
                SqlQuery = ""
            Else
                ParameterizedValuesConfig("@Order1") = New Dictionary(Of String, String) From {
                    {"value", PrevOrder},
                    {"typeOf", "int"}
                }
                ParameterizedValuesConfig("@Key2") = New Dictionary(Of String, String) From {
                    {"value", PrevKey},
                    {"typeOf", "int"}
                }
            End If
        Else
            If NextKey = -1 OrElse SamePhase(NextKey, Key) = False Then 'if label is top most of LabelOrder OR PhaseOrder
                SqlQuery = ""
            Else
                ParameterizedValuesConfig("@Order1") = New Dictionary(Of String, String) From {
                    {"value", NextOrder},
                    {"typeOf", "int"}
                }
                ParameterizedValuesConfig("@Key2") = New Dictionary(Of String, String) From {
                    {"value", NextKey},
                    {"typeOf", "int"}
                }
            End If
        End If

        If String.IsNullOrEmpty(SqlQuery) = False Then 'these 2 key value pairs always keep the same value, hence why they're not in the if statement above
            ParameterizedValuesConfig("@Key1") = New Dictionary(Of String, String) From {
                {"value", Key},
                {"typeOf", "int"}
            }
            ParameterizedValuesConfig("@Order2") = New Dictionary(Of String, String) From {
                {"value", Order},
                {"typeOf", "int"}
            }
        End If

        Res("ParameterizedValues") = JsonSerializer.Serialize(ParameterizedValuesConfig)
        Res("SqlQuery") = SqlQuery

        Return Res
    End Function

    Private Function SamePhase(LabelKey1, LabelKey2) As Boolean
        Dim PhaseQuery As String = "SELECT PhaseKey FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey"
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From
        {
            {"@LabelKey", New Dictionary(Of String, String) From {
                {"value", String.Empty},
                {"typeOf", "int"}
            }}
        }
        Dim PhaseKey1 As Integer
        Dim PhaseKey2 As Integer

        QueryConfig("@LabelKey")("value") = LabelKey1
        PhaseKey1 = Sql.GetSingleDbField(PhaseQuery, QueryConfig, "PhaseKey")

        QueryConfig("@LabelKey")("value") = LabelKey2
        PhaseKey2 = Sql.GetSingleDbField(PhaseQuery, QueryConfig, "PhaseKey")

        If PhaseKey1 = PhaseKey2 Then
            Return True
        Else
            Return False
        End If
    End Function

    Function GetAreaDdlSelectConfig(AreaIntervalKey As String, DepartmentKey As String) As Dictionary(Of String, String) 'this query is used in several areas, but needs to use the current value in Session("AreaIntervalKey"). That is why it in a function
        Dim Res As New Dictionary(Of String, String)

        Res("AreaIntervalKey") = If(AreaIntervalKey Is Nothing OrElse AreaIntervalKey = "All", -1, AreaIntervalKey)
        Res("SelectQuery") = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND OneTimeDate IS NULL" & If(DepartmentKey Is Nothing, String.Empty, " AND DepartmentKey=@DepartmentKey") & " OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area"

        Return Res
    End Function
End Class
