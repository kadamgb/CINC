
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/31/2018 12:28:53
-- Generated from EDMX file: D:\gg\NQR.CINC.Entities\EntityFramework\CINCEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CINC];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Patient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Patient];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[CINCModelStoreContainer].[UserPatient]', 'U') IS NOT NULL
    DROP TABLE [CINCModelStoreContainer].[UserPatient];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Email] nvarchar(50)  NULL,
    [Phone] nvarchar(50)  NULL
);
GO

-- Creating table 'Patient'
CREATE TABLE [dbo].[Patient] (
    [Id] bigint  NOT NULL,
    [Salutation] nchar(5)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [Mobile] bigint  NULL,
    [DateOfBirth] datetime  NULL,
    [Weight] int  NULL,
    [Height] nchar(10)  NULL,
    [Sex] nchar(10)  NULL,
    [Address] nvarchar(max)  NULL,
    [AddharNumber] bigint  NULL,
    [Gaurdian] nvarchar(max)  NULL,
    [EmergencyContact] bigint  NULL,
    [IsActive] bit  NULL,
    [StartDate] datetime  NULL
);
GO

-- Creating table 'UserPatient'
CREATE TABLE [dbo].[UserPatient] (
    [UserId] bigint  NOT NULL,
    [PatientId] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Patient'
ALTER TABLE [dbo].[Patient]
ADD CONSTRAINT [PK_Patient]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId], [PatientId] in table 'UserPatient'
ALTER TABLE [dbo].[UserPatient]
ADD CONSTRAINT [PK_UserPatient]
    PRIMARY KEY CLUSTERED ([UserId], [PatientId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------