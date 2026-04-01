# SatiDotNet2

## Release / Production

`Web.Release.config` contains XDT transforms that replace dev defaults with production values during **Publish**. The only values that differ between `Web.config` (Debug) and `Web.Release.config` (Release) are:

- **Database connection strings** — point to the `PWI-31\SATIDB` SQL Server instance with SQL authentication
- **300mm CofA paths** — point to network shares on `\\PWI-40`

These transforms are **not** applied during a regular Build — only when you Publish the website.

## Development Setup

`Web.config` contains local development defaults. No changes are needed to run the application locally.

### Database Connection Strings

All connections default to `localhost` with Windows Authentication:

| Name | Database |
|------|----------|
| ALTSConnectionString | ALTS |
| SATI_SPCConnectionString | SATI_SPC |
| AutoDataConnectionString | AutoData |
| SatiUsersConnectionString | SatiUsers |
| SatiToolsConnectionString | SatiTools |
| LocalSqlServer | SatiUsers |

### 300mm CofA Paths

These paths are used for generating Certificates of Analysis (CofAs) in the **MakeShipment** webpage. They default to the local `$CofATests` directory:

| AppSetting Key | Default Dev Path |
|----------------|-----------------|
| CofA:TemplatePath | `$CofATests\LabelTemplates\SatiCofA.xls` |
| CofA:ArchivePath | `$CofATests\LabelArchive\` |
| CofA:CustomerDataPath | `$CofATests\CustomerData\` |

These paths are relative to the repo root (`SatiDotNet2\$CofATests\`). Ensure the `$CofATests` directory and its subdirectories exist locally before running CofA features.

## Test Project

`SatiDotNet2Tests/app.config` has its own CofA dev paths (`C:\Dev\LabelTemplates\`, etc.) independent of the web project's `Web.config`.
