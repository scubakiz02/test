Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Generates Certificate of Analysis (CofA) data for 300mm silicon wafers.
''' Handles metal contamination analysis (18 elements), wafer geometry measurements
''' (thickness, TTV, warp, bow, TIR), and particle/defect data from the T7 process line.
''' Metal results are cached in the GFAAS Data table to avoid redundant computation.
''' </summary>
Public Class CofA300mm
    Inherits Security

    Private EmptyQueryConfig As New Dictionary(Of String, Dictionary(Of String, String))

    ''' <summary>
    ''' Generates a random integer between lowerCap (inclusive) and UpperCap (exclusive),
    ''' returned as a Double. Used to apply small random variation to metal concentration values.
    ''' </summary>
    Private Function GetRandomNumber(ByVal UpperCap As Integer, ByVal lowerCap As Integer) As Double
        Dim RanNumber As New Random
        Dim Num As Integer
        Num = RanNumber.Next(lowerCap, UpperCap)
        GetRandomNumber = Num
    End Function

    ''' <summary>
    ''' Queries the 18 metal concentration values (Ca, Ma, Ni, Zn, Al, Fe, Cr, Cu, Na, K,
    ''' Co, Mn, Mo, W, Ti, V, Au, Ag) for a given wafer box instance from the
    ''' fctn_SatiCofA300MetalsSample database function.
    ''' </summary>
    Function Get300Metals(ByVal InstanceNumber As String) As Data.DataSet
        Dim qc As New Dictionary(Of String, Dictionary(Of String, String))
        qc("@InstanceNumber") = GetParamVarHash(InstanceNumber, "varchar")
        Return GetMyDataSetParamQuery("SELECT Ca, Ma, Ni, Zn, Al, Fe, Cr, Cu, Na, K, Co, Mn, Mo, W, Ti, V, Au, Ag FROM dbo.fctn_SatiCofA300MetalsSample(@InstanceNumber) AS fctn_SatiCofA300MetalsSample_1", qc)
    End Function

    ''' <summary>
    ''' Retrieves metal analysis data for a 300mm wafer box instance, using a cache-first strategy.
    ''' First looks up the lot number from T_FGI_Boxes/LabelsMade, then checks if metals data
    ''' already exists in the GFAAS Data table (with Source='300mm' or 'SATI').
    ''' If cached data is found (more than 1 row), returns it directly.
    ''' Otherwise, fetches raw metals via Get300Metals and persists them via WriteMetals.
    ''' </summary>
    Function GetCarton300mmMetals(ByVal InstanceNumber As String) As Data.DataSet
        Dim LotNumber As String = ""
        Dim DS_LotNumber As New Data.DataSet
        Dim qc As New Dictionary(Of String, Dictionary(Of String, String))
        qc("@InstanceKey") = GetParamVarHash(InstanceNumber, "int")
        DS_LotNumber = GetMyDataSetParamQuery("SELECT dbo.T_FGI_Boxes.InstanceKey, dbo.LabelsMade.Lot FROM dbo.LabelsMade INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey WHERE (dbo.T_FGI_Boxes.InstanceKey = @InstanceKey)", qc)
        If DS_LotNumber.Tables(0).Rows.Count > 0 Then
            Dim DR_lotNumber As Data.DataRow
            DR_lotNumber = DS_LotNumber.Tables(0).Rows(0)
            LotNumber = DR_lotNumber("Lot")
            Dim DS_MetalsTable As New Data.DataSet
            Dim qc2 As New Dictionary(Of String, Dictionary(Of String, String))
            qc2("@LotNumber") = GetParamVarHash(LotNumber, "varchar")
            DS_MetalsTable = GetMyDataSetParamQuery("SELECT [Date/Time], Source, [Test Type], Location, Idenyification, Notes, NotesExtra, Ca, Ma, Ni, Zn, Al, Fe, Cr, Cu, Na, K, Co, Mn, Mo, W, Ti, V, Au, Ag FROM dbo.[GFAAS Data] WHERE (Source = N'300mm' OR Source = N'SATI') AND (Idenyification = @LotNumber) AND (Notes IS NULL)", qc2)
            If DS_MetalsTable.Tables(0).Rows.Count > 1 Then
                Return DS_MetalsTable
            End If
        End If
        Dim DS_Get300mmMetalsData As New Data.DataSet
        DS_Get300mmMetalsData = Get300Metals(InstanceNumber)
        Return WriteMetals(DS_Get300mmMetalsData, LotNumber)
    End Function

    ''' <summary>
    ''' Writes 2 rows of metal concentration data to the GFAAS Data table for a given lot.
    ''' Each of the 18 metal values is taken from the source DataSet (DS_Metals) and a small
    ''' random variation (+/- 0.001 to 0.009) is applied to simulate measurement variance.
    ''' Values at or below 0.01 (or DBNull) are floored to the minimum of 0.01.
    ''' Rows are stamped with Source="SATI", Test Type="at/cm²", Location="Prescott".
    ''' After inserting, re-queries and returns the persisted rows.
    ''' </summary>
    ''' <remarks>
    ''' NOTE: This function exists in 2 places: here (SatiDotNet2.Library/CofA300mm.vb) and in
    ''' SatiDotNet2/App_Code/Class1.vb. The goal is to eventually remove the copy in Class1.vb
    ''' and have only this Library version exist, once the 200mm CofA path is also migrated.
    ''' </remarks>
    Private Function WriteMetals(DS_Metals As Data.DataSet, LotNumber As String) As Data.DataSet

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim DR_Metals As Data.DataRow


        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT [Record Number], [Date/Time], Source, [Test Type], Idenyification, Location, Ca, Ma, Ni, Zn, Al, Fe, Cr, Cu, Na, K, Co, Mn, Mo, W, Ti, Ag, Au, V FROM [GFAAS Data] WHERE ([Record Number] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = MySelectCmd

        Dim MyInsertCmd As New System.Data.SqlClient.SqlCommand
        With MyInsertCmd
            .CommandText = "INSERT INTO [GFAAS Data] ([Date/Time], [Source], [Test Type], [Idenyification], [Location], [Ca], [Ma], [Ni], [Zn], [Al], [Fe], [Cr], [Cu], [Na], [K], [Co], [Mn], [Mo], [W], [Ti], [Ag], [Au], [V]) VALUES (@p1, @Source, @Test_Type, @Idenyification, @Location, @Ca, @Ma, @Ni, @Zn, @Al, @Fe, @Cr, @Cu, @Na, @K, @Co, @Mn, @Mo, @W, @Ti, @Ag, @Au, @V); SELECT [Record Number], [Date/Time], Source, [Test Type], Idenyification, Location, Ca, Ma, Ni, Zn, Al, Fe, Cr, Cu, Na, K, Co, Mn, Mo, W, Ti, Ag, Au, V FROM [GFAAS Data] WHERE ([Record Number] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@p1", System.Data.SqlDbType.SmallDateTime, 0, "Date/Time"), New System.Data.SqlClient.SqlParameter("@Source", System.Data.SqlDbType.NVarChar, 0, "Source"), New System.Data.SqlClient.SqlParameter("@Test_Type", System.Data.SqlDbType.NVarChar, 0, "Test Type"), New System.Data.SqlClient.SqlParameter("@Idenyification", System.Data.SqlDbType.NVarChar, 0, "Idenyification"), New System.Data.SqlClient.SqlParameter("@Location", System.Data.SqlDbType.NVarChar, 0, "Location"), New System.Data.SqlClient.SqlParameter("@Ca", System.Data.SqlDbType.Float, 0, "Ca"), New System.Data.SqlClient.SqlParameter("@Ma", System.Data.SqlDbType.Float, 0, "Ma"), New System.Data.SqlClient.SqlParameter("@Ni", System.Data.SqlDbType.Float, 0, "Ni"), New System.Data.SqlClient.SqlParameter("@Zn", System.Data.SqlDbType.Float, 0, "Zn"), New System.Data.SqlClient.SqlParameter("@Al", System.Data.SqlDbType.Float, 0, "Al"), New System.Data.SqlClient.SqlParameter("@Fe", System.Data.SqlDbType.Float, 0, "Fe"), New System.Data.SqlClient.SqlParameter("@Cr", System.Data.SqlDbType.Float, 0, "Cr"), New System.Data.SqlClient.SqlParameter("@Cu", System.Data.SqlDbType.Float, 0, "Cu"), New System.Data.SqlClient.SqlParameter("@Na", System.Data.SqlDbType.Float, 0, "Na"), New System.Data.SqlClient.SqlParameter("@K", System.Data.SqlDbType.Float, 0, "K"), New System.Data.SqlClient.SqlParameter("@Co", System.Data.SqlDbType.Float, 0, "Co"), New System.Data.SqlClient.SqlParameter("@Mn", System.Data.SqlDbType.Float, 0, "Mn"), New System.Data.SqlClient.SqlParameter("@Mo", System.Data.SqlDbType.Float, 0, "Mo"), New System.Data.SqlClient.SqlParameter("@W", System.Data.SqlDbType.Float, 0, "W"), New System.Data.SqlClient.SqlParameter("@Ti", System.Data.SqlDbType.Float, 0, "Ti"), New System.Data.SqlClient.SqlParameter("@Ag", System.Data.SqlDbType.Float, 0, "Ag"), New System.Data.SqlClient.SqlParameter("@Au", System.Data.SqlDbType.Float, 0, "Au"), New System.Data.SqlClient.SqlParameter("@V", System.Data.SqlDbType.Float, 0, "V")})
        End With
        DA.InsertCommand = MyInsertCmd

        Dim MyUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MyUpdateCmd
            .CommandText = "UPDATE [GFAAS Data] SET [Date/Time] = @p1, [Source] = @Source, [Test Type] = @Test_Type, [Idenyification] = @Idenyification, [Location] = @Location, [Ca] = @Ca, [Ma] = @Ma, [Ni] = @Ni, [Zn] = @Zn, [Al] = @Al, [Fe] = @Fe, [Cr] = @Cr, [Cu] = @Cu, [Na] = @Na, [K] = @K, [Co] = @Co, [Mn] = @Mn, [Mo] = @Mo, [W] = @W, [Ti] = @Ti, [Ag] = @Ag, [Au] = @Au, [V] = @V WHERE (([Record Number] = @Original_Record_Number) AND ((@p3 = 1 AND [Date/Time] IS NULL) OR ([Date/Time] = @p2)) AND ((@IsNull_Source = 1 AND [Source] IS NULL) OR ([Source] = @Original_Source)) AND ((@IsNull_Test_Type = 1 AND [Test Type] IS NULL) OR ([Test Type] = @Original_Test_Type)) AND ((@IsNull_Idenyification = 1 AND [Idenyification] IS NULL) OR ([Idenyification] = @Original_Idenyification)) AND ((@IsNull_Location = 1 AND [Location] IS NULL) OR ([Location] = @Original_Location)) AND ((@IsNull_Ca = 1 AND [Ca] IS NULL) OR ([Ca] = @Original_Ca)) AND ((@IsNull_Ma = 1 AND [Ma] IS NULL) OR ([Ma] = @Original_Ma)) AND ((@IsNull_Ni = 1 AND [Ni] IS NULL) OR ([Ni] = @Original_Ni)) AND ((@IsNull_Zn = 1 AND [Zn] IS NULL) OR ([Zn] = @Original_Zn)) AND ((@IsNull_Al = 1 AND [Al] IS NULL) OR ([Al] = @Original_Al)) AND ((@IsNull_Fe = 1 AND [Fe] IS NULL) OR ([Fe] = @Original_Fe)) AND ((@IsNull_Cr = 1 AND [Cr] IS NULL) OR ([Cr] = @Original_Cr)) AND ((@IsNull_Cu = 1 AND [Cu] IS NULL) OR ([Cu] = @Original_Cu)) AND ((@IsNull_Na = 1 AND [Na] IS NULL) OR ([Na] = @Original_Na)) AND ((@IsNull_K = 1 AND [K] IS NULL) OR ([K] = @Original_K)) AND ((@IsNull_Co = 1 AND [Co] IS NULL) OR ([Co] = @Original_Co)) AND ((@IsNull_Mn = 1 AND [Mn] IS NULL) OR ([Mn] = @Original_Mn)) AND ((@IsNull_Mo = 1 AND [Mo] IS NULL) OR ([Mo] = @Original_Mo)) AND ((@IsNull_W = 1 AND [W] IS NULL) OR ([W] = @Original_W)) AND ((@IsNull_Ti = 1 AND [Ti] IS NULL) OR ([Ti] = @Original_Ti)) AND ((@IsNull_Ag = 1 AND [Ag] IS NULL) OR ([Ag] = @Original_Ag)) AND ((@IsNull_Au = 1 AND [Au] IS NULL) OR ([Au] = @Original_Au)) AND ((@IsNull_V = 1 AND [V] IS NULL) OR ([V] = @Original_V))); SELECT [Record Number], [Date/Time], Source, [Test Type], Idenyification, Location, Ca, Ma, Ni, Zn, Al, Fe, Cr, Cu, Na, K, Co, Mn, Mo, W, Ti, Ag, Au, V FROM [GFAAS Data] WHERE ([Record Number] = @Record_Number)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@p1", System.Data.SqlDbType.SmallDateTime, 0, "Date/Time"), New System.Data.SqlClient.SqlParameter("@Source", System.Data.SqlDbType.NVarChar, 0, "Source"), New System.Data.SqlClient.SqlParameter("@Test_Type", System.Data.SqlDbType.NVarChar, 0, "Test Type"), New System.Data.SqlClient.SqlParameter("@Idenyification", System.Data.SqlDbType.NVarChar, 0, "Idenyification"), New System.Data.SqlClient.SqlParameter("@Location", System.Data.SqlDbType.NVarChar, 0, "Location"), New System.Data.SqlClient.SqlParameter("@Ca", System.Data.SqlDbType.Float, 0, "Ca"), New System.Data.SqlClient.SqlParameter("@Ma", System.Data.SqlDbType.Float, 0, "Ma"), New System.Data.SqlClient.SqlParameter("@Ni", System.Data.SqlDbType.Float, 0, "Ni"), New System.Data.SqlClient.SqlParameter("@Zn", System.Data.SqlDbType.Float, 0, "Zn"), New System.Data.SqlClient.SqlParameter("@Al", System.Data.SqlDbType.Float, 0, "Al"), New System.Data.SqlClient.SqlParameter("@Fe", System.Data.SqlDbType.Float, 0, "Fe"), New System.Data.SqlClient.SqlParameter("@Cr", System.Data.SqlDbType.Float, 0, "Cr"), New System.Data.SqlClient.SqlParameter("@Cu", System.Data.SqlDbType.Float, 0, "Cu"), New System.Data.SqlClient.SqlParameter("@Na", System.Data.SqlDbType.Float, 0, "Na"), New System.Data.SqlClient.SqlParameter("@K", System.Data.SqlDbType.Float, 0, "K"), New System.Data.SqlClient.SqlParameter("@Co", System.Data.SqlDbType.Float, 0, "Co"), New System.Data.SqlClient.SqlParameter("@Mn", System.Data.SqlDbType.Float, 0, "Mn"), New System.Data.SqlClient.SqlParameter("@Mo", System.Data.SqlDbType.Float, 0, "Mo"), New System.Data.SqlClient.SqlParameter("@W", System.Data.SqlDbType.Float, 0, "W"), New System.Data.SqlClient.SqlParameter("@Ti", System.Data.SqlDbType.Float, 0, "Ti"), New System.Data.SqlClient.SqlParameter("@Ag", System.Data.SqlDbType.Float, 0, "Ag"), New System.Data.SqlClient.SqlParameter("@Au", System.Data.SqlDbType.Float, 0, "Au"), New System.Data.SqlClient.SqlParameter("@V", System.Data.SqlDbType.Float, 0, "V"), New System.Data.SqlClient.SqlParameter("@Original_Record_Number", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Record Number", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@p3", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Date/Time", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@p2", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Date/Time", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Source", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Source", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Source", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Source", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Test_Type", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Test Type", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Test_Type", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Test Type", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Idenyification", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Idenyification", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Idenyification", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Idenyification", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Location", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Location", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Location", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Location", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Ca", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Ca", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Ca", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Ca", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Ma", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Ma", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Ma", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Ma", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Ni", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Ni", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Ni", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Ni", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Zn", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Zn", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Zn", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Zn", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Al", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Al", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Al", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Al", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Fe", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Fe", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Fe", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Fe", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Cr", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Cr", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Cr", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Cr", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Cu", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Cu", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Cu", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Cu", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Na", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Na", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Na", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Na", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_K", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "K", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_K", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "K", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Co", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Co", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Co", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Co", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Mn", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Mn", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Mn", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Mn", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Mo", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Mo", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Mo", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Mo", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_W", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "W", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_W", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "W", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Ti", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Ti", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Ti", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Ti", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Ag", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Ag", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Ag", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Ag", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Au", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Au", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Au", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Au", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_V", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "V", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_V", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "V", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Record_Number", System.Data.SqlDbType.Int, 4, "Record Number")})
        End With
        DA.UpdateCommand = MyUpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "GFAAS Data", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Record Number", "Record Number"), New System.Data.Common.DataColumnMapping("Date/Time", "Date/Time"), New System.Data.Common.DataColumnMapping("Source", "Source"), New System.Data.Common.DataColumnMapping("Test Type", "Test Type"), New System.Data.Common.DataColumnMapping("Idenyification", "Idenyification"), New System.Data.Common.DataColumnMapping("Location", "Location"), New System.Data.Common.DataColumnMapping("Ca", "Ca"), New System.Data.Common.DataColumnMapping("Ma", "Ma"), New System.Data.Common.DataColumnMapping("Ni", "Ni"), New System.Data.Common.DataColumnMapping("Zn", "Zn"), New System.Data.Common.DataColumnMapping("Al", "Al"), New System.Data.Common.DataColumnMapping("Fe", "Fe"), New System.Data.Common.DataColumnMapping("Cr", "Cr"), New System.Data.Common.DataColumnMapping("Cu", "Cu"), New System.Data.Common.DataColumnMapping("Na", "Na"), New System.Data.Common.DataColumnMapping("K", "K"), New System.Data.Common.DataColumnMapping("Co", "Co"), New System.Data.Common.DataColumnMapping("Mn", "Mn"), New System.Data.Common.DataColumnMapping("Mo", "Mo"), New System.Data.Common.DataColumnMapping("W", "W"), New System.Data.Common.DataColumnMapping("Ti", "Ti"), New System.Data.Common.DataColumnMapping("Ag", "Ag"), New System.Data.Common.DataColumnMapping("Au", "Au"), New System.Data.Common.DataColumnMapping("V", "V")})})
        DA.Fill(DS)

        Dim R As Double = 0
        Dim M As Int16 = 0
        Dim MyNull As Object
        MyNull = System.DBNull.Value

        ' Write 2 rows (one per source sample row) with metadata and per-metal variation.
        ' For each metal: if the source value is non-null and > 0.01, apply a random offset
        ' of +/- 0.001..0.009 (decided by a coin flip). Otherwise, floor the value to 0.01.
        ' Later metals (Co through V) are wrapped in Try/Catch as a safeguard against
        ' missing columns in older data.
        For i As Int16 = 0 To 1
            DR_Metals = DS_Metals.Tables(0).Rows(i)
            DR = DS.Tables("GFAAS Data").NewRow
            DR("Date/Time") = DateTime.Now.ToShortDateString
            DR("Source") = "SATI"
            DR("Test Type") = "at/cm²"
            DR("Idenyification") = LotNumber
            DR("Location") = "Prescott"

            'Ca ************************************
            If Not DR_Metals("Ca") Is MyNull Then
                If DR_Metals("Ca") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Ca") = DR_Metals("Ca") + R
                    Else
                        DR("Ca") = DR_Metals("Ca") - R
                    End If
                Else
                    DR("Ca") = 0.01
                End If
            Else
                DR("Ca") = 0.01
            End If

            'Ma ************************************
            If Not DR_Metals("Ma") Is MyNull Then
                If DR_Metals("Ma") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Ma") = DR_Metals("Ma") + R
                    Else
                        DR("Ma") = DR_Metals("Ma") - R
                    End If
                Else
                    DR("Ma") = 0.01
                End If
            Else
                DR("Ma") = 0.01
            End If

            'Ni ************************************
            If Not DR_Metals("Ni") Is MyNull Then
                If DR_Metals("Ni") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Ni") = DR_Metals("Ni") + R
                    Else
                        DR("Ni") = DR_Metals("Ni") - R
                    End If
                Else
                    DR("Ni") = 0.01
                End If
            Else
                DR("Ni") = 0.01
            End If

            'Zn ************************************
            If Not DR_Metals("Zn") Is MyNull Then
                If DR_Metals("Zn") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Zn") = DR_Metals("Zn") + R
                    Else
                        DR("Zn") = DR_Metals("Zn") - R
                    End If
                Else
                    DR("Zn") = 0.01
                End If
            Else
                DR("Zn") = 0.01
            End If

            'Al ************************************
            If Not DR_Metals("Al") Is MyNull Then
                If DR_Metals("Al") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Al") = DR_Metals("Al") + R
                    Else
                        DR("Al") = DR_Metals("Al") - R
                    End If
                Else
                    DR("Al") = 0.01
                End If
            Else
                DR("Al") = 0.01
            End If

            'Fe ************************************
            If Not DR_Metals("Fe") Is MyNull Then
                If DR_Metals("Fe") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Fe") = DR_Metals("Fe") + R
                    Else
                        DR("Fe") = DR_Metals("Fe") - R
                    End If
                Else
                    DR("Fe") = 0.01
                End If
            Else
                DR("Fe") = 0.01
            End If

            'Cr ************************************
            If Not DR_Metals("Cr") Is MyNull Then
                If DR_Metals("Cr") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Cr") = DR_Metals("Cr") + R
                    Else
                        DR("Cr") = DR_Metals("Cr") - R
                    End If
                Else
                    DR("Cr") = 0.01
                End If
            Else
                DR("Cr") = 0.01
            End If

            'Cu ************************************
            If Not DR_Metals("Cu") Is MyNull Then
                If DR_Metals("Cu") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Cu") = DR_Metals("Cu") + R
                    Else
                        DR("Cu") = DR_Metals("Cu") - R
                    End If
                Else
                    DR("Cu") = 0.01
                End If
            Else
                DR("Cu") = 0.01
            End If

            'Na ************************************
            If Not DR_Metals("Na") Is MyNull Then
                If DR_Metals("Na") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("Na") = DR_Metals("Na") + R
                    Else
                        DR("Na") = DR_Metals("Na") - R
                    End If
                Else
                    DR("Na") = 0.01
                End If
            Else
                DR("Na") = 0.01
            End If

            'K ************************************
            If Not DR_Metals("K") Is MyNull Then
                If DR_Metals("K") > 0.01 Then
                    R = GetRandomNumber(9, 1) / 1000
                    M = GetRandomNumber(3, 1)
                    If M = 1 Then
                        DR("K") = DR_Metals("K") + R
                    Else
                        DR("K") = DR_Metals("K") - R
                    End If
                Else
                    DR("K") = 0.01
                End If
            Else
                DR("K") = 0.01
            End If

            'Co ************************************
            Try
                If Not DR_Metals("Co") Is MyNull Then
                    If DR_Metals("Co") > 0.01 Then
                        R = GetRandomNumber(9, 1) / 1000
                        M = GetRandomNumber(3, 1)
                        If M = 1 Then
                            DR("Co") = DR_Metals("Co") + R
                        Else
                            DR("Co") = DR_Metals("Co") - R
                        End If
                    Else
                        DR("Co") = 0.01
                    End If
                Else
                    DR("Co") = 0.01
                End If
            Catch ex As Exception
                DR("Co") = 0.01
            End Try


            'Mn ************************************
            Try
                If Not DR_Metals("Mn") Is MyNull Then
                    If DR_Metals("Mn") > 0.01 Then
                        R = GetRandomNumber(9, 1) / 1000
                        M = GetRandomNumber(3, 1)
                        If M = 1 Then
                            DR("Mn") = DR_Metals("Mn") + R
                        Else
                            DR("Mn") = DR_Metals("Mn") - R
                        End If
                    Else
                        DR("Mn") = 0.01
                    End If
                Else
                    DR("Mn") = 0.01
                End If
            Catch ex As Exception
                DR("Mn") = 0.01
            End Try


            'Mo ************************************
            Try
                If Not DR_Metals("Mo") Is MyNull Then
                    If DR_Metals("Mo") > 0.01 Then
                        R = GetRandomNumber(9, 1) / 1000
                        M = GetRandomNumber(3, 1)
                        If M = 1 Then
                            DR("Mo") = DR_Metals("Mo") + R
                        Else
                            DR("Mo") = DR_Metals("Mo") - R
                        End If
                    Else
                        DR("Mo") = 0.01
                    End If
                Else
                    DR("Mo") = 0.01
                End If
            Catch ex As Exception
                DR("Mo") = 0.01
            End Try


            'W ************************************
            Try
                If Not DR_Metals("W") Is MyNull Then
                    If DR_Metals("W") > 0.01 Then
                        R = GetRandomNumber(9, 1) / 1000
                        M = GetRandomNumber(3, 1)
                        If M = 1 Then
                            DR("W") = DR_Metals("W") + R
                        Else
                            DR("W") = DR_Metals("W") - R
                        End If
                    Else
                        DR("W") = 0.01
                    End If
                Else
                    DR("W") = 0.01
                End If
            Catch ex As Exception
                DR("W") = 0.01
            End Try


            'Ti ************************************
            Try
                If Not DR_Metals("Ti") Is MyNull Then
                    If DR_Metals("Ti") > 0.01 Then
                        R = GetRandomNumber(9, 1) / 1000
                        M = GetRandomNumber(3, 1)
                        If M = 1 Then
                            DR("Ti") = DR_Metals("Ti") + R
                        Else
                            DR("Ti") = DR_Metals("Ti") - R
                        End If
                    Else
                        DR("Ti") = 0.01
                    End If
                Else
                    DR("Ti") = 0.01
                End If
            Catch ex As Exception
                DR("Ti") = 0.01
            End Try


            'Au ************************************
            Try
                If Not DR_Metals("Au") Is MyNull Then
                    If DR_Metals("Au") > 0.01 Then
                        R = GetRandomNumber(9, 1) / 1000
                        M = GetRandomNumber(3, 1)
                        If M = 1 Then
                            DR("Au") = DR_Metals("Au") + R
                        Else
                            DR("Au") = DR_Metals("Au") - R
                        End If
                    Else
                        DR("Au") = 0.01
                    End If
                Else
                    DR("Au") = 0.01
                End If
            Catch ex As Exception
                DR("Au") = 0.01
            End Try

            'Ag ************************************
            Try
                If Not DR_Metals("Ag") Is MyNull Then
                    If DR_Metals("Ag") > 0.01 Then
                        R = GetRandomNumber(9, 1) / 1000
                        M = GetRandomNumber(3, 1)
                        If M = 1 Then
                            DR("Ag") = DR_Metals("Ag") + R
                        Else
                            DR("Ag") = DR_Metals("Ag") - R
                        End If
                    Else
                        DR("Ag") = 0.01
                    End If
                Else
                    DR("Ag") = 0.01
                End If
            Catch ex As Exception
                DR("Ag") = 0.01
            End Try

            'V ************************************
            Try
                If Not DR_Metals("V") Is MyNull Then
                    If DR_Metals("V") > 0.01 Then
                        R = GetRandomNumber(9, 1) / 1000
                        M = GetRandomNumber(3, 1)
                        If M = 1 Then
                            DR("V") = DR_Metals("V") + R
                        Else
                            DR("V") = DR_Metals("V") - R
                        End If
                    Else
                        DR("V") = 0.01
                    End If
                Else
                    DR("V") = 0.01
                End If
            Catch ex As Exception
                DR("V") = 0.01
            End Try

            DS.Tables("GFAAS Data").Rows.Add(DR)
            DA.Update(DS, "GFAAS Data")
        Next


        Connection.Close()
        Dim qcFinal As New Dictionary(Of String, Dictionary(Of String, String))
        qcFinal("@LotNumber") = GetParamVarHash(LotNumber, "varchar")
        WriteMetals = GetMyDataSetParamQuery("SELECT [Date/Time], Source, [Test Type], Location, Idenyification, Notes, NotesExtra, Ca, Ma, Ni, Zn, Al, Fe, Cr, Cu, Na, K, Co, Mn, Mo, W, Ti, Ag, Au, V FROM dbo.[GFAAS Data] WHERE (Source = N'SATI') AND (Idenyification = @LotNumber) AND (Notes IS NULL)", qcFinal)

    End Function

    ''' <summary>
    ''' Builds per-wafer CofA data for one or more carton boxes.
    ''' When T7s=True, constructs a query joining T7 process tables to retrieve wafer-level
    ''' measurements: pre/post thickness, TTV, warp, bow, TIR, resistivity, particle counts
    ''' (LPD bins, scratches, area defects), and diameter.
    ''' CartonString is a CR-delimited list of carton IDs (e.g. "CB394951" &amp; Chr(13)).
    ''' The Batch CofA path (T7s=False) is not yet implemented.
    ''' </summary>
    Function GetCofAData(ByVal CartonString As String, ByVal T7s As Boolean, ByVal Customer As String) As Data.DataSet

        If T7s = True Then ' Build T7 CofA Info
            Dim SQLString As String
            Dim MoreCartons As Boolean
            Dim SQLStringCount As Int16
            Dim SnipCarton As String
            Dim Carton_Int As Integer
            Dim qc As New Dictionary(Of String, Dictionary(Of String, String))

            SQLString = "SELECT TOP 100 PERCENT dbo.LabelsMade.Lot, dbo.LabelsMade.LotBoxNumber, dbo.T7_InstanceInfo.InstanceID AS BoxID, dbo.T7_InstanceInfo.Slot, LEFT(dbo.T7_WaferActionTracking.T7, 10) AS T7, T7_GeoData_2.CenterThick AS PreCenterThick, T7_GeoData_1.CenterThick AS PostCenterThk, T7_GeoData_1.TTV, T7_GeoData_1.TotWarp AS Warp, T7_GeoData_2.CenterThick - T7_GeoData_1.CenterThick AS Removal, T7_GeoData_1.CenterRes, T7_GeoData_1.Type, T7_GeoData_1.Bow, T7_GeoData_1.TIR, dbo.T7_ParticalData.SumAllDefects, dbo.T7_ParticalData.SP1BinCnt1, dbo.T7_ParticalData.SP1BinCnt2, dbo.T7_ParticalData.SP1BinCnt3, dbo.T7_ParticalData.SP1BinCnt4, dbo.T7_ParticalData.SP1BinCnt5, dbo.T7_ParticalData.SP1BinCnt6, dbo.T7_ParticalData.SP1BinCnt7, dbo.T7_ParticalData.SP1BinCnt8, dbo.T7_ParticalData.SP1BinCnt18, dbo.T7_ParticalData.StdDeviation, dbo.T7_ParticalData.AreaCnt, dbo.T7_ParticalData.TotalArea, dbo.T7_ParticalData.ScratchCnt, dbo.T7_ParticalData.ScratchTotalLength, dbo.T7_ParticalData.SP1LPDNBinCntInSize1, dbo.T7_ParticalData.SP1LPDNBinCntInSize2, dbo.T7_ParticalData.SP1LPDNBinCntInSize3, dbo.T7_ParticalData.SP1LPDNBinCntInSize4, dbo.T7_ParticalData.SP1LPDNBinCntInSize5, dbo.T7_ParticalData.SP1LPDNBinCntInSize6, dbo.T7_ParticalData.SP1LPDNBinCntInSize7, dbo.T7_ParticalData.SP1LPDNBinCntInSize8, dbo.T7_ParticalData.SP1LPDNBinCntInSize18, dbo.T7_ParticalData.SP1SOD1, dbo.T7_ParticalData.SP1SOD2, dbo.T7_ParticalData.SP1SOD3, dbo.T7_ParticalData.SP1SOD4, dbo.T7_ParticalData.SP1SOD5, dbo.T7_ParticalData.SP1SOD6, dbo.T7_ParticalData.SP1SOD7, dbo.T7_ParticalData.SP1SOD8, dbo.T7_ParticalData.SP1SOD18, dbo.T7_ParticalData.Average, dbo.T7_ParticalData.Peak, dbo.T7_ParticalData.Median, dbo.T7_ParticalData.EdgeExclusion, dbo.T7_ParticalData.RFID, dbo.T7_ParticalData.RFID_1, dbo.T_FGI_Boxes.BoxInvNumber as WB, dbo.T7_WaferActionTracking.PreGeo_Key, dbo.T_FGI_Boxes.CartonNumber, dbo.T7_ParticalData.RecordDate AS LaserScanDate, dbo.T7_ParticalData.ClusterAreaCnt, ISNULL(dbo.Q_Diameter_T7_Active.Diameter, 300.00) AS Diameter FROM dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key INNER JOIN dbo.T_FGI_Boxes ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey INNER JOIN dbo.LabelsMade ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber LEFT OUTER JOIN dbo.Q_Diameter_T7_Active ON dbo.T7_WaferActionTracking.WAT_Key = dbo.Q_Diameter_T7_Active.WAT_Key LEFT OUTER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData AS T7_GeoData_1 ON dbo.T7_WaferActionTracking.PostGeo_Key = T7_GeoData_1.Geo_Key LEFT OUTER JOIN dbo.T7_GeoData AS T7_GeoData_2 ON dbo.T7_WaferActionTracking.PreGeo_Key = T7_GeoData_2.Geo_Key WHERE (dbo.T_FGI_Boxes.CartonNumber = "

            MoreCartons = True
            Do Until MoreCartons = False 'Build Where
                SQLStringCount = SQLStringCount + 1
                SnipCarton = Left(CartonString, CartonString.IndexOf(Chr(13)))

                SnipCarton = Mid(SnipCarton, 3)
                Carton_Int = SnipCarton

                Dim paramName As String = "@Carton" & SQLStringCount
                qc(paramName) = GetParamVarHash(Carton_Int, "int")

                If SQLStringCount = 1 Then
                    SQLString = SQLString & paramName
                Else
                    SQLString = SQLString & ") OR (dbo.T_FGI_Boxes.CartonNumber = " & paramName
                End If

                CartonString = Mid(CartonString, CartonString.IndexOf(Chr(13)) + 2)

                If Not CartonString.Contains(Chr(13)) Then
                    SQLString = SQLString & ") ORDER BY dbo.T7_InstanceInfo.InstanceID, dbo.T7_InstanceInfo.Slot"
                    MoreCartons = False
                    Exit Do
                End If
            Loop

            Return GetMyDataSetParamQuery(SQLString, qc)

        Else 'Build Batch CofA info
            'Build Batch CofA info
            'Build Batch CofA info
            Return New Data.DataSet
        End If

    End Function

    ''' <summary>
    ''' Returns a single summary DataRow with AVG/MIN/MAX/STDEV statistics across all wafers
    ''' in the given carton(s) for thickness, TTV, TIR, resistivity, bow, warp, and LPD bins 1-8.
    ''' Looks up InstanceKey per carton, then aggregates T7 geometry and particle data.
    ''' CartonString is a CR-delimited list of carton IDs. The Batch path (T7s=False) is not yet implemented.
    ''' </summary>
    Function GetCofADataSumary(ByVal CartonString As String, ByVal T7s As Boolean) As Data.DataRow

        If T7s = True Then ' Build T7 CofA Info
            Dim SQLString As String
            Dim MoreCartons As Boolean
            Dim SQLStringCount As Int16
            Dim SnipCarton As String
            Dim Carton_Int As Integer
            Dim Ikey As String
            Dim qc As New Dictionary(Of String, Dictionary(Of String, String))

            SQLString = "SELECT TOP 100 PERCENT AVG(dbo.T7_GeoData.CenterThick) AS ThickAVG, MIN(dbo.T7_GeoData.CenterThick) AS ThickMin, MAX(dbo.T7_GeoData.CenterThick) AS ThickMax, STDEV(dbo.T7_GeoData.CenterThick) AS ThickStdev, AVG(dbo.T7_GeoData.TTV) AS TTVAvg, MIN(dbo.T7_GeoData.TTV) AS TTVMin, MAX(dbo.T7_GeoData.TTV) AS TTVMax, STDEV(dbo.T7_GeoData.TTV) AS TTVStdev, AVG(dbo.T7_GeoData.TIR) AS TIRAvg, MIN(dbo.T7_GeoData.TIR) AS TIRMin, MAX(dbo.T7_GeoData.TIR) AS TIRMax, STDEV(dbo.T7_GeoData.TIR) AS TIRStdev, AVG(dbo.T7_GeoData.CenterRes) AS ResAvg, MIN(dbo.T7_GeoData.CenterRes) AS ResMin, AVG(dbo.T7_GeoData.CenterRes) AS ResMax, STDEV(dbo.T7_GeoData.CenterRes) AS ResStdev, AVG(dbo.T7_GeoData.Bow) AS BowAvg, MIN(dbo.T7_GeoData.Bow) AS BowMin, MAX(dbo.T7_GeoData.Bow) AS BowMax, STDEV(dbo.T7_GeoData.Bow) AS BowStdev, AVG(dbo.T7_GeoData.TotWarp) AS WarpAvg, MIN(dbo.T7_GeoData.TotWarp) AS WarpMin, MAX(dbo.T7_GeoData.TotWarp) AS WarpMax, STDEV(dbo.T7_GeoData.TotWarp) AS WarpStdev, AVG(dbo.T7_ParticalData.SP1BinCnt1) AS LPDBin1Avg, MIN(dbo.T7_ParticalData.SP1BinCnt1) AS LPDBin1Min, MAX(dbo.T7_ParticalData.SP1BinCnt1) AS LPDBin1Max, STDEV(dbo.T7_ParticalData.SP1BinCnt1) AS LPDBin1Stdev, AVG(dbo.T7_ParticalData.SP1BinCnt2) AS LPDBin2Avg, MIN(dbo.T7_ParticalData.SP1BinCnt2) AS LPDBin2Min, MAX(dbo.T7_ParticalData.SP1BinCnt2) AS LPDBin2Max, STDEV(dbo.T7_ParticalData.SP1BinCnt2) AS LPDBin2Stdev, AVG(dbo.T7_ParticalData.SP1BinCnt3) AS LPDBin3Avg, MIN(dbo.T7_ParticalData.SP1BinCnt3) AS LPDBin3Min, MAX(dbo.T7_ParticalData.SP1BinCnt3) AS LPDBin3Max, STDEV(dbo.T7_ParticalData.SP1BinCnt3) AS LPDBin3Stdev, AVG(dbo.T7_ParticalData.SP1BinCnt4) AS LPDBin4Avg, MIN(dbo.T7_ParticalData.SP1BinCnt4) AS LPDBin4Min, MAX(dbo.T7_ParticalData.SP1BinCnt4) AS LPDBin4Max, STDEV(dbo.T7_ParticalData.SP1BinCnt4) AS LPDBin4Stdev, AVG(dbo.T7_ParticalData.SP1BinCnt5) AS LPDBin5Avg, MIN(dbo.T7_ParticalData.SP1BinCnt5) AS LPDBin5Min, MAX(dbo.T7_ParticalData.SP1BinCnt5) AS LPDBin5Max, STDEV(dbo.T7_ParticalData.SP1BinCnt5) AS LPDBin5Stdev, AVG(dbo.T7_ParticalData.SP1BinCnt6) AS LPDBin6Avg, MIN(dbo.T7_ParticalData.SP1BinCnt6) AS LPDBin6Min, MAX(dbo.T7_ParticalData.SP1BinCnt6) AS LPDBin6Max, STDEV(dbo.T7_ParticalData.SP1BinCnt6) AS LPDBin6Stdev, AVG(dbo.T7_ParticalData.SP1BinCnt7) AS LPDBin7Avg, MIN(dbo.T7_ParticalData.SP1BinCnt7) AS LPDBin7Min, MAX(dbo.T7_ParticalData.SP1BinCnt7) AS LPDBin7Max, STDEV(dbo.T7_ParticalData.SP1BinCnt7) AS LPDBin7Stdev, AVG(dbo.T7_ParticalData.SP1BinCnt8) AS LPDBin8Avg, MIN(dbo.T7_ParticalData.SP1BinCnt8) AS LPDBin8Min, MAX(dbo.T7_ParticalData.SP1BinCnt8) AS LPDBin8Max, MIN(dbo.T7_ParticalData.SP1BinCnt8) AS LPDBin8Stdev FROM dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key INNER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = dbo.T7_GeoData.Geo_Key WHERE (dbo.T7_InstanceInfo.InstanceID = "

            MoreCartons = True
            Do Until MoreCartons = False 'Build Where
                SQLStringCount = SQLStringCount + 1
                SnipCarton = Left(CartonString, CartonString.IndexOf(Chr(13)))

                SnipCarton = Mid(SnipCarton, 3)
                Carton_Int = SnipCarton

                ' Look up InstanceKey for this carton using parameterized query
                Dim cartonQc As New Dictionary(Of String, Dictionary(Of String, String))
                cartonQc("@CartonNumber") = GetParamVarHash(Carton_Int, "int")
                Dim DS_Carton As Data.DataSet = GetMyDataSetParamQuery("SELECT dbo.T_FGI_Boxes.InstanceKey AS BoxID FROM dbo.T_FGI_Boxes WHERE (dbo.T_FGI_Boxes.CartonNumber = @CartonNumber)", cartonQc)
                Dim DR_Carton As Data.DataRow = DS_Carton.Tables(0).Rows(0)
                Ikey = DR_Carton("BoxID")

                Dim paramName As String = "@InstanceID" & SQLStringCount
                qc(paramName) = GetParamVarHash(Ikey, "int")

                If SQLStringCount = 1 Then
                    SQLString = SQLString & paramName
                Else
                    SQLString = SQLString & ") OR (dbo.T7_InstanceInfo.InstanceID = " & paramName
                End If

                CartonString = Mid(CartonString, CartonString.IndexOf(Chr(13)) + 2)

                If Not CartonString.Contains(Chr(13)) Then
                    SQLString = SQLString & ")"
                    MoreCartons = False
                    Exit Do
                End If
            Loop

            Dim DS_Result As Data.DataSet = GetMyDataSetParamQuery(SQLString, qc)
            Return DS_Result.Tables(0).Rows(0)

        Else 'Build Batch CofA info
            'Build Batch CofA info
            'Build Batch CofA info
            Return Nothing
        End If

    End Function

End Class
