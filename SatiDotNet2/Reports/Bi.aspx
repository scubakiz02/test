<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Bi.aspx.vb" Inherits="Reports_Bi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
</head>
<body>
   
        <iframe id="IframeDefalt" runat="server" style="position: fixed; top:0; left: 0; bottom: 0; right: 0; width: 100%; height: 100%; border: none; margin: 0; padding: 0; overflow: hidden; z-index: 999999;" src="https://app.powerbi.com/reportEmbed?reportId=9569abc8-beb2-49a4-8b94-b01e3adfdc25&autoAuth=true&ctid=8b577483-7da7-4a2b-bb62-ff7597b8369f&config=eyJjbHVzdGVyVXJsIjoiaHR0cHM6Ly93YWJpLXVzLXdlc3QyLXJlZGlyZWN0LmFuYWx5c2lzLndpbmRvd3MubmV0LyJ9"></iframe>       
    
   

    <script>
        window.setInterval(function () {
            reloadIFrame()
        }, 3000);

        function reloadIFrame() {
           document.getElementById('IframeDefalt').contentWindow.location.reload();
        }
    </script>
</body>
</html>
