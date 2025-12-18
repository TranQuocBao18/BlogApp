IF SCHEMA_ID(N'Application') IS NULL EXEC(N'CREATE SCHEMA [Application];');

GO

CREATE TABLE [Application].[Category] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NOT NULL,
    [Slug] nvarchar(450) NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModified] datetime2 NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Application].[Banner] (
    [Id] nvarchar(450) NOT NULL,
    [Url] nvarchar(max) NOT NULL,
    [Width] FLOAT DEFAULT 0,
    [Height] FLOAT DEFAULT 0,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT [PK_Banner] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Application].[Blog] (
    [Id] nvarchar(450) NOT NULL,
    [CategoryId] nvarchar(450) NOT NULL,
    [BannerId] nvarchar(450) NOT NULL,
    [Tittle] nvarchar(256) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Slug] nvarchar(450) NULL,
    [IsPublished] bit NOT NULL DEFAULT 0,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModified] datetime2 NULL,
    CONSTRAINT [PK_Blog] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Blog_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Application].[Category] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Blog_Banner_BannerId] FOREIGN KEY ([BannerId]) REFERENCES [Application].[Banner] ([Id]) ON DELETE CASCADE
);

GO


CREATE TABLE [Application].[Tag] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NOT NULL,
    [IsDeleted] bit NOT NULL DEFAULT 0,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModified] datetime2 NULL,
    CONSTRAINT [PK_Tag] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Application].[BlogTag] (
    [Id] nvarchar(450) NOT NULL,
    [BlogId] nvarchar(450) NOT NULL,
    [TagId] nvarchar(450) NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModified] datetime2 NULL,
    CONSTRAINT [PK_BlogTag] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BlogTag_Blog_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Application].[Blog] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BlogTag_Tag_TagId] FOREIGN KEY ([TagId]) REFERENCES [Application].[Tag] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Application].[Comment] (
    [Id] nvarchar(450) NOT NULL,
    [BlogId] nvarchar(450) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [ParentId] nvarchar(450) NULL,
    [Content] nvarchar(max) NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [LastModifiedBy] nvarchar(max) NULL,
    [LastModified] datetime2 NULL,
    CONSTRAINT [PK_Comment] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comment_Blog_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Application].[Blog] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Comment_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Comment_Comment_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Application].[Comment] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Application].[Like] (
    [Id] nvarchar(450) NOT NULL,
    [BlogId] nvarchar(450) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [CommentId] nvarchar(450) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT [PK_Like] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Like_Blog_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Application].[Blog] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Like_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Like_Comment_CommentId] FOREIGN KEY ([CommentId]) REFERENCES [Application].[Comment] ([Id]) ON DELETE NO ACTION
);

GO