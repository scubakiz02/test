Public Class Security
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

    Public Function ReturnTrue() As Boolean
        Return True
    End Function

End Class
