Imports Microsoft.VisualBasic

Public Class SatiUtility

    ''' <summary>
    ''' This method will be used to disable a button on a client side page with the use of Javascript.
    ''' The method takes in your page (Me.Page) and your button's Client ID (Button.ClientID) as 
    ''' the paramater.
    ''' </summary>
    ''' <param name="ThisPage"></param>
    ''' <param name="ButtonName"></param>
    Public Shared Sub DisableButton(ThisPage As Page, ButtonName As String)
        ScriptManager.RegisterClientScriptBlock(ThisPage, GetType(Type), "Disable" + ButtonName, "satiUtilityDisableButtonClick(" + ButtonName + ")", True)
    End Sub

    ''' <summary>
    ''' This method will be used to enable a button on a client side page with the use of Javascript.
    ''' The method takes in your page (Me.Page) and your button's Client ID (Button.ClientID) as 
    ''' the paramater. Call this method to reenable the button. 
    ''' </summary>
    ''' <param name="ThisPage"></param>
    ''' <param name="ButtonName"></param>
    Public Shared Sub EnableButton(ThisPage As Page, ButtonName As String)
        ScriptManager.RegisterClientScriptBlock(ThisPage, GetType(Type), "Enable" + ButtonName, "satiUtilityEnableButtonClick(" + ButtonName + ")", True)
    End Sub

    ''' <summary>
    ''' This method will be used to convert a Hexadecimal into Ascii code. This method will 
    ''' take in a string of Hexadecimal numbers/letters and will convert each pair in the 
    ''' string into the equivalent Ascii code. 
    ''' <br></br><br></br> Note: If the inputted string contains an odd number of characters a 
    ''' zero will be filled at the end. Furthermore, All spaces, commas, and/or hyphens contained in the 
    ''' string will be removed. For instance, spaces between Hexadecimal. (e.g. (41 41 52 4f 4e)
    ''' -> (4141524f4e)) 
    ''' </summary>
    ''' <param name="input"></param>
    ''' <returns></returns>
    Public Shared Function GetAsciiFromHex(input As String) As String
        input = input.Replace(" ", "")
        input = input.Replace(",", "")
        input = input.Replace("-", "")

        If input.Length Mod 2 <> 0 Then
            input += "0"
        End If

        Dim aStr As String = ""
        Try
            For x = 0 To input.Length - 1 Step 2
                Dim sStr As String = input.Substring(x, 2)
                aStr &= System.Convert.ToChar(System.Convert.ToUInt32(sStr, 16)).ToString()
            Next
        Catch ex As Exception
            aStr = " -- CONVERSION FAILED: EXCEPTION CAUGHT -- "
        End Try

        Return aStr
    End Function
End Class
