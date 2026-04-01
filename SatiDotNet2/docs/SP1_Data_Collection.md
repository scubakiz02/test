# SP1_Data Collection Architecture

## Overview

The `SP1_Data` table in the AutoData database acts as a central cache of wafer inspection data from physical surf scan tools. Data is not inserted directly by the .NET application.

**Two data collection methods exist:**

| Method | Tools | How It Works |
|--------|-------|--------------|
| Linked Servers + Stored Procedures | SP1, SP1-3 | SQL Server linked servers connect to tool databases; stored procedures sync data on-demand |
| SP2DC Software | SP2, SP3, SP5 | External data collection software uploads data directly to SP1_Data |

This document focuses on the linked server approach used for SP1 and SP1-3.

## Linked Servers

Linked servers are configured in SQL Server under `Server Objects > Linked Servers`. These provide direct database connectivity to the surf scan tool systems.

| Linked Server | Tool | Used By |
|---------------|------|---------|
| `SP11` | SP1 | `[exsil_user].[SP1DataCollector_SP11Only]` |
| `SP1_3` | SP1-3 | `[exsil_user].[SP1DataCollector_SP13Only]` |

## Tool Database Schema

Each surf scan tool has its own database (`SP1`) with the following tables that store inspection results:

| Table | Purpose |
|-------|---------|
| `TBL_DataSumInfo` | Session/wafer info: IDs, dates, recipe, disposition, slot positions |
| `TBL_DataSumClassification` | Classification data: area counts, scratch metrics, cluster counts |
| `TBL_DataSumCommonChannel` | Channel data: sum of all defects |
| `TBL_DataSumHazeMap` | Haze measurements: average, peak, median, std deviation |
| `TBL_DataSumDefect` | Defect counts: bin counts, LPD counts by size |

**Important:** The AutoData database also contains tables with similar names (`TBL_DataSum*`). These are **obsolete** and are not the same as the tables on the physical tool databases. The stored procedures query the tool databases via linked servers, not the local AutoData copies.

## Stored Procedures

### `[exsil_user].[SP1DataCollector_SP11Only]`

Syncs data from the SP1 tool into `SP1_Data`.

**Data Flow:**
```
SP11 (Linked Server)
    └── SP1.dbo.TBL_DataSumInfo
    └── SP1.dbo.TBL_DataSumClassification
    └── SP1.dbo.TBL_DataSumCommonChannel
    └── SP1.dbo.TBL_DataSumHazeMap
    └── SP1.dbo.TBL_DataSumDefect
            │
            ▼
    JOIN all tables on CreationDate + ChannelID
            │
            ▼
    LEFT JOIN with existing SP1_Data (Machine='SP1')
            │
            ▼
    WHERE SP1_Data.CreationDate IS NULL (new records only)
            │
            ▼
    INSERT into SP1_Data with Machine='SP1'
```

### `[exsil_user].[SP1DataCollector_SP13Only]`

Syncs data from the SP1-3 tool into `SP1_Data`. Identical logic to SP11Only but uses:
- Linked server: `SP1_3`
- Machine value: `'SP1-3'`

## Key Logic

### New Records Only

Both procedures use a LEFT OUTER JOIN pattern to avoid duplicates:

```sql
LEFT OUTER JOIN dbo.SP1_Data SP1
    ON Info.CreationDate = SP1.CreationDate
    AND SP1.Machine = 'SP1'  -- or 'SP1-3'
WHERE SP1.CreationDate IS NULL
```

This ensures only records that don't already exist in `SP1_Data` are inserted.

### Composite Channel Filter

Both procedures filter for composite channel data only:

```sql
WHERE LEFT(Defect.ChannelID, 9) = 'Composite'
```

### SOD Column Calculation

Size-of-Defect (SOD) columns are calculated by combining bin counts:

```sql
SOD1 = Defect.BinCnt1 + Defect.LPDNBinCntInSize1
SOD2 = Defect.BinCnt2 + Defect.LPDNBinCntInSize2
...
SOD8 = Defect.BinCnt8 + Defect.LPDNBinCntInSize8
SOD18 = Defect.BinCnt18 + Defect.LPDNBinCntInSize18
```

### Column Mappings

Key field mappings from tool database to SP1_Data:

| SP1_Data Column | Source Table | Source Column |
|-----------------|--------------|---------------|
| `ID#` | Info | `LotIdLabel` (trimmed) |
| `RUN#` | Info | `StepID` (trimmed) |
| `Wafer_log` | Info | `ProcessToolID` (trimmed) |
| `Comment1` | Info | `ProcessGroup` |
| `Comment2` | Info | `ProcessArea` |
| `SessionDate` | Info | `SessionDate` |
| `SourceSlotID` | Info | `SourceSlotID` |
| `DestinationStationID` | Info | `DestinationStationID` |
| `DestinationSlotID` | Info | `DestinationSlotID` |
| `DispositionName` | Info | `DispositionName` |
| `ScratchCnt` | Class | `ScratchCnt` |
| `ClusterAreaCnt` | Class | `ClusterAreaCnt` |
| `SumAllDefects` | Chnl | `SumAllDefects` |
| `Average`, `Peak`, `Median` | Haze | Haze measurements |
| `BinCnt1-8`, `PosCnt`, `NegCnt` | Defect | Defect counts |

## Triggering Data Collection

The .NET application triggers these stored procedures via:

1. **Shared utility method:** `UpdateSPxTool()` in `App_Code/Class1.vb`
2. **Direct calls:** Some pages call the stored procedures directly

See [SurfScan.md](SurfScan.md#data-refresh-mechanism) for the list of pages that trigger refresh.

## Architecture Diagram

```
┌─────────────────┐     ┌─────────────────┐
│   SP1 Tool      │     │   SP1-3 Tool    │
│   (Physical)    │     │   (Physical)    │
└────────┬────────┘     └────────┬────────┘
         │                       │
         ▼                       ▼
┌─────────────────┐     ┌─────────────────┐
│  SP11 Linked    │     │  SP1_3 Linked   │
│     Server      │     │     Server      │
└────────┬────────┘     └────────┬────────┘
         │                       │
         ▼                       ▼
┌─────────────────────────────────────────┐
│         SQL Server (AutoData)           │
│  ┌───────────────────────────────────┐  │
│  │ SP1DataCollector_SP11Only         │  │
│  │ SP1DataCollector_SP13Only         │  │
│  └───────────────┬───────────────────┘  │
│                  ▼                      │
│  ┌───────────────────────────────────┐  │
│  │          SP1_Data Table           │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────┐
│        SatiDotNet2 Web Application      │
│  (Reports, Label Printing, SPC, etc.)   │
└─────────────────────────────────────────┘
```
