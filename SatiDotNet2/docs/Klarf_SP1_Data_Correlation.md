# Klarf File to SP1_Data Correlation

## Overview

Klarf files (`.001`) are created by SP1 surf scan tools and contain individual wafer inspection data. This document explains how to correlate a klarf file to its corresponding record in the `SP1_Data` table.

## Klarf File Format

Klarf files are text-based with a structured format. Key fields for correlation:

```
FileVersion 1 2;
FileTimestamp 12-31-25 11:38:44;
LotID "2850";
StepID "6346";
WaferID "01";
Slot 1;
ResultTimestamp 12-31-25 11:37:37;
ProcessEquipmentIDList 1
  "6448" ;
```

## Field Mapping

| Klarf Field | SP1_Data Column | Description |
|-------------|-----------------|-------------|
| `LotID` | `ID#` | Wafer lot identifier |
| `StepID` | `RUN#` | Process step/run number |
| `ProcessEquipmentIDList` | `Wafer_log` | Equipment ID used for processing |
| `Slot` | `SourceSlotID` | Cassette slot position (1-25) |
| `ResultTimestamp` | `SessionDate` | Scan session timestamp |

## Correlation Method

### Required Fields for Unique Identification

All 5 fields are required to uniquely identify a SP1_Data record:

| Composite Key | Accuracy |
|---------------|----------|
| LotID + StepID + ProcessEquip + Slot | 13.72% |
| LotID + StepID + ProcessEquip + Slot + **ResultTimestamp** | **100%** |

**Why all 5 fields?** The same wafer (same Lot/Step/Equipment/Slot) can be scanned multiple times. In test data, some wafers appeared in up to 14 different klarf files due to re-scans.

### SQL Query

```sql
SELECT * FROM SP1_Data
WHERE [ID#] = '<LotID>'
  AND [RUN#] = '<StepID>'
  AND Wafer_log = '<ProcessEquipmentIDList>'
  AND SourceSlotID = <Slot>
  AND SessionDate = '<ResultTimestamp>'
```

### Example

For klarf file with:
- `LotID "2850"`
- `StepID "6346"`
- `Slot 1`
- `ProcessEquipmentIDList "6448"`
- `ResultTimestamp 12-31-25 11:37:37`

Query:
```sql
SELECT * FROM SP1_Data
WHERE [ID#] = '2850'
  AND [RUN#] = '6346'
  AND Wafer_log = '6448'
  AND SourceSlotID = 1
  AND SessionDate = '2025-12-31 11:37:37'
```

## Filename Analysis

The klarf filename (e.g., `5B9F5CD6.001`) is a **hex-encoded Unix timestamp** representing the file creation time.

| Filename | Hex Value | Unix Timestamp | DateTime (UTC) |
|----------|-----------|----------------|----------------|
| 5B9F5CD6.001 | 5B9F5CD6 | 1537170646 | 09/17/2018 07:50:46 |

**Note:** The filename timestamp is the file creation time, NOT the scan time. Use `ResultTimestamp` from inside the file for correlation.

### Converting Filename to DateTime

```csharp
// C# / VB.NET
string filename = "5B9F5CD6.001";
string hex = filename.Replace(".001", "");
long unixTimestamp = Convert.ToInt64(hex, 16);
DateTime fileDate = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
```

```sql
-- SQL Server (if needed)
DECLARE @hex VARCHAR(8) = '5B9F5CD6'
DECLARE @unixTs BIGINT = CONVERT(BIGINT, CONVERT(VARBINARY(8), '0x' + @hex, 1))
SELECT DATEADD(SECOND, @unixTs, '1970-01-01')
```

## Analysis Summary

Based on analysis of 4,336 klarf files:

| Metric | Value |
|--------|-------|
| Total files analyzed | 4,336 |
| Date range | 09/17/2018 - 02/06/2019 |
| Unique LotIDs | 2 |
| Unique StepIDs | 24 |
| Unique ProcessEquipment | 13 |
| Unique Slots | 25 |
| Max re-scans per wafer | 14 |

## Parsing Klarf Files

### Key Fields to Extract

```
LotID "<value>"           → ID#
StepID "<value>"          → RUN#
Slot <number>;            → SourceSlotID
ProcessEquipmentIDList N
  "<value>" ;             → Wafer_log
ResultTimestamp <value>;  → SessionDate
```

### Regex Patterns

| Field | Regex Pattern |
|-------|---------------|
| LotID | `LotID "([^"]+)"` |
| StepID | `StepID "([^"]+)"` |
| Slot | `Slot (\d+);` |
| ProcessEquipmentIDList | `ProcessEquipmentIDList \d+\s+"([^"]+)"` |
| ResultTimestamp | `ResultTimestamp ([^;]+);` |

## Related Documentation

- [SP1_Data Collection Architecture](SP1_Data_Collection.md) - How data flows from tools to SP1_Data
- [SurfScan (Bin Fall) Report](SurfScan.md) - Report that displays SP1_Data
