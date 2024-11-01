<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SPC_View.aspx.vb" Inherits="SPC_SPC_View" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel0" runat="server">
                 <asp:Label ID="Label0" runat="server" Text="SPC View" Font-Bold="True" Font-Size="X-Large"></asp:Label><br />
                <br />
                Select a Tool to view history:<br />
                <asp:DropDownList ID="DropDownListTool" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Tool_Name" DataValueField="Key" AppendDataBoundItems="True">
                    <asp:ListItem>Select...</asp:ListItem>
                </asp:DropDownList>
                 <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SATI_SPCConnectionString %>" SelectCommand="SELECT [Key], Tool_Name FROM T_SPC_Tool_Info WHERE (Enable = 1) ORDER BY Tool_Name"></asp:SqlDataSource>
               
            </asp:Panel>

            <asp:Panel ID="Panel1" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label1" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart1A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart1B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />


            <asp:Panel ID="Panel2" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label2" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart2A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart2B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />

            
            <asp:Panel ID="Panel3" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label3" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart3A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart3B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />


            <asp:Panel ID="Panel4" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label4" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart4A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart4B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />


            <asp:Panel ID="Panel5" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label5" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart5A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart5B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />


            <asp:Panel ID="Panel6" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label6" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart6A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart6B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />


            <asp:Panel ID="Panel7" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label7" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart7A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart7B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />


            <asp:Panel ID="Panel8" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label8" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart8A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart8B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />


            <asp:Panel ID="Panel9" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label9" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart9A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart9B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />

             <asp:Panel ID="Panel10" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label10" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart10A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart10B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />

             <asp:Panel ID="Panel11" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label11" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart11A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart11B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />

             <asp:Panel ID="Panel12" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label12" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart12A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart12B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />

             <asp:Panel ID="Panel13" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label13" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart13A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart13B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />

             <asp:Panel ID="Panel14" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label14" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart14A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart14B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />

             <asp:Panel ID="Panel15" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label15" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart15A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                   
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart15B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />

            <asp:Panel ID="Panel16" runat="server" Visible="false" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                <div align="center">
                    <asp:Label ID="Label16" runat="server" Text="Name 1" Font-Size="XX-Large" Font-Bold="True" ForeColor="Black"></asp:Label>
                </div>
                
                <asp:Chart ID="Chart16A" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="AVG" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red" > </asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                   
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                <br />

                <asp:Chart ID="Chart16B" runat="server" Width="980px" BorderlineColor="Black" BorderlineDashStyle="Solid" BackColor="AliceBlue" >
                    <Titles> 
                        <asp:Title Text="StDev" Font="Microsoft Sans Serif, 14pt"></asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Docking="Top"  Alignment="Center" BackColor="AliceBlue"/>
                    </Legends>
                    <Series>
                        <asp:Series Name="LCL" ChartType="Line" Color="Red"></asp:Series>
                        <asp:Series Name="Value" ChartType="Line" Color="Black"></asp:Series>
                        <asp:Series Name="UCL" ChartType="Line" Color="Red"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="AliceBlue"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

            </asp:Panel><br />




        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
