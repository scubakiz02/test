Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class Part
    Inherits Security

    Private Function PartRequirementsMet(PartInfoConfig As Dictionary(Of String, String)) As Boolean
        'must include PartDescription & Quantity
        Try
            Integer.Parse(PartInfoConfig("Qty")) 'in case quantity does not exist, or is not a valid integer

            Return PartInfoConfig IsNot Nothing AndAlso
           PartInfoConfig.Count > 0 AndAlso
           String.IsNullOrEmpty(PartInfoConfig("PartDescription")) = False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function AddPart(FieldsConfig As Dictionary(Of String, String), Optional ExecuteSql As Boolean = False) As Dictionary(Of String, String)
        'call this function to create record in T_SMR_Parts
        'write logic for edgecases
        'invocate Security.ExecuteSqlParamQuery() to execute insert into sql query
        Dim Res As New Dictionary(Of String, String)
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim SqlQuery As String = "INSERT INTO [ALTS].[dbo].[T_SMR_PartToOrder] (SMR_Key, ManufacturerOrVendor, PartDescription, PW_PartNum, Vendor_PartNum, Qty, Procured) VALUES (@SMR_Key, @ManufacturerOrVendor, @PartDescription, @PW_PartNum, @Vendor_PartNum, @Qty, @Procured)"
        Dim SqlQueryFields As New List(Of String)

        If PartRequirementsMet(FieldsConfig) = False Then
            Res("Success") = False
            Res("Message") = "Error: Name & Quantity -- required"
            Return Res
        End If

        'if the value in the key value pairs within FieldsConfig is an empty string, place a dbnull there for sql insert into query
        For Each FieldConfig As KeyValuePair(Of String, String) In FieldsConfig
            Dim FieldKey As String = FieldConfig.Key
            Dim FieldValue As String = If(String.IsNullOrEmpty(FieldConfig.Value), Nothing, FieldConfig.Value) 'Nothing will be parsed as DBNull by ExecuteSqlParamQuery() function when it is called

            'required values for DB INSERT INTO query on table 'T_SMR_PartToOrder' are:
            '1) SMR_Key
            '2) Procured
            '3) Qty
            '4) Part Description
            '****expecting Procured and SMR_Key from client side to ALWAYS NOT be NULL
            'Why you may ask?
            'SMR_Key is grabbed from GridView
            'Procured value is derived from a dll option that is either yes or no
            QueryConfig("@" & FieldKey) = New Dictionary(Of String, String) From {
                {"value", FieldValue},
                {"typeOf", "string"}
            }
            SqlQueryFields.Add(FieldKey)
        Next

        Res("QueryConfig") = JsonSerializer.Serialize(QueryConfig)
        Res("SqlQuery") = SqlQuery

        If ExecuteSql Then ExecSqlWithSafeguards(Res, "Error: Add failed. Retry or contact Sati admin")

        Return Res
    End Function

    Private Sub BuildQueryConfigForIntValue(QueryConfig As Dictionary(Of String, Dictionary(Of String, String)), Key As String, Value As Integer)
        QueryConfig("@" & Key) = New Dictionary(Of String, String) From {
            {"value", Value},
            {"typeOf", "int"}
        }
    End Sub

    Private Sub ExecSqlWithSafeguards(ReturnDict As Dictionary(Of String, String), ErrorMessage As String)
        Dim SqlSuccess As Boolean = ExecuteSqlParamQuery(ReturnDict("SqlQuery"), JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ReturnDict("QueryConfig")))("Success")
        'Dim SqlSuccess As Boolean = False 'for client side regression tests regarding error handling 
        ReturnDict("Success") = SqlSuccess

        If SqlSuccess = False Then ReturnDict("Message") = ErrorMessage

        ReturnDict.Remove("QueryConfig")
        ReturnDict.Remove("SqlQuery")
    End Sub

End Class
