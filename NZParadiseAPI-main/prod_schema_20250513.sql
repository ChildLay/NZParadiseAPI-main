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
CREATE TABLE [PropertyTypes] (
    [PropertyTypeID] int NOT NULL IDENTITY,
    [Description] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_PropertyTypes] PRIMARY KEY ([PropertyTypeID])
);

CREATE TABLE [Users] (
    [UserID] int NOT NULL IDENTITY,
    [UserTypeID] int NOT NULL,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [AgencyName] nvarchar(120) NOT NULL,
    [PhoneNo] nvarchar(20) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Gender] int NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedDate] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserID])
);

CREATE TABLE [Properties] (
    [PropertyID] int NOT NULL IDENTITY,
    [UserID] int NOT NULL,
    [PropertyTypeID] int NOT NULL,
    [Description] nvarchar(300) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Bedroom] int NOT NULL,
    [Bathroom] int NOT NULL,
    [Parking] int NOT NULL,
    [Area] decimal(18,2) NOT NULL,
    [PropertyFeatureIDs] nvarchar(max) NOT NULL,
    [AvailableFromDate] datetime2 NULL,
    [Status] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedDate] datetime2 NULL,
    CONSTRAINT [PK_Properties] PRIMARY KEY ([PropertyID]),
    CONSTRAINT [FK_Properties_PropertyTypes_PropertyTypeID] FOREIGN KEY ([PropertyTypeID]) REFERENCES [PropertyTypes] ([PropertyTypeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Properties_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID]) ON DELETE CASCADE
);

CREATE INDEX [IX_Properties_PropertyTypeID] ON [Properties] ([PropertyTypeID]);

CREATE INDEX [IX_Properties_UserID] ON [Properties] ([UserID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250328080443_InitialCreate', N'9.0.3');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PropertyTypes]') AND [c].[name] = N'Description');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [PropertyTypes] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [PropertyTypes] ALTER COLUMN [Description] nvarchar(max) NOT NULL;

CREATE TABLE [PropertyFeatures] (
    [PropertyFeatureID] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PropertyFeatures] PRIMARY KEY ([PropertyFeatureID])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyFeatureID', N'Description') AND [object_id] = OBJECT_ID(N'[PropertyFeatures]'))
    SET IDENTITY_INSERT [PropertyFeatures] ON;
INSERT INTO [PropertyFeatures] ([PropertyFeatureID], [Description])
VALUES (1, N'Gas'),
(2, N'Washing Machine'),
(3, N'Dryer'),
(4, N'Electricity'),
(5, N'Cold Water'),
(6, N'Hot Water'),
(7, N'Air Conditioner'),
(8, N'Heater'),
(9, N'Swimming Pool'),
(10, N'GYM'),
(20, N'Other');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyFeatureID', N'Description') AND [object_id] = OBJECT_ID(N'[PropertyFeatures]'))
    SET IDENTITY_INSERT [PropertyFeatures] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyTypeID', N'Description') AND [object_id] = OBJECT_ID(N'[PropertyTypes]'))
    SET IDENTITY_INSERT [PropertyTypes] ON;
INSERT INTO [PropertyTypes] ([PropertyTypeID], [Description])
VALUES (1, N'Apartment'),
(2, N'Condominium'),
(3, N'Mansions'),
(4, N'Room'),
(5, N'Landed House'),
(6, N'Townhouse'),
(20, N'Other');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyTypeID', N'Description') AND [object_id] = OBJECT_ID(N'[PropertyTypes]'))
    SET IDENTITY_INSERT [PropertyTypes] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250328083306_PropertyType-Attachment-AuditLog', N'9.0.3');

CREATE TABLE [Attachments] (
    [AttachmentID] int NOT NULL IDENTITY,
    [Description] nvarchar(100) NOT NULL,
    [FilePath] nvarchar(max) NOT NULL,
    [ParentID] int NOT NULL,
    [ParentType] int NOT NULL,
    [UserID] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedDate] datetime2 NULL,
    CONSTRAINT [PK_Attachments] PRIMARY KEY ([AttachmentID])
);

CREATE TABLE [AuditLog] (
    [AuditLogID] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    [UserID] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedDate] datetime2 NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY ([AuditLogID])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250328083409_PropertyFeature-Attachment-AuditLog', N'9.0.3');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250328141527_UsersUpdate', N'9.0.3');

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'PhoneNo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [PhoneNo] nvarchar(20) NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'AgencyName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] ALTER COLUMN [AgencyName] nvarchar(120) NULL;

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Properties]') AND [c].[name] = N'PropertyFeatureIDs');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Properties] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Properties] ALTER COLUMN [PropertyFeatureIDs] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250328141722_NullableFields', N'9.0.3');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250503122254_WishlistTable', N'9.0.3');

CREATE TABLE [Wishlists] (
    [WishlistID] int NOT NULL IDENTITY,
    [PropertyID] int NOT NULL,
    [UserID] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Wishlists] PRIMARY KEY ([WishlistID]),
    CONSTRAINT [FK_Wishlists_Properties_PropertyID] FOREIGN KEY ([PropertyID]) REFERENCES [Properties] ([PropertyID]) ON DELETE CASCADE
);

CREATE INDEX [IX_Wishlists_PropertyID] ON [Wishlists] ([PropertyID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250503122844_WishlistTable-v2', N'9.0.3');

ALTER TABLE [Properties] DROP CONSTRAINT [FK_Properties_Users_UserID];

ALTER TABLE [Properties] ADD CONSTRAINT [FK_Properties_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID]);

CREATE TABLE [Messages] (
    [MessageID] int NOT NULL IDENTITY,
    [PropertyID] int NOT NULL,
    [FromUserID] int NOT NULL,
    [MessageDesc] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY ([MessageID]),
    CONSTRAINT [FK_Messages_Properties_PropertyID] FOREIGN KEY ([PropertyID]) REFERENCES [Properties] ([PropertyID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_Users_FromUserID] FOREIGN KEY ([FromUserID]) REFERENCES [Users] ([UserID])
);

CREATE INDEX [IX_Attachments_ParentID] ON [Attachments] ([ParentID]);

CREATE INDEX [IX_Messages_FromUserID] ON [Messages] ([FromUserID]);

CREATE INDEX [IX_Messages_PropertyID] ON [Messages] ([PropertyID]);

ALTER TABLE [Attachments] ADD CONSTRAINT [FK_Attachments_Properties_ParentID] FOREIGN KEY ([ParentID]) REFERENCES [Properties] ([PropertyID]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250506103804_Messages', N'9.0.3');

COMMIT;
GO

