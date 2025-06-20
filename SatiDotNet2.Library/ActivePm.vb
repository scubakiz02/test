Imports System.Drawing
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class ActivePm
    Inherits Security

    Public Function GetOutOfRange(DataKey As String) As Dictionary(Of String, Object)
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@DataKey", GetParamVarHash(DataKey, "int")}
        }
        Dim OutOfRangeStringified As String = GetSingleDbField("SELECT OutOfRange FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@DataKey", SqlConfig, "OutOfRange")
        Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(OutOfRangeStringified)
    End Function

    Public Function SetOutOfRange(DataKey As Integer, NewOutOfRange As String) As Dictionary(Of String, Object)
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@DataKey", GetParamVarHash(DataKey, "int")},
            {"@OutOfRange", GetParamVarHash(NewOutOfRange, "varchar")}
        }
        Dim Res As New Dictionary(Of String, Object)

        Res = ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] SET OutOfRange=@OutOfRange WHERE [Key]=@DataKey", SqlConfig)
        Res.Remove("PrimaryKey")

        Return Res
    End Function
End Class
