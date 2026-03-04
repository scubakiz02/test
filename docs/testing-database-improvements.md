# Database Testing Improvements

This document outlines issues with the current database-related test implementation and provides a roadmap for addressing them.

---

## Summary of Priorities

| Priority | Issue | Impact | Effort |
|----------|-------|--------|--------|
| **High** | Test interdependency on shared state | Tests fail randomly based on execution order | Medium |
| **High** | No transaction rollback | Database becomes polluted with test data | Low |
| **High** | No reproducible test database setup | Tests fail on other dev machines; no CI/CD capability | Medium |
| **High** | CI/CD database unavailability | All database tests fail in pipelines; no automated test gates on PRs | Medium-High |
| **Medium** | Hardcoded primary keys | Tests break if database records change | Medium |
| **Low** | Missing failure scenario tests | Incomplete coverage of error paths | Low |

---

## Current Issues

### 1. Test Interdependency on Shared State
Tests modify database records and expect other tests to revert those changes. Example from `GeneralTests.vb`:

```vb
' ExecuteSqlParamQuery2 modifies record with id=4
' ExecuteSqlParamQuery3 must run after to revert the change
' If test order changes, tests fail
```

### 2. No Transaction Rollback
Tests execute real INSERT, UPDATE, and DELETE statements without rolling back changes. Failed tests leave the database in an inconsistent state.

### 3. Hardcoded Primary Keys
Tests reference specific database records by primary key (e.g., `AreaKey=82`, `id=4`). If these records are modified or deleted, tests fail.

### 4. No Reproducible Test Database Setup
The test database (`SatiTest`) is hosted locally on individual dev machines. This creates issues:
- New developers cannot run tests without manual database setup
- Test data may drift between machines over time
- No documentation or scripts exist to recreate the expected test state

### 5. Missing Failure Scenario Tests
No tests verify behavior when:
- Database connection fails
- Queries timeout
- Deadlocks occur

### 6. CI/CD Database Unavailability

The test database connection string in `web.config` references a SQL Server instance hosted on a single developer's machine. This creates critical CI/CD failures:

**Connection String Problem:**
- Current connection string points to a dev-machine-hosted SQL Server instance
- CI/CD runners (GitHub Actions, Azure DevOps agents) cannot resolve or reach this server
- Tests fail immediately with `SqlException: A network-related or instance-specific error`

**Affected Test Scope:**
- `GeneralTests.vb` - All `ExecuteSqlParamQuery*` and `GetMyDataSetParamQuery*` tests
- `PhaseControllerTests.vb` - All tests querying `T_Area`, `T_Phase`, `T_Input` tables
- `PmInputTests.vb` - Tests dependent on checklist data
- Essentially ALL database integration tests (estimated 80%+ of test suite)

**CI/CD Implications:**
- **No PR test gates:** Pull requests cannot require passing tests
- **No regression detection:** Breaking changes merge without automated verification
- **Manual testing burden:** All verification falls on developers running tests locally
- **Environment drift:** CI/CD and local environments behave differently

**Network/Security Constraints:**
- Dev machine is behind corporate firewall; not exposed to internet
- Even if exposed, CI runners use ephemeral IPs that cannot be whitelisted
- Embedding dev machine credentials in CI/CD secrets is a security risk
- Dev machine may be offline (weekends, maintenance, power outages)

**Current Workarounds (all inadequate):**
- Skip database tests in CI/CD → defeats purpose of automated testing
- Self-hosted runners on same network → adds infrastructure overhead
- VPN tunnels from CI runners → complex, fragile, security concerns

---

## Roadmap

### Phase 1: Transaction Wrapper (High Priority, Low Effort)

**Goal:** Prevent tests from permanently modifying database state.

**Implementation:**

1. Create a base test class with transaction support:

```vb
Imports System.Transactions
Imports Xunit

Public MustInherit Class DatabaseTestBase
    Implements IDisposable

    Private _transactionScope As TransactionScope

    Protected Sub New()
        _transactionScope = New TransactionScope(
            TransactionScopeOption.Required,
            New TransactionOptions With {
                .IsolationLevel = IsolationLevel.ReadCommitted,
                .Timeout = TimeSpan.FromMinutes(5)
            }
        )
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No Complete() call = automatic rollback
        _transactionScope?.Dispose()
    End Sub
End Class
```

2. Update test classes to inherit from `DatabaseTestBase`:

```vb
Public Class SecurityTests
    Inherits DatabaseTestBase

    <Fact>
    Public Sub ExecuteSqlParamQuery_UpdateRecord_ReturnsSuccess()
        ' Changes automatically roll back after test
    End Sub
End Class
```

**Files to modify:**
- Create: `SatiDotNet2Tests/DatabaseTestBase.vb`
- Update: `GeneralTests.vb`, `PhaseControllerTests.vb`, `PmInputTests.vb`

---

### Phase 2: Remove Test Interdependencies (High Priority, Medium Effort)

**Goal:** Each test should be completely independent.

**Implementation:**

1. Identify tests that depend on other tests:
   - `ExecuteSqlParamQuery2` / `ExecuteSqlParamQuery3`
   - `GetMyDataSetParamQueryTests` (depends on id=4 state)

2. Refactor to self-contained tests:

```vb
<Fact>
Public Sub ExecuteSqlParamQuery_UpdatePassword_ReturnsSuccess()
    ' Arrange - create isolated test record
    Dim testId = CreateTestRecord("test_user", "original_password")

    ' Act
    Dim result = Security.ExecuteSqlParamQuery(
        "UPDATE [SatiTest].[dbo].[T_LogSqlInjectionPrevention] SET password=@password WHERE id=@id",
        BuildQueryObject(testId, "new_password")
    )

    ' Assert
    Assert.True(result("Success"))

    ' Cleanup handled by transaction rollback
End Sub
```

3. Create test data helper methods:

```vb
Protected Function CreateTestRecord(username As String, password As String) As Integer
    ' Insert and return new ID
End Function

Protected Function BuildQueryObject(id As Integer, password As String) As Dictionary(Of String, Dictionary(Of String, String))
    ' Build parameterized query object
End Function
```

**Files to modify:**
- `GeneralTests.vb` - refactor `ExecuteSqlParamQuery*` and `GetMyDataSetParamQuery*` tests

---

### Phase 3: Replace Hardcoded Keys (Medium Priority, Medium Effort)

**Goal:** Tests should not rely on specific database record IDs.

**Implementation:**

1. Create test data constants with descriptive names:

```vb
Public Module TestConstants
    ' Area Keys
    Public Const AREA_KEY_EDG_MONTHLY As Integer = 82
    Public Const AREA_KEY_DUMMY_CHECKLIST As Integer = 75
    Public Const AREA_KEY_SCISSOR_LIFT As Integer = 86

    ' Phase Keys
    Public Const PHASE_KEY_BEFORE_RUNNING As Integer = 1
    Public Const PHASE_KEY_OPERATING_TEMP As Integer = 2
    Public Const PHASE_KEY_AFTER_OFF As Integer = 3
End Module
```

2. For dynamic data, create records at test start:

```vb
<Fact>
Public Sub GetPhase_AllInputsFilled_ReturnsLastPhase()
    ' Arrange - create test area with phases
    Dim areaKey = CreateTestArea("Test Checklist")
    Dim phase1Key = CreateTestPhase(areaKey, "Phase 1", 1)
    Dim phase2Key = CreateTestPhase(areaKey, "Phase 2", 2)

    ' ... test logic using dynamic keys
End Sub
```

**Files to modify:**
- Create: `SatiDotNet2Tests/TestConstants.vb`
- Update: `PhaseControllerTests.vb`, `GeneralTests.vb`

---

### Phase 4: Reproducible Test Database Setup (Medium Priority, Medium Effort)

**Goal:** Enable any developer to run tests by providing scripts to create and seed the local test database.

**Current State:** The test database (`SatiTest`) is hosted locally on individual dev machines with no documented setup process.

**Implementation:**

1. Create database schema script:

```sql
-- scripts/create-test-database.sql
CREATE DATABASE SatiTest;
GO

USE SatiTest;
GO

CREATE TABLE T_LogSqlInjectionPrevention (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100),
    password NVARCHAR(100),
    fullname NVARCHAR(100),
    willitnull NVARCHAR(50)
);
-- ... other required tables
```

2. Create seed data script with expected test records:

```sql
-- scripts/seed-test-database.sql
USE SatiTest;
GO

-- Required test data (referenced by hardcoded IDs in tests)
SET IDENTITY_INSERT T_LogSqlInjectionPrevention ON;

INSERT INTO T_LogSqlInjectionPrevention (id, username, password, fullname)
VALUES
    (1, 'jork-frol-pliy', 'jxCv7$LEM!nuWcUb', 'john doe'),
    (2, 'seck-hor-zup', 'zcKbRwe+5Nk9k&gY', 'karen smith'),
    (3, 'test-user-3', 'SxhNFEsp$A!m7Bx4', 'test user 3'),
    (4, 'benk-sef-rhid', 'R)y+j%Lg28petjgN', 'tim hughes');

SET IDENTITY_INSERT T_LogSqlInjectionPrevention OFF;
GO
```

3. Create setup documentation in README or dedicated doc:

```markdown
## Test Database Setup

1. Run `scripts/create-test-database.sql` to create schema
2. Run `scripts/seed-test-database.sql` to populate test data
3. Verify connection string in `SatiDotNet2Tests/app.config`
```

4. (Optional) Create PowerShell setup script:

```powershell
# scripts/setup-test-db.ps1
param(
    [string]$ServerInstance = "localhost"
)

sqlcmd -S $ServerInstance -i "create-test-database.sql"
sqlcmd -S $ServerInstance -i "seed-test-database.sql"
Write-Host "Test database setup complete."
```

**Files to create:**
- `scripts/create-test-database.sql`
- `scripts/seed-test-database.sql`
- `scripts/setup-test-db.ps1` (optional)
- Update: `README.md` or create `docs/test-setup.md`

#### CI/CD Database Strategy

Since the test database is hosted on a dev machine and inaccessible from CI/CD runners, implement one of these approaches:

**Option A: SQL Server LocalDB (Recommended for simplicity)**
- Ships with Visual Studio Build Tools (available on GitHub Actions `windows-latest`)
- Lightweight, no Docker required
- Connection string: `Server=(localdb)\MSSQLLocalDB;Database=SatiTest;Integrated Security=true`
- CI/CD workflow runs `create-test-database.sql` and `seed-test-database.sql` before tests

**Option B: Docker SQL Server Container**
- Use `mcr.microsoft.com/mssql/server:2019-latest` in CI/CD pipeline
- More production-like environment
- Requires Docker support on CI runner
- Adds ~30-60 seconds container startup time

**Option C: Separate Integration Test Stage**
- Unit tests (mocked database) run in CI/CD on every PR
- Integration tests (real database) run on self-hosted runner or nightly schedule
- Requires refactoring tests into two categories

**Option D: Environment-Aware Connection Strings**

```vb
Public Shared Function GetTestConnectionString() As String
    Dim ciEnvironment = Environment.GetEnvironmentVariable("CI")

    If Not String.IsNullOrEmpty(ciEnvironment) Then
        ' CI/CD: Use LocalDB
        Return "Server=(localdb)\MSSQLLocalDB;Database=SatiTest;Integrated Security=true"
    Else
        ' Local dev: Use configured server
        Return ConfigurationManager.ConnectionStrings("SatiTest").ConnectionString
    End If
End Function
```

**Recommended CI/CD Workflow (GitHub Actions example):**

```yaml
name: Test Suite

on:
  pull_request:
    branches: [main]
  push:
    branches: [main]

jobs:
  test:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup LocalDB
        run: SqlLocalDB.exe create MSSQLLocalDB -s

      - name: Create Test Database
        run: sqlcmd -S "(localdb)\MSSQLLocalDB" -i scripts/create-test-database.sql

      - name: Seed Test Data
        run: sqlcmd -S "(localdb)\MSSQLLocalDB" -i scripts/seed-test-database.sql

      - name: Run Tests
        run: dotnet test
        env:
          CI: true
```

**Additional Files to create for CI/CD:**
- `.github/workflows/test.yml` (or Azure DevOps equivalent)
- `SatiDotNet2Tests/TestConfiguration.vb` (environment-aware connection logic)

---

### Phase 5: Error Scenario Tests (Low Priority, Low Effort)

**Goal:** Verify application handles database failures gracefully.

**Implementation:**

1. Add connection failure tests:

```vb
<Fact>
Public Sub ExecuteSqlParamQuery_InvalidConnectionString_ReturnsFailure()
    Dim badSecurity = New Security("Server=invalid;Database=fake;")
    Dim result = badSecurity.ExecuteSqlParamQuery("SELECT 1", New Dictionary(...))

    Assert.False(result("Success"))
    Assert.Contains("connection", result("Message").ToLower())
End Sub
```

2. Add timeout tests:

```vb
<Fact>
Public Sub ExecuteSqlParamQuery_LongRunningQuery_TimesOut()
    ' Test with WAITFOR DELAY or similar
End Sub
```

3. Add invalid query tests:

```vb
<Fact>
Public Sub GetMyDataSetParamQuery_InvalidTable_ReturnsNothing()
    Dim result = Security.GetMyDataSetParamQuery(
        "SELECT * FROM NonExistentTable",
        New Dictionary(...)
    )
    Assert.Equal(Nothing, result)
End Sub
```

**Files to modify:**
- `GeneralTests.vb` - add new test class `DatabaseErrorTests`

---

## Implementation Timeline

```
Week 1-2:   Phase 1 - Transaction Wrapper
Week 3-4:   Phase 2 - Remove Test Interdependencies
Week 5-6:   Phase 3 - Replace Hardcoded Keys
Week 7-8:   Phase 4a - Create database schema/seed scripts
Week 9-10:  Phase 4b - Implement CI/CD database provisioning
Week 11-12: Phase 4c - Environment-aware connection strings + CI workflow
Week 13:    Phase 5 - Error Scenario Tests
```

---

## Success Metrics

- [ ] All tests pass when run in any order
- [ ] All tests pass when run in parallel
- [ ] Database state unchanged after test suite completion
- [ ] No hardcoded primary keys in test assertions
- [ ] Test suite runs against isolated test database
- [ ] Error scenarios have >80% coverage
- [ ] Tests pass in CI/CD pipeline (GitHub Actions / Azure DevOps)
- [ ] Database provisioning is automated in CI/CD workflow
- [ ] Connection string selection is environment-aware (CI vs local)
- [ ] PR merge requires passing test suite

---

## References

- [xUnit Documentation](https://xunit.net/docs/getting-started/netfx/visual-studio)
- [TransactionScope Class](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope)
- [Database Testing Best Practices](https://docs.microsoft.com/en-us/ef/core/testing/)
