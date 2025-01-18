-- Check the schema "New" exists
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'New')
BEGIN
    EXEC('CREATE SCHEMA New');
END

-- Drop tables in reverse dependency order
IF OBJECT_ID('New.Transactions', 'U') IS NOT NULL
    DROP TABLE New.Transactions;

IF OBJECT_ID('New.VouchersLines', 'U') IS NOT NULL
    DROP TABLE New.VouchersLines;

IF OBJECT_ID('New.Vouchers', 'U') IS NOT NULL
    DROP TABLE New.Vouchers;

IF OBJECT_ID('New.VoucherTypes', 'U') IS NOT NULL
    DROP TABLE New.VoucherTypes;

IF OBJECT_ID('New.ChartOfAccounts', 'U') IS NOT NULL
    DROP TABLE New.ChartOfAccounts;

IF OBJECT_ID('New.CostCenters', 'U') IS NOT NULL
    DROP TABLE New.CostCenters;

IF OBJECT_ID('New.CoATypes', 'U') IS NOT NULL
    DROP TABLE New.CoATypes;

-- Recreate tables
CREATE TABLE New.CoATypes(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL
);

CREATE TABLE New.CostCenters(
    Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ShortName NVARCHAR(10) NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    TrakingId NVARCHAR(100) NOT NULL,
    CreatedBy NVARCHAR(150) NOT NULL,
    CreatedOn DATETIME NOT NULL,
    UpdatedBy NVARCHAR(150) NULL,
    UpdatedOn DATETIME NULL,
    IsActive BIT NOT NULL
);

CREATE TABLE New.ChartOfAccounts(
    AccountCode VARCHAR(13) NOT NULL PRIMARY KEY,
    AccountName NVARCHAR(100) NOT NULL,
    ParentAccountCode VARCHAR(11) NOT NULL,
    Level INT NOT NULL,
    CoATypeId INT NOT NULL,
    IsRoot BIT NOT NULL,
    IsActive BIT NOT NULL,
    CreatedBy NVARCHAR(150) NOT NULL,
    CreatedOn DATETIME NOT NULL,
    UpdatedBy NVARCHAR(150) NULL,
    UpdatedOn DATETIME NULL,
    FOREIGN KEY (CoATypeId) REFERENCES New.CoATypes(Id)
);

CREATE TABLE New.VoucherTypes(
    Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ShortName NVARCHAR(10) NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    TrakingId NVARCHAR(100) NOT NULL,
    CreatedBy NVARCHAR(150) NOT NULL,
    CreatedOn DATETIME NOT NULL,
    UpdatedBy NVARCHAR(150) NULL,
    UpdatedOn DATETIME NULL,
    IsActive BIT NOT NULL
);

CREATE TABLE New.Vouchers(
    Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    VoucherNo NVARCHAR(30) NOT NULL,
    VoucherDate DATE NOT NULL,
    VoucherTypesId BIGINT NOT NULL,
    CostCentersId BIGINT NOT NULL,
    BankAccNo NVARCHAR(40) NULL,
    CheckNo NVARCHAR(12) NULL,
    IssueDate DATETIME NULL,
    TotalDebitAmount DECIMAL(18,3) NOT NULL,
    TotalCreditAmount DECIMAL(18,3) NOT NULL,
    Narration NVARCHAR(500) NULL,
    Status INT NOT NULL,
    TrakingId NVARCHAR(100) NOT NULL,
    CreatedBy NVARCHAR(150) NOT NULL,
    CreatedOn DATETIME NOT NULL,
    UpdatedBy NVARCHAR(150) NULL,
    UpdatedOn DATETIME NULL,
    IsActive BIT NOT NULL,
    FOREIGN KEY (VoucherTypesId) REFERENCES New.VoucherTypes(Id),
    FOREIGN KEY (CostCentersId) REFERENCES New.CostCenters(Id)
);

CREATE TABLE New.VouchersLines(
    Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    VouchersId BIGINT NOT NULL,
    AccountCode VARCHAR(13) NOT NULL,
    DebitAmount DECIMAL(18,3) NOT NULL,
    CreditAmount DECIMAL(18,3) NOT NULL,
    Particular NVARCHAR(250) NULL,
    AttachmentPath NVARCHAR(250) NULL,
    TrakingId NVARCHAR(100) NOT NULL,
    CreatedBy NVARCHAR(150) NOT NULL,
    CreatedOn DATETIME NOT NULL,
    UpdatedBy NVARCHAR(150) NULL,
    UpdatedOn DATETIME NULL,
    IsActive BIT NOT NULL,
    FOREIGN KEY (VouchersId) REFERENCES New.Vouchers(Id),
    FOREIGN KEY (AccountCode) REFERENCES New.ChartOfAccounts(AccountCode)
);

CREATE TABLE New.Transactions(
    Id VARCHAR(30) NOT NULL PRIMARY KEY,
    VouchersId BIGINT NOT NULL,
    AccountCode VARCHAR(13) NOT NULL,
    Description NVARCHAR(250) NULL,
    DebitAmount DECIMAL(18,3) NOT NULL,
    CreditAmount DECIMAL(18,3) NOT NULL,
    TransactionBy NVARCHAR(150) NOT NULL,
    TransactionDate DATETIME NOT NULL,
    FOREIGN KEY (VouchersId) REFERENCES New.Vouchers(Id),
    FOREIGN KEY (AccountCode) REFERENCES New.ChartOfAccounts(AccountCode)
);
