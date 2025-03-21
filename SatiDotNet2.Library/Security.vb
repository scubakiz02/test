Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.IO

Public Class Security
    Private connectionString As String = "Data Source=PWI-31\SATIDB;Initial Catalog=ALTS;Persist Security Info=True;User ID=exsil_user;Password=exsiluser"

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

    Function GetSqlDbType(typeName As String) As SqlDbType
        Select Case typeName.ToLower()
            Case "int", "integer"
                Return SqlDbType.Int
            Case "string"
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
    Function ExecuteSqlParamQuery(SqlQuery As String, QueryConfig As Dictionary(Of String, Dictionary(Of String, String))) As Boolean
        Dim Success As Boolean = True

        Try
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(SqlQuery, conn)
                    ' Add parameters
                    For Each kvp As KeyValuePair(Of String, Dictionary(Of String, String)) In QueryConfig
                        Dim paramValue As String = kvp.Key
                        Dim paramConfig As Dictionary(Of String, String) = kvp.Value

                        cmd.Parameters.Add(paramValue, GetSqlDbType(paramConfig("typeOf"))).Value = paramConfig("value")
                    Next

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Success = False
        End Try

        Return Success
    End Function

    Function StripIllegalFileSysChars(ChecklistFolder As String, DatePeriodFolder As String) As String
        Return Path.Combine(Regex.Replace(ChecklistFolder, "[:#'""/\\]", ""), Regex.Replace(DatePeriodFolder, "[/\\]", "-"))
    End Function
End Class
