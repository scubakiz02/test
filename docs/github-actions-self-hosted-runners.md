# GitHub Actions Self-Hosted Runners

This guide explains how to set up self-hosted runners for deploying SatiDotNet2 to on-premises IIS servers.

## Overview

A **self-hosted runner** is a machine you control that executes GitHub Actions workflows. Instead of using GitHub's cloud-hosted runners (`runs-on: windows-latest`), your workflows run on your own server.

### Architecture

```
┌─────────────────┐         ┌──────────────────────────────┐
│   GitHub.com    │         │   Your On-Prem Network       │
│                 │         │                              │
│  ┌───────────┐  │  HTTPS  │  ┌────────────────────────┐  │
│  │  Actions  │◄─┼────────►┼──│  Self-Hosted Runner    │  │
│  │  Workflow │  │  (443)  │  │  (Windows Server)      │  │
│  └───────────┘  │         │  └──────────┬─────────────┘  │
│                 │         │             │                │
└─────────────────┘         │             ▼                │
                            │  ┌────────────────────────┐  │
                            │  │   IIS Server           │  │
                            │  │   (SatiDotNet2)        │  │
                            │  └────────────────────────┘  │
                            └──────────────────────────────┘
```

### How It Works

1. **Runner polls GitHub** - The runner connects outbound to GitHub (no inbound firewall rules needed)
2. **GitHub queues a job** - When a workflow triggers, GitHub assigns it to an available runner with matching labels
3. **Runner executes steps** - Downloads your code, runs the workflow steps on your machine
4. **Reports back** - Sends logs, status, and artifacts back to GitHub

## Requirements

| Requirement | Details |
|-------------|---------|
| **OS** | Windows Server 2016+ |
| **Network** | Outbound HTTPS to `github.com`, `*.actions.githubusercontent.com` |
| **Permissions** | Service account with IIS management rights |
| **Disk space** | ~5GB for runner + workspace |
| **Availability** | Server must be running to pick up jobs |

## Installation Guide

Setup typically takes 15-30 minutes.

### Step 1: Create the Runner in GitHub

1. Go to your repo → **Settings** → **Actions** → **Runners**
2. Click **New self-hosted runner**
3. Select **Windows** and **x64**

### Step 2: Install on Your Server

GitHub provides exact commands. On your Windows server, run PowerShell as Administrator:

```powershell
# Create a folder
mkdir C:\actions-runner
cd C:\actions-runner

# Download the runner package (check GitHub for latest version)
Invoke-WebRequest -Uri https://github.com/actions/runner/releases/download/v2.311.0/actions-runner-win-x64-2.311.0.zip -OutFile actions-runner.zip

# Extract
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory("$PWD\actions-runner.zip", "$PWD")

# Configure (GitHub provides your specific token)
.\config.cmd --url https://github.com/Pure-Wafer/SATI --token YOUR_TOKEN_HERE
```

During configuration, you'll be prompted for:

| Prompt | Recommendation |
|--------|----------------|
| **Runner group** | Default is fine |
| **Runner name** | e.g., `staging-server` or `prod-iis-01` |
| **Labels** | Add custom labels like `staging`, `production`, `iis` |
| **Work folder** | Where builds happen (default `_work`) |

### Step 3: Run as a Windows Service

```powershell
# Install as service (runs on boot, survives restarts)
.\svc.cmd install

# Start the service
.\svc.cmd start

# Check status
.\svc.cmd status
```

### Step 4: Verify Runner is Online

Once installed, verify your runner appears in GitHub:

**Settings → Actions → Runners**

```
NAME              STATUS    LABELS
staging-server    🟢 Idle   self-hosted, windows, staging
prod-server       🟢 Idle   self-hosted, windows, production
```

## Workflow Configuration

### Using Self-Hosted Runners

Update your workflow to target your self-hosted runner:

```yaml
jobs:
  deploy:
    # Use your self-hosted runner instead of GitHub's
    runs-on: [self-hosted, windows, staging]
    steps:
      - name: Deploy to IIS
        run: |
          # This runs ON your server - direct access to IIS, file system, etc.
          Import-Module WebAdministration
          Stop-WebSite -Name "SatiDotNet2"
          # ... deployment steps
```

### Example: Full CI/CD Pipeline

```yaml
name: Build, Test & Deploy

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: windows-latest
    steps:
    - uses: actions/checkout@v4

    - uses: NuGet/setup-nuget@v1
    - uses: microsoft/setup-msbuild@v1

    - name: Restore packages
      run: nuget restore SatiDotNet2/SatiDotNet2.sln

    - name: Build & Publish
      run: |
        msbuild SatiDotNet2/SatiDotNet2.sln `
          /p:Configuration=Release `
          /p:DeployOnBuild=true `
          /p:PublishProfile=FolderProfile `
          /p:PublishUrl=./publish

    - name: Run tests
      run: vstest.console.exe "SatiDotNet2Tests/bin/Release/SatiDotNet2Tests.dll"

    - name: Upload artifact
      uses: actions/upload-artifact@v4
      with:
        name: web-app
        path: ./publish

  deploy-staging:
    needs: build
    runs-on: [self-hosted, windows, staging]
    if: github.ref == 'refs/heads/main'
    environment: staging

    steps:
    - name: Download artifact
      uses: actions/download-artifact@v4
      with:
        name: web-app
        path: ./deploy

    - name: Stop IIS Site
      run: |
        Import-Module WebAdministration
        Stop-WebSite -Name "SatiDotNet2-Staging"

    - name: Deploy to IIS
      run: |
        robocopy ./deploy "D:\inetpub\SatiDotNet2-Staging" /MIR /XF web.config
      continue-on-error: true  # robocopy returns non-zero on success

    - name: Start IIS Site
      run: |
        Import-Module WebAdministration
        Start-WebSite -Name "SatiDotNet2-Staging"

  deploy-production:
    needs: deploy-staging
    runs-on: [self-hosted, windows, production]
    if: github.ref == 'refs/heads/main'
    environment:
      name: production
      url: https://sati.purewafer.com

    steps:
    - name: Download artifact
      uses: actions/download-artifact@v4
      with:
        name: web-app
        path: ./deploy

    - name: Deploy with Web Deploy
      run: |
        msdeploy.exe -verb:sync `
          -source:contentPath="./deploy" `
          -dest:contentPath="D:\inetpub\SatiDotNet2",computerName="localhost" `
          -skip:objectName=filePath,absolutePath="web.config"
```

## Security Considerations

| Concern | Recommendation |
|---------|----------------|
| **Repo access** | Runner can access any secret/code in workflows - use only for trusted repos |
| **Isolation** | Consider separate runners for staging vs production |
| **Service account** | Use a dedicated account, not admin, with only IIS permissions |
| **Private repo** | For private repos, runner auto-cleans work folder |

### Service Account Permissions

The runner service account needs:

- **IIS Management**: Read/write to `IIS:\Sites\*`
- **File System**: Read/write to deployment target directories
- **Log on as a service**: Required for Windows service operation

## Troubleshooting

### Runner Shows Offline

1. Check the service is running:
   ```powershell
   Get-Service actions.runner.*
   ```

2. Check network connectivity:
   ```powershell
   Test-NetConnection github.com -Port 443
   Test-NetConnection pipelines.actions.githubusercontent.com -Port 443
   ```

3. Review runner logs:
   ```
   C:\actions-runner\_diag\
   ```

### Deployment Fails with Access Denied

1. Verify service account has IIS permissions:
   ```powershell
   # Check IIS site permissions
   Import-Module WebAdministration
   Get-WebSite -Name "SatiDotNet2" | Select-Object *
   ```

2. Verify file system permissions on target directory

### Runner Not Picking Up Jobs

1. Verify labels match your workflow:
   ```yaml
   runs-on: [self-hosted, windows, staging]  # Must match runner labels
   ```

2. Check runner is assigned to correct runner group

## Maintenance

### Updating the Runner

```powershell
cd C:\actions-runner

# Stop service
.\svc.cmd stop

# Download new version
Invoke-WebRequest -Uri https://github.com/actions/runner/releases/download/vX.X.X/actions-runner-win-x64-X.X.X.zip -OutFile actions-runner-new.zip

# Extract (backup old files first)
# ... extraction steps

# Start service
.\svc.cmd start
```

### Removing a Runner

```powershell
cd C:\actions-runner

# Stop and uninstall service
.\svc.cmd stop
.\svc.cmd uninstall

# Remove from GitHub
.\config.cmd remove --token YOUR_REMOVE_TOKEN
```

## Additional Resources

- [GitHub Docs: Self-hosted runners](https://docs.github.com/en/actions/hosting-your-own-runners)
- [GitHub Docs: Adding self-hosted runners](https://docs.github.com/en/actions/hosting-your-own-runners/managing-self-hosted-runners/adding-self-hosted-runners)
- [GitHub Docs: Using self-hosted runners in a workflow](https://docs.github.com/en/actions/hosting-your-own-runners/managing-self-hosted-runners/using-self-hosted-runners-in-a-workflow)
