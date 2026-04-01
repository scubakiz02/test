Imports System.Diagnostics
Imports System.Web.UI.DataVisualization.Charting
Imports Microsoft.VisualBasic


Public Class ItemTracker
    ''' <summary>
    ''' This method is designed to take in a location Name as well as a Column Number (Starting at 1).
    ''' With these it will construct a SELECT statement. This statement will return the data at that 
    ''' column that has that location name.
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="Column"></param>
    ''' <returns> ReturnLocationParts </returns>
    Public Function GetLocationPartsData(Name As String, Column As Integer) As Object
        Dim ReturnLocationParts As Object = ""

        Dim SQLColumn As String = "[key]"
        If Column = 2 Then
            SQLColumn = "Section"
        ElseIf Column = 3 Then
            SQLColumn = "LocationName"
        ElseIf Column = 4 Then
            SQLColumn = "Char"
        ElseIf Column = 5 Then
            SQLColumn = "Notes"
        ElseIf Column = 6 Then
            SQLColumn = "TimeStamp"
        End If

        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT " & SQLColumn & " FROM T_SATI_Item_Location_Parts WHERE (LocationName = '" & Name & "')"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }

        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If Column = 1 Then
                ReturnLocationParts += checker(0).ToString
            Else
                ReturnLocationParts += checker(0)
                ReturnLocationParts += " "
            End If
        End While

        connect.Close()
        Return ReturnLocationParts
    End Function

    ''' <summary>
    ''' This method takes in the Location Section Number, the Name of the location, he Location Symbol or Char,
    ''' and the notes for the location. This will be used to make a INSERT statement to add a new location Part
    ''' </summary>
    ''' <param name="Sec"></param>
    ''' <param name="Name"></param>
    ''' <param name="symbol"></param>
    ''' <param name="Notes"></param>
    Public Sub AddLocationPart(Sec As Integer, Name As String, symbol As String, Notes As String)
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "INSERT INTO T_SATI_Item_Location_Parts (Section, LocationName, Char, Notes) VALUES ('" & Sec & "', '" & Name & "', '" & symbol & "', '" & Notes & "')"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }
        command.ExecuteNonQuery()
        connect.Close()
    End Sub





    ''' <summary>
    ''' This function will take in a ItemNumber as a Integer and take in a column number as an integer (Starting at 1).
    ''' These will be used to make a SELECT statement to find any of the corresponding ItemName from the Database.
    ''' This will then be returned as a string.
    ''' </summary>
    ''' <param name="ItemNumber"></param>
    ''' <param name="Column"></param>
    ''' <returns> ReturnItemData </returns>
    Public Function GetItemData(ItemNumber As Integer, Column As Integer) As Object
        Dim ReturnItemData As Object = ""

        Dim SQLColumn As String = "[key]"
        If Column = 2 Then
            SQLColumn = "ItemNumber"
        ElseIf Column = 3 Then
            SQLColumn = "ItemName"
        ElseIf Column = 4 Then
            SQLColumn = "LocationMobile"
        ElseIf Column = 5 Then
            SQLColumn = "Notes"
        ElseIf Column = 6 Then
            SQLColumn = "TimeStamp"
        End If

        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT " & SQLColumn & " FROM T_SATI_Item_Names WHERE (ItemNumber = " & ItemNumber & ")"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }

        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If Column = 1 Or Column = 4 Then
                ReturnItemData += checker(0).ToString
            Else
                ReturnItemData += checker(0)
                ReturnItemData += " "
            End If
        End While

        connect.Close()
        Return ReturnItemData
    End Function


    ''' <summary>
    ''' This method will take in a Item Number as a Integer, an Item Name as a String, Boolean for the Mobile Location 
    ''' identifier, and a String for the notes of the items. This will be then used to make a INSERT statement and add 
    ''' a new record to the database.
    ''' </summary>
    ''' <param name="ItemNumber"></param>
    ''' <param name="ItemName"></param>
    ''' <param name="MobileLocation"></param>
    ''' <param name="Notes"></param>
    Public Sub AddItemName(ItemNumber As Integer, ItemName As String, MobileLocation As Boolean, Notes As String)
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "INSERT INTO T_SATI_Item_Names (ItemNumber, ItemName, LocationMobile, Notes) VALUES ('" & ItemNumber & "', '" & ItemName & "', '" & MobileLocation & "', '" & Notes & "')"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }
        command.ExecuteNonQuery()
        connect.Close()
    End Sub





    ''' <summary>
    ''' This method takes in a Location number and a Column number (Starting at 1), 
    ''' which are used to makes a SELECT statement to find and return any Tracked 
    ''' Item Data that corresponds to the Item that is located at that location.
    ''' </summary>
    ''' <param name="Location"></param>
    ''' <param name="Column"></param>
    ''' <returns> ReturnItemTrackingData </returns>
    Public Function GetItemTrackingData(Location As String, Column As Integer) As Object
        Dim ReturnItemTrackingData As Object = ""

        Dim SQLColumn As String = "[key]"
        If Column = 2 Then
            SQLColumn = "Location"
        ElseIf Column = 3 Then
            SQLColumn = "ItemNumber"
        ElseIf Column = 4 Then
            SQLColumn = "ItemType"
        ElseIf Column = 5 Then
            SQLColumn = "Notes"
        ElseIf Column = 6 Then
            SQLColumn = "Active"
        ElseIf Column = 7 Then
            SQLColumn = "TimeStamp"
        End If

        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT " & SQLColumn & " FROM T_SATI_Item_Tracking WHERE (Location = '" & Location & "')"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }

        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If Column = 1 Or Column = 6 Then
                ReturnItemTrackingData += checker(0).ToString
            Else
                ReturnItemTrackingData += checker(0)
                ReturnItemTrackingData += " "
            End If
        End While

        connect.Close()
        Return ReturnItemTrackingData
    End Function


    ''' <summary>
    ''' This method takes in a Location as a String, Item Number as a String, a Item Type to see if it is a item (I) or 
    ''' a wafer Lot (w), a String for Notes and a boolean toggle to show it that location is currently active with an
    ''' item at it. This will be used to make a INSERT statment to add a new record to the database.
    ''' </summary>
    ''' <param name="Location"></param>
    ''' <param name="ItemNumber"></param>
    ''' <param name="ItemType"></param>
    ''' <param name="Notes"></param>
    ''' <param name="Active"></param>
    Public Sub AddItemTracking(Location As String, ItemNumber As String, ItemType As String, Notes As String, Active As Boolean)
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "INSERT INTO T_SATI_Item_Tracking (Location, ItemNumber, ItemType, Notes, Active) VALUES ('" & Location & "', '" & ItemNumber & "', '" & ItemType & "', '" & Notes & "', '" & Active & "')"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }
        command.ExecuteNonQuery()
        connect.Close()
    End Sub


    ''' <summary>
    ''' This method takes in a Location and a boolean toggle for the location being 
    ''' active and makes an UPDARE statement to set that location to active or inactive
    ''' when it is needed.
    ''' </summary>
    ''' <param name="Location"></param>
    ''' <param name="Active"></param>
    Public Sub SetActiveTracking(Location As String, Active As Boolean)
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "UPDATE T_SATI_Item_Tracking SET Active = '" & Active & "' WHERE Location = '" & Location & "'"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }
        command.ExecuteNonQuery()
        connect.Close()
    End Sub


    ''' <summary>
    ''' This method is designed to take in a location and clear and delete all the items
    ''' or item within that Location. Then it will return an all clear message to show the 
    ''' user that it is cleared.
    ''' </summary>
    ''' <param name="Location"></param>
    Public Sub ClearLocationsTrackedItems(Location As String)
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "UPDATE T_SATI_Item_Tracking SET Active = '0' WHERE Location = '" & Location & "'"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }
        command.ExecuteNonQuery()
        connect.Close()
    End Sub




    ''' <summary>
    ''' This method takes in a Location as a String, a FullName as a String, and Alisa or AKA as a string,
    ''' a boolean for a active location, and a Stirng for Notes. This will allow a INSERT Statement to 
    ''' add a new record to the Database.
    ''' </summary>
    ''' <param name="Location"></param>
    ''' <param name="FullName"></param>
    ''' <param name="AKA"></param>
    ''' <param name="Active"></param>
    ''' <param name="Notes"></param>
    Public Sub AddNewLocation(Location As String, FullName As String, AKA As String, Active As Boolean, Notes As String)
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "INSERT INTO T_SATI_Item_Location_Names (Location, FullName, Alias, Active, Notes) VALUES ('" & Location & "', '" & FullName & "', '" & AKA & "', '" & Active & "', '" & Notes & "')"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }
        command.ExecuteNonQuery()
        connect.Close()
    End Sub


    ''' <summary>
    ''' This method takes in a location as a String and a boolean for the location 
    ''' being active. This will then be used to make an UPDATE Statement to turn that active 
    ''' boolean on or off for that location.
    ''' </summary>
    ''' <param name="Location"></param>
    ''' <param name="Active"></param>
    Public Sub EnableLocation(Location As String, Active As Boolean)
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "UPDATE T_SATI_Item_Location_Names SET Active = '" & Active & "' WHERE Location = '" & Location & "'"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }
        command.ExecuteNonQuery()
        connect.Close()
    End Sub


    ''' <summary>
    ''' This method takes a Location as a String and a column as a Integer (Starting at 1)
    ''' These will be used to make a SELECT statement to return the any of the location's 
    ''' data that matches that Location.
    ''' </summary>
    ''' <param name="Location"></param>
    ''' <param name="Column"></param>
    ''' <returns> ReturnLocationData </returns>
    Public Function GetLocationData(Location As String, Column As Integer) As Object
        Dim ReturnLocationData As Object = ""

        Dim SQLColumn As String = "[key]"
        If Column = 2 Then
            SQLColumn = "Location"
        ElseIf Column = 3 Then
            SQLColumn = "FullName"
        ElseIf Column = 4 Then
            SQLColumn = "Alias"
        ElseIf Column = 5 Then
            SQLColumn = "Active"
        ElseIf Column = 6 Then
            SQLColumn = "Notes"
        ElseIf Column = 7 Then
            SQLColumn = "TimeStamp"
        End If

        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT " & SQLColumn & " FROM T_SATI_Item_Location_Names WHERE (Location = '" & Location & "')"
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection With {
            .ConnectionString = conString
        }
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand With {
            .CommandText = camString,
            .Connection = connect
        }

        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If Column = 1 Or Column = 5 Then
                ReturnLocationData += checker(0).ToString
            Else
                ReturnLocationData += checker(0)
                ReturnLocationData += " "
            End If
        End While

        connect.Close()
        Return ReturnLocationData
    End Function
End Class