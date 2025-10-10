Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Globalization
Imports System.Text.Json
Imports System.Configuration

Public Class Security
    Private connectionString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString

    Public Function NoSqlInjection(str As String) As Boolean
        'SELECT -extracts data from a database       
        'UPDATE -updates data In a database
        'DELETE -deletes data from a database
        'INSERT INTO - inserts New data into a database
        'CREATE DATABASE - creates a New database
        'ALTER DATABASE - modifies a database
        'CREATE TABLE - creates a New table
        'ALTER TABLE - modifies a table
        'DROP TABLE - deletes a table
        'CREATE INDEX - creates an index (search key)
        'DROP INDEX - deletes an index

        Dim Res As Boolean = True
        Dim SqlCommands As String() = {"SELECT", "UPDATE", "DELETE", "INSERT", "CREATE", "ALTER", "DROP"}
        Dim strLowerCased As String = LCase(str)

        For Each SqlCommand In SqlCommands
            If strLowerCased.Contains(LCase(SqlCommand)) Then
                Return False
            End If
        Next

        Return Res
    End Function

    Function GetSingleDbField(SqlQuery As String, QueryConfig As Dictionary(Of String, Dictionary(Of String, String)), Field As String) As String
        Dim Res As String

        'using try catch block in case 'There is no row at position 0.', which means there are no associated record in Table
        Try
            Res = GetMyDataSetParamQuery(SqlQuery, QueryConfig).Tables(0).Rows(0)(Field)
            Res = If(IsDBNull(Res), Nothing, Res) 'using ternary operator as a workaround to Null DB field values, which in that case the function will return Nothing
        Catch ex As Exception
            Res = Nothing
        End Try

        Return Res
    End Function

    Public Function ReturnTrue() As Boolean
        Return True
    End Function

    Private Function GetSqlDbType(typeName As String) As SqlDbType
        Select Case typeName.ToLower()
            Case "int", "integer"
                Return SqlDbType.Int
            Case "string", "varchar"
                Return SqlDbType.VarChar
            Case "decimal"
                Return SqlDbType.Decimal
            Case "float"
                Return SqlDbType.Float
            Case "bit", "boolean"
                Return SqlDbType.Bit
            Case "date", "datetime", "smalldatetime"
                Return SqlDbType.DateTime
            Case "unique", "identifier", "uniqueidentifier"
                Return SqlDbType.UniqueIdentifier
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function GetParamVarHash(Value As Object, DbType As String) As Dictionary(Of String, String)
        Dim Res As New Dictionary(Of String, String)

        Res("value") = Value
        Res("typeOf") = DbType

        Return Res
    End Function

    'using parameterized queries with select sql statement to prevent SQL injection and improve security
    Function GetMyDataSetParamQuery(SqlQuery As String, QueryConfig As Dictionary(Of String, Dictionary(Of String, String))) As Data.DataSet
        Dim ds As New DataSet()

        Try
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(SqlQuery, conn)

                    For Each kvp As KeyValuePair(Of String, Dictionary(Of String, String)) In QueryConfig
                        Dim paramValue As String = kvp.Key
                        Dim paramConfig As Dictionary(Of String, String) = kvp.Value

                        cmd.Parameters.Add(paramValue, GetSqlDbType(paramConfig("typeOf"))).Value = paramConfig("value")
                    Next

                    Using adapter As New SqlDataAdapter(cmd)
                        conn.Open()
                        adapter.Fill(ds)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try

        Return ds
    End Function

    'using parameterized queries to execute non returning sql statements (insert into, update, delete, etc.) to prevent SQL injection and improve security
    Function ExecuteSqlParamQuery(SqlQuery As String, QueryConfig As Dictionary(Of String, Dictionary(Of String, String))) As Dictionary(Of String, Object)
        Dim Res As New Dictionary(Of String, Object)

        Try
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(SqlQuery, conn)
                    Dim PrimaryKey As String

                    ' Add parameters
                    For Each kvp As KeyValuePair(Of String, Dictionary(Of String, String)) In QueryConfig
                        Dim paramValue As String = kvp.Key
                        Dim paramConfig As Dictionary(Of String, String) = kvp.Value
                        Dim DbValue As String = paramConfig("value")

                        cmd.Parameters.Add(paramValue, GetSqlDbType(paramConfig("typeOf"))).Value = If(DbValue Is Nothing, DBNull.Value, DbValue)
                    Next

                    conn.Open()

                    ' ExecuteScalar returns the first column of the first row in the result set,
                    ' which is our newly generated identity value.
                    PrimaryKey = cmd.ExecuteScalar()
                    Res("PrimaryKey") = If(PrimaryKey IsNot Nothing, Convert.ToInt32(PrimaryKey), 0)
                End Using
            End Using
            Res("Success") = True
        Catch ex As Exception
            Res("Success") = False
        End Try

        Return Res
    End Function

    Function StripIllegalFileSysChars(ChecklistFolder As String, DatePeriodFolder As String) As String
        Return Path.Combine(Regex.Replace(ChecklistFolder, "[:#'""/\\]", ""), Regex.Replace(DatePeriodFolder, "[/\\]", "-"))
    End Function

    Public Function GetStatusBoardRole(View As String, Department As String, Where As Date) As String()
        Dim Res As New List(Of String)

        If Where <> Today.Date Then
            Res.Add("admin")
        ElseIf View = "Focus" AndAlso Department = "Production" Then 'if view is focus & department is production, return should be nothing
            Res.Add(Nothing)
        ElseIf View = "Full" Then 'if user wnats to see past issues column, they will need the associated supervisor role
            If Department <> "Production" Then
                Res.Add("FMManagerApproval")
                Res.Add("QSHEManagerApproval")
            Else
                Res.Add("PC")
            End If
        Else 'user will need to at minimum have 'Maintenance' role to view 'All' or 'Maintenance' department logs
            If Department <> "Production" Then
                Res.Add("Maintenance")
            End If
        End If

        Return Res.ToArray()
    End Function
End Class

Public Class SqlParameters
    Public Function ValidParameterizedValues(CreateArg As Dictionary(Of String, String), CreateFuncRes As Dictionary(Of String, String)) As Boolean
        'this function ensures:
        '1) parameterized values exists in sql query;
        '2) content in CreateFuncRes("QueryConfig") is valid for arg 2 within Security.GetMyDataSetParamQuery
        Dim QueryConfigDeserialized As Dictionary(Of String, Dictionary(Of String, String))
        Dim ParameterizedKeys As List(Of String)
        Dim Valid As Boolean = True

        Try
            QueryConfigDeserialized = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(CreateFuncRes("QueryConfig"))
            ParameterizedKeys = CreateArg.Keys.ToList()

            For Each ParameterizedKey As String In ParameterizedKeys
                'intializing variables with 'Object' variable type, in case they are DBNull values
                Dim ValueFromCreateFunc As String = QueryConfigDeserialized("@" & ParameterizedKey)("value")
                Dim ValueFromCreateArg As String = CreateArg(ParameterizedKey)

                'there is an exception where a mismatch in the 2 strings listed in the condition CAN be a valid parameterized value:
                'an empty value from a textbox is an empty string. Empty value in DB is Null. This represented by ValueFromCreateArg being an empty string, and ValueFromCreateFunc is nothing
                'that is the only exception that is valid
                If ValueFromCreateArg = String.Empty Then
                    If ValueFromCreateFunc IsNot Nothing Then
                        Valid = False
                        Exit For
                    End If
                ElseIf ValueFromCreateFunc <> ValueFromCreateArg Then
                    Valid = False
                    Exit For
                End If
            Next
        Catch ex As Exception
            Valid = False
        End Try

        Return Valid
    End Function
End Class

Public Class SatiUser
    Inherits Security

    Private ReadOnly UserName As String
    Private ReadOnly UserDS As Data.DataSet
    Private ReadOnly DepartmentDS As Data.DataSet
    Private QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Private Roles As New List(Of String)
    Private ReadOnly DepartmentsHashmap As New Dictionary(Of String, String) From {
        {"All", Nothing}
    }

    Public Sub New(ByVal User As String)
        Dim UserRC As Integer
        Dim DepartmentRC As Integer
        Dim UserDR As Data.DataRow
        Dim DepartmentDR As Data.DataRow

        UserName = LCase(User)

        QueryConfig("@username") = New Dictionary(Of String, String) From {
            {"value", UserName},
            {"typeOf", "string"}
        }
        'MyBase keyword refers to parent class 'Security', which this class inherits
        UserDS = MyBase.GetMyDataSetParamQuery("SELECT RoleName FROM [SatiUsers].[dbo].[aspnet_UsersInRoles] RoleList INNER JOIN [SatiUsers].[dbo].[aspnet_Users] Users On RoleList.UserId = Users.UserId INNER JOIN [SatiUsers].[dbo].[aspnet_Roles] RoleInfo On RoleList.RoleId = RoleInfo.RoleId WHERE Users.LoweredUserName=@username ORDER BY RoleName", QueryConfig)
        UserRC = UserDS.Tables(0).Rows.Count - 1
        For I As Integer = 0 To UserRC
            UserDR = UserDS.Tables(0).Rows(I)
            Roles.Add(UserDR("RoleName"))
        Next

        QueryConfig.Clear()
        'MyBase keyword refers to parent class 'Security', which this class inherits
        DepartmentDS = MyBase.GetMyDataSetParamQuery("SELECT [Key], Department FROM [ALTS].[dbo].[T_LogDepartment]", QueryConfig)
        DepartmentRC = DepartmentDS.Tables(0).Rows.Count - 1
        For I As Integer = 0 To DepartmentRC
            DepartmentDR = DepartmentDS.Tables(0).Rows(I)
            DepartmentsHashmap(DepartmentDR("Department")) = DepartmentDR("Key")
        Next

    End Sub

    Public Function GetDepartment() As String
        Dim Res As String

        If Roles.Contains("admin") Then
            Res = "All"
        ElseIf Roles.Contains("FMManagerApproval") OrElse Roles.Contains("QSHEManagerApproval") OrElse Roles.Contains("PmChecklistBuild") Then
            Res = "Maintenance"
        ElseIf Roles.Contains("PC") Then
            Res = "Production"
        Else
            Res = Nothing
        End If

        Return Res
    End Function

    Public Function GetDepartmentKey() As String
        Dim Department As String = GetDepartment()

        If Department Is Nothing Then
            Return Nothing
        Else
            Return DepartmentsHashmap(Department)
        End If

    End Function
End Class

Public Class AspWebpage
    Private ReadOnly WebpageUrl As String
    Private QsConfig As New Dictionary(Of String, String)
    Private UrlWithQs As String

    Sub New(Url As String, QsKeys As List(Of String))
        WebpageUrl = Url
        UrlWithQs = Url

        For Each QsKey As String In QsKeys
            QsConfig(QsKey) = Nothing
        Next
    End Sub

    Public Sub SetUrl(Key As String, Value As String)
        Dim QsPresent As Boolean = False
        QsConfig(Key) = Value
        UrlWithQs = WebpageUrl 'reset

        For Each kvp As KeyValuePair(Of String, String) In QsConfig
            Dim QsKey As String = kvp.Key
            Dim QsValue As String = kvp.Value
            Dim UrlSplit As String() = UrlWithQs.Split("?")
            Dim QueryStrings As New List(Of String)

            If UrlSplit.Count > 1 Then 'if querystrings exist
                QueryStrings.AddRange(UrlSplit(1).Split("&"))
            End If

            If QsValue IsNot Nothing AndAlso QueryStrings.Contains(QsKey & "=" & QsValue) = False Then 'qs value is not null AND qs key does NOT exist in the url
                If QsPresent Then
                    UrlWithQs += "&"
                Else
                    QsPresent = True
                    UrlWithQs += "?"
                End If

                UrlWithQs += QsKey & "=" & QsValue
            End If
        Next
    End Sub

    Public Function GetUrl() As String
        Return UrlWithQs
    End Function

End Class

Public Class Format
    Public Sub New()

    End Sub

    Public Function DateField(InputDate As String) As String
        Dim ParsedDate As DateTime

        Try
            ParsedDate = DateTime.Parse(InputDate)
            Return ParsedDate.ToString("MM/dd/yyyy hh:mm:ss tt")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function DateNoTime(InputDate As String) As String
        Dim ParsedDate As DateTime

        Try
            ParsedDate = DateTime.Parse(InputDate)
            Return ParsedDate.ToString("MM/dd/yyyy")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ValidLogDate(InputDate As String) As Boolean
        If String.IsNullOrEmpty(Trim(InputDate)) Then Return False 'null or empty string edgecases

        Try
            Date.Parse(InputDate) 'invalid timestamp (date, hour, minute, seconds, etc.)
            If DateField(InputDate) <> InputDate Then Return False 'ensure mm/dd/yyyy hh:mm:ss tt is the ONLY accepted format
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

End Class

