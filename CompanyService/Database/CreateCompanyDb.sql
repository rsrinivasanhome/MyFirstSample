IF DB_ID('CompanyDb') IS NULL CREATE DATABASE CompanyDb;
GO
USE CompanyDb;
GO
IF OBJECT_ID('dbo.Companies') IS NULL
BEGIN
    CREATE TABLE dbo.Companies (
        CompanyId   INT IDENTITY(1,1) PRIMARY KEY,
        CompanyName NVARCHAR(150) NOT NULL,
        City        NVARCHAR(100) NULL
    );

    INSERT INTO dbo.Companies (CompanyName, City) VALUES
        ('Contoso Ltd', 'Seattle'),
        ('Fabrikam Inc', 'New York'),
        ('Northwind Traders', 'Chicago');
END
GO
