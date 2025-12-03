IF SCHEMA_ID(N'Identity') IS NULL EXEC(N'CREATE SCHEMA [Identity];');

GO

CREATE TABLE [Identity].[Role] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Code] nvarchar(100) NULL,
    [Description] VARCHAR(100) NULL,
    [CanDeleted] bit NOT NULL DEFAULT 0,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModified] datetime2 NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Identity].[User] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Code] nvarchar(100) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModified] datetime2 NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Identity].[RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Role] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Identity].[RefreshToken] (
    [Id] int NOT NULL IDENTITY,
    [Token] nvarchar(max) NULL,
    [Expires] datetime2 NOT NULL,
    [Created] datetime2 NOT NULL,
    [CreatedByIp] nvarchar(max) NULL,
    [Revoked] datetime2 NULL,
    [RevokedByIp] nvarchar(max) NULL,
    [ReplacedByToken] nvarchar(max) NULL,
    [ApplicationUserId] nvarchar(450) NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshToken_User_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Identity].[UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Identity].[UserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Identity].[UserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Role] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Identity].[UserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_RefreshToken_ApplicationUserId] ON [Identity].[RefreshToken] ([ApplicationUserId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [Identity].[Role] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_RoleClaims_RoleId] ON [Identity].[RoleClaims] ([RoleId]);

GO

CREATE INDEX [EmailIndex] ON [Identity].[User] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [Identity].[User] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

CREATE INDEX [IX_UserClaims_UserId] ON [Identity].[UserClaims] ([UserId]);

GO

CREATE INDEX [IX_UserLogins_UserId] ON [Identity].[UserLogins] ([UserId]);

GO

CREATE INDEX [IX_UserRoles_RoleId] ON [Identity].[UserRoles] ([RoleId]);

GO
