
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/25/2014 14:26:30
-- Generated from EDMX file: E:\Robin\Desktop\BPMS\BPMS.Model\Model.edmx
-- --------------------------------------------------
CREATE DATABASE [BPMS]


SET QUOTED_IDENTIFIER OFF;
GO
USE [BPMS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DataDictionary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DataDictionary];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[IPBlacklist]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IPBlacklist];
GO
IF OBJECT_ID(N'[dbo].[MenuInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MenuInfo];
GO
IF OBJECT_ID(N'[dbo].[Organization]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Organization];
GO
IF OBJECT_ID(N'[dbo].[PurviewInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurviewInfo];
GO
IF OBJECT_ID(N'[dbo].[RoleInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleInfo];
GO
IF OBJECT_ID(N'[dbo].[RolePurview]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolePurview];
GO
IF OBJECT_ID(N'[dbo].[SysLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysLog];
GO
IF OBJECT_ID(N'[dbo].[SysLogDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysLogDetails];
GO
IF OBJECT_ID(N'[dbo].[SysLoginLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysLoginLog];
GO
IF OBJECT_ID(N'[dbo].[SystemExceptionLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemExceptionLog];
GO
IF OBJECT_ID(N'[dbo].[SystemInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemInfo];
GO
IF OBJECT_ID(N'[dbo].[TableId]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TableId];
GO
IF OBJECT_ID(N'[dbo].[UserInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfo];
GO
IF OBJECT_ID(N'[dbo].[UserPurview]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserPurview];
GO
IF OBJECT_ID(N'[dbo].[UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRole];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DataDictionary'
CREATE TABLE [dbo].[DataDictionary] (
    [ID] int  NOT NULL,
    [SystemId] int  NOT NULL,
    [DictType] int  NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Code] varchar(50)  NOT NULL,
    [ParentId] int  NOT NULL,
    [AllowEdit] bit  NOT NULL,
    [AllowDelete] bit  NOT NULL,
    [Remark] nvarchar(500)  NULL,
    [IsEnable] bit  NOT NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'Employee'
CREATE TABLE [dbo].[Employee] (
    [ID] int  NOT NULL,
    [Code] varchar(50)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Spell] nvarchar(200)  NULL,
    [Alias] nvarchar(50)  NULL,
    [Age] smallint  NOT NULL,
    [Birthday] datetime  NOT NULL,
    [Gender] char(2)  NOT NULL,
    [SubCompanyId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [DepartmentId] int  NOT NULL,
    [WorkgroupId] int  NOT NULL,
    [DutyId] int  NOT NULL,
    [IDCard] varchar(18)  NULL,
    [BankCode] varchar(50)  NULL,
    [Email] varchar(50)  NULL,
    [Mobile] varchar(50)  NULL,
    [ShortNumber] varchar(50)  NULL,
    [Telephone] varchar(50)  NULL,
    [OICQ] varchar(50)  NULL,
    [OfficePhone] varchar(50)  NULL,
    [OfficeZipCode] varchar(6)  NULL,
    [OfficeAddress] nvarchar(50)  NULL,
    [OfficeFax] nvarchar(50)  NULL,
    [EducationId] int  NULL,
    [School] nvarchar(200)  NULL,
    [GraduationDate] datetime  NULL,
    [Major] nvarchar(50)  NULL,
    [DegreeId] int  NOT NULL,
    [TitleId] int  NOT NULL,
    [TitleDate] datetime  NULL,
    [TitleLevelId] int  NOT NULL,
    [WorkingDate] varchar(50)  NULL,
    [JoinInDate] datetime  NULL,
    [HomeZipCode] varchar(6)  NULL,
    [HomeAddress] nvarchar(200)  NULL,
    [HomePhone] varchar(50)  NULL,
    [HomeFax] nvarchar(50)  NULL,
    [Province] varchar(50)  NULL,
    [City] varchar(50)  NULL,
    [Area] varchar(50)  NULL,
    [NativePlace] nvarchar(50)  NULL,
    [PartyId] int  NULL,
    [NationId] int  NULL,
    [NationalityId] int  NULL,
    [WorkingPropertyId] int  NULL,
    [Competency] varchar(200)  NULL,
    [EmergencyContact] varchar(50)  NULL,
    [IsDimission] bit  NULL,
    [DimissionDate] datetime  NULL,
    [DimissionCause] varchar(200)  NULL,
    [DimissionWhither] varchar(200)  NULL,
    [Remark] nvarchar(500)  NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'IPBlacklist'
CREATE TABLE [dbo].[IPBlacklist] (
    [ID] int  NOT NULL,
    [Category] int  NOT NULL,
    [StartIp] varchar(50)  NULL,
    [EndIp] varchar(50)  NULL,
    [Failuretime] datetime  NULL,
    [Remark] nvarchar(500)  NULL,
    [IsEnable] bit  NOT NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'MenuInfo'
CREATE TABLE [dbo].[MenuInfo] (
    [ID] int  NOT NULL,
    [SystemId] int  NOT NULL,
    [ParentId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [Category] int  NOT NULL,
    [PurviewId] int  NULL,
    [Icon] varbinary(max)  NULL,
    [IconUrl] nvarchar(200)  NULL,
    [NavigateUrl] nvarchar(200)  NULL,
    [FormName] nvarchar(200)  NULL,
    [IsSplit] bit  NOT NULL,
    [Remark] nvarchar(500)  NULL,
    [IsEnable] bit  NOT NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'Organization'
CREATE TABLE [dbo].[Organization] (
    [ID] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [ParentId] int  NOT NULL,
    [ShortName] nvarchar(50)  NULL,
    [Category] int  NOT NULL,
    [Manager] int  NULL,
    [AssistantManager] int  NULL,
    [OuterPhone] nvarchar(50)  NULL,
    [InnerPhone] nvarchar(50)  NULL,
    [Fax] nvarchar(50)  NULL,
    [Postalcode] varchar(6)  NULL,
    [Address] nvarchar(200)  NULL,
    [Web] nvarchar(200)  NULL,
    [Remark] nvarchar(500)  NULL,
    [IsEnable] bit  NOT NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'PurviewInfo'
CREATE TABLE [dbo].[PurviewInfo] (
    [ID] int  NOT NULL,
    [SystemId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Code] nvarchar(20)  NOT NULL,
    [ParentId] int  NOT NULL,
    [PurviewType] int  NOT NULL,
    [Remark] nvarchar(500)  NULL,
    [IsEnable] bit  NOT NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'RoleInfo'
CREATE TABLE [dbo].[RoleInfo] (
    [ID] int  NOT NULL,
    [SystemId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [Category] int  NOT NULL,
    [Remark] nvarchar(500)  NULL,
    [IsEnable] bit  NOT NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'RolePurview'
CREATE TABLE [dbo].[RolePurview] (
    [ID] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [PurviewId] int  NOT NULL
);
GO

-- Creating table 'SysLogDetails'
CREATE TABLE [dbo].[SysLogDetails] (
    [ID] int  NOT NULL,
    [SyslogId] int  NOT NULL,
    [FieldName] varchar(50)  NOT NULL,
    [FieldText] varchar(50)  NOT NULL,
    [NewValue] varchar(max)  NOT NULL,
    [OldValue] varchar(max)  NOT NULL,
    [Remark] varchar(200)  NULL
);
GO

-- Creating table 'SysLoginLog'
CREATE TABLE [dbo].[SysLoginLog] (
    [ID] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [SystemId] int  NOT NULL,
    [Account] varchar(50)  NOT NULL,
    [Status] int  NOT NULL,
    [IPAddress] varchar(50)  NOT NULL,
    [IPAddressName] varchar(200)  NOT NULL,
    [Remark] nvarchar(500)  NULL
);
GO

-- Creating table 'SystemExceptionLog'
CREATE TABLE [dbo].[SystemExceptionLog] (
    [ID] int  NOT NULL,
    [Source] nvarchar(200)  NULL,
    [Exception] nvarchar(200)  NULL,
    [Description] varchar(max)  NULL,
    [CreateDate] datetime  NOT NULL,
    [IPAddress] varchar(50)  NOT NULL,
    [IPAddressName] varchar(200)  NULL
);
GO

-- Creating table 'SystemInfo'
CREATE TABLE [dbo].[SystemInfo] (
    [ID] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [IsEnable] bit  NOT NULL,
    [Remark] nvarchar(500)  NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'TableId'
CREATE TABLE [dbo].[TableId] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UpdateTime] datetime  NOT NULL,
    [TableName] varchar(50)  NOT NULL,
    [CurrentId] int  NOT NULL
);
GO

-- Creating table 'UserInfo'
CREATE TABLE [dbo].[UserInfo] (
    [ID] int  NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [Account] nvarchar(50)  NOT NULL,
    [Password] varchar(50)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Spell] varchar(200)  NOT NULL,
    [Alias] nvarchar(50)  NULL,
    [RoleId] int  NOT NULL,
    [Gender] char(2)  NOT NULL,
    [Mobile] varchar(50)  NULL,
    [Telephone] varchar(50)  NULL,
    [Birthday] datetime  NULL,
    [Email] nvarchar(50)  NULL,
    [OICQ] varchar(50)  NULL,
    [DutyId] int  NOT NULL,
    [TitleId] int  NULL,
    [CompanyId] int  NOT NULL,
    [DepartmentId] int  NOT NULL,
    [WorkgroupId] int  NOT NULL,
    [ChangePasswordDate] datetime  NULL,
    [IPAddress] varchar(50)  NULL,
    [MACAddress] varchar(50)  NULL,
    [LogOnCount] int  NULL,
    [FirstVisit] datetime  NULL,
    [PreviousVisit] datetime  NULL,
    [LastVisit] datetime  NULL,
    [Remark] nvarchar(500)  NULL,
    [IsEnable] bit  NOT NULL,
    [SortIndex] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL,
    [ModifyDate] datetime  NOT NULL,
    [ModifyUserId] int  NOT NULL,
    [ModifyUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'UserPurview'
CREATE TABLE [dbo].[UserPurview] (
    [ID] int  NOT NULL,
    [UserId] int  NOT NULL,
    [PurviewId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [HasRight] bit  NOT NULL
);
GO

-- Creating table 'UserRole'
CREATE TABLE [dbo].[UserRole] (
    [ID] int  NOT NULL,
    [UserId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL
);
GO

-- Creating table 'SysLog'
CREATE TABLE [dbo].[SysLog] (
    [ID] int  NOT NULL,
    [OperationType] int  NOT NULL,
    [TableName] varchar(50)  NOT NULL,
    [BusinessName] varchar(50)  NOT NULL,
    [ObjectId] int  NOT NULL,
    [IPAddress] varchar(50)  NOT NULL,
    [IPAddressName] varchar(200)  NULL,
    [Remark] nvarchar(500)  NULL,
    [CreateDate] datetime  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateUserName] varchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'DataDictionary'
ALTER TABLE [dbo].[DataDictionary]
ADD CONSTRAINT [PK_DataDictionary]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [PK_Employee]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'IPBlacklist'
ALTER TABLE [dbo].[IPBlacklist]
ADD CONSTRAINT [PK_IPBlacklist]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'MenuInfo'
ALTER TABLE [dbo].[MenuInfo]
ADD CONSTRAINT [PK_MenuInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Organization'
ALTER TABLE [dbo].[Organization]
ADD CONSTRAINT [PK_Organization]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PurviewInfo'
ALTER TABLE [dbo].[PurviewInfo]
ADD CONSTRAINT [PK_PurviewInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RoleInfo'
ALTER TABLE [dbo].[RoleInfo]
ADD CONSTRAINT [PK_RoleInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RolePurview'
ALTER TABLE [dbo].[RolePurview]
ADD CONSTRAINT [PK_RolePurview]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SysLogDetails'
ALTER TABLE [dbo].[SysLogDetails]
ADD CONSTRAINT [PK_SysLogDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SysLoginLog'
ALTER TABLE [dbo].[SysLoginLog]
ADD CONSTRAINT [PK_SysLoginLog]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SystemExceptionLog'
ALTER TABLE [dbo].[SystemExceptionLog]
ADD CONSTRAINT [PK_SystemExceptionLog]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SystemInfo'
ALTER TABLE [dbo].[SystemInfo]
ADD CONSTRAINT [PK_SystemInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TableId'
ALTER TABLE [dbo].[TableId]
ADD CONSTRAINT [PK_TableId]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserInfo'
ALTER TABLE [dbo].[UserInfo]
ADD CONSTRAINT [PK_UserInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserPurview'
ALTER TABLE [dbo].[UserPurview]
ADD CONSTRAINT [PK_UserPurview]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [PK_UserRole]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SysLog'
ALTER TABLE [dbo].[SysLog]
ADD CONSTRAINT [PK_SysLog]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------