# SurfScan (Bin Fall) Report

## Overview

- **Purpose:** Wafer surface scan inspection report for viewing pass/reject data from surface particle detection equipment
- **Page Title:** Bin Fall
- **Location:** `Reports/SurfScan.aspx`
- **Code-behind:** `Reports/SurfScan.aspx.vb`

## Supported Equipment

### SPx Tools (Primary)
| Checkbox Label | Machine ID |
|----------------|------------|
| SP1 | SP1 |
| SP1-2 | SP2 |
| SP1-3 | SP1-3 |
| SP2 | SP2-S0132 |
| SP3-1 | SP3-2110224 |
| SP3-2 | SP3-2110164 |
| SP5-1 | SP5-2130406 |

### Tencor Tools (Hidden by default)
- Tencor
- Tencor 3
- Tencor 4

## Features

### Session Quantity Selection
- **10, 25, 50, 75:** Returns top N records
- **Select Date:** Custom date range with start/end date pickers

### Wafer Diameter Filter
- All
- 200mm (WaferDia = 200000)
- 300mm (WaferDia = 300000)

### Daily Tests
- "Find all 'Daily' tests on all tools" checkbox searches for sessions with "Daily" in the name

### Advanced Filtering
Enable via "Advanced Filter" checkbox to access:

| Filter | Description |
|--------|-------------|
| ID | Wafer ID filtering (find only or exclude) |
| Run | Run number filtering |
| WL | Wafer log filtering |
| Session Name | Session name filtering |
| Comment 1 | CMP type indicator |
| Comment 2 | Instance numbers |
| CMP Type | Dropdown for CMP 1-5, 4L, 4R |
| Remove Daily Tests | Excludes sessions with "Daily" in name |
| Archives | Query archived data instead of live (last 90 days) |

### Pass Bin Configuration
- **Bin 2:** Count wafers sent to destination station 2 as passed
- **Bin 3:** Count wafers sent to destination station 3 as passed
- **Bins 2 & 3:** Count both as passed

### Slot Filtering
Individual checkboxes for slots 1-25 to include/exclude specific source slots from the query.

### Footer Summation
"Show Footer As Sum" checkbox toggles between column labels and calculated totals in the grid footer.

### Data Export
- Enter email prefix (appends @purewafer.com)
- Exports GridView data to CSV
- Sends via email using `Saticode.SendMailWithFile()`
- File saved to: `\\PWI-40\TempImageWebFiles$\`

## Data Sources

| Table | Purpose |
|-------|---------|
| `dbo.SP1_Data` | Live SPx tool inspection data (cache of tool data) |
| `dbo.Archive_SP1_Data` | Archived SPx data (older than 90 days) |
| `dbo.Tencor_Data` | Tencor tool inspection data |
| `dbo.T_Spec_Scan_Log` | Spec scan timestamps |
| `dbo.CofA_Info` | Certificate of Analysis bin configuration |

## Data Refresh Mechanism

The `SP1_Data` table acts as a **cache** of data from the physical surf scan tool systems. Data is not inserted directly by the .NET application. Instead, stored procedures sync data from the tools on-demand.

### Stored Procedures

| Procedure | Tool |
|-----------|------|
| `exsil_user.[SP1DataCollector_SP11Only]` | SP1 |
| `exsil_user.[SP1DataCollector_SP12Only]` | SP2 |
| `exsil_user.[SP1DataCollector_SP13Only]` | SP1-3 |

**Note:** SP2-S0132 uses real-time data collection and does not require manual refresh.

### How Refresh Works

1. User triggers a refresh (e.g., clicks "Update SP1" button)
2. Application calls the appropriate stored procedure via `UpdateSPxTool()` in `App_Code/Class1.vb`
3. Stored procedure pulls latest data from the tool system into `SP1_Data`
4. Application queries the now-updated `SP1_Data` table

### Refresh Triggers in This Page

The Advanced Filter panel contains "Update SP1", "Update SP12", and "Update SP13" buttons that call:
- `SP1UpdateButton_Click` → `Saticode.UpdateSPxTool("SP1")` (line 776)
- `SP12UpdateButton_Click` → `Saticode.UpdateSPxTool("SP2")` (line 781)
- `SP13UpdateButton_Click` → `Saticode.UpdateSPxTool("SP13")` (line 1194)

### Other Pages That Refresh SP1_Data

| Page | Method | User Action |
|------|--------|-------------|
| `Production/MakeSurfScanWaferBoxLabel.aspx` | `Button_Pull_Data_Click` | Pull data for label printing |
| `Production/SurfScanLabelMaker.aspx` | `GoSPxSQL()` | Generate surf scan labels |
| `SPC/SPC_Compile.aspx` | `UpdateToolData()` | Compile SPC statistics |
| `Production/SPxT7DupeCompatibilityCheck.aspx` | `FindOut()` | Check T7 compatibility |

### Why Refresh Is Needed

Without calling refresh, users query potentially stale data that doesn't reflect wafers scanned since the last sync. Users trigger refresh when they need the most current data from the physical tool systems.

## UI Components

### Main Panel
- Tool selection radio buttons and checkboxes
- Session quantity options
- Diameter dropdown
- Advanced filter toggle

### GridView1 (SPx Data)
Displays aggregated session data:
- SessionDate, Comment2 (Instance), SPSessionName, Lot
- Wafers In, Passed, %Pass, Rejects, OverLoads, Bin2, Bin3, Comment1 (CMP)

### GridView2 (Tencor Data)
Displays Tencor session data:
- Run Time, Tencor, Recipe, Operator, Lot
- Wafers, Passed, Reject, %Pass

### Modal Popups

#### SPx Detail Panel
Shows individual wafer data for a session:
- Source slot, destination, disposition class
- SOD1-SOD8 values, Scratch Count, Cluster Area Count
- Map button for wafer map image

#### Tencor Detail Panel
Shows individual wafer data:
- Event time, slot, sort result, LPD counts by size bin
- Scratch count, area count

#### Map Panel
Displays wafer map images with navigation:
- Back/Next map buttons
- Back/Next session buttons
- Map images loaded from `SatiMapsDir` or `SP2Files` session paths

## Key Methods

| Method | Purpose |
|--------|---------|
| `UpdateData()` | Builds and executes SQL query based on filter selections |
| `FooterSum()` | Calculates and displays footer totals |
| `ChangeToolSet()` | Toggles visibility between SPx and Tencor controls |
| `getbucket()` | Loads session detail data for popup display |
| `LookMap()` | Loads wafer map image for display |
| `ExportData()` | Exports grid data to CSV and sends email |

## Authentication
Page uses `MenuAuthenication.AuthenicationByPass(Page)` on load.
