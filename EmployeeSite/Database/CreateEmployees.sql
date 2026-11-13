IF DB_ID('EmployeeDb') IS NULL CREATE DATABASE EmployeeDb;
GO
USE EmployeeDb;
GO
IF OBJECT_ID('dbo.Employees') IS NULL
BEGIN
    CREATE TABLE dbo.Employees (
        EmployeeId   INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeName NVARCHAR(100) NOT NULL,
        Age          INT NOT NULL
    );

    INSERT INTO dbo.Employees (EmployeeName, Age) VALUES
        ('John Smith', 34),
        ('Jane Doe', 28),
        ('Michael Brown', 45),
        ('Emily Davis', 31);
END
GO
