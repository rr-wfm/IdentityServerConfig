IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'IdentityServerConfig') IS NULL EXEC(N'CREATE SCHEMA [IdentityServerConfig];');
GO

CREATE TABLE [IdentityServerConfig].[AuditLog] (
    [ID] int NOT NULL IDENTITY,
    [UserId] nvarchar(max) NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    [Action] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY ([ID])
    );
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230516071604_InitialCreate', N'7.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [IdentityServerConfig].[AuditLog] ADD [UserName] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230516082321_AddedUserName', N'7.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [IdentityServerConfig].[AuditLog] ADD [Data] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230517145458_AddedDataField', N'7.0.5');
GO

COMMIT;
GO

