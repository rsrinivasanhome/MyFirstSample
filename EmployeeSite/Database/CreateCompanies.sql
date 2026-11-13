USE EmployeeDb;
GO
IF OBJECT_ID('dbo.Companies') IS NULL
BEGIN
    CREATE TABLE dbo.Companies (
        CompanyId   INT NOT NULL PRIMARY KEY,
        CompanyName NVARCHAR(150) NOT NULL,
        City        NVARCHAR(100) NULL
    );
END
GO
