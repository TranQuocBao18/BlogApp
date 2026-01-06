IF SCHEMA_ID(N'communication') IS NULL EXEC(N'CREATE SCHEMA [communication];');

GO

-- PushMultiNotificationMessage
CREATE TABLE [communication].NotificationMessages (
    [Id] nvarchar(100) NOT NULL,
    [Title] NVARCHAR(500) NOT NULL,
    [ContentNotify] NVARCHAR(500) NULL,
    [ReferenceData] NVARCHAR(500) NULL,
    [TargetType] NVARCHAR(100),
    [NotificationType] NVARCHAR(100),

    [IsDeleted] bit NOT NULL DEFAULT 0,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModified] datetime2 NULL,
    CONSTRAINT [PK_NotificationMessages] PRIMARY KEY ([Id])
);

GO;

-- NotificationUsers
CREATE TABLE [communication].NotificationUsers (
    [Id] nvarchar(100) NOT NULL,
    [UserId] NVARCHAR(100) NOT NULL,
    [NotificationId] NVARCHAR(100) NOT NULL,
    [IsRead] bit NOT NULL DEFAULT 0,
    [ReadAt] datetime2 NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_NotificationUsers] PRIMARY KEY ([Id])
);

CREATE INDEX IDX_NotificationUsers_UserId ON [communication].NotificationUsers (UserId);
CREATE INDEX IDX_NotificationUsers_NotificationId ON [communication].NotificationUsers (NotificationId);

GO;