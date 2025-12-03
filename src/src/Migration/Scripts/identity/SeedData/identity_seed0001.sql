--Roles
INSERT INTO [identity].[Role] (Id, Name, NormalizedName, ConcurrencyStamp, Description, CanDeleted, IsDeleted, CreatedBy, Created, LastModifiedBy, LastModified)
VALUES(N'fabe2807-af78-468a-aeb7-a5ffe9045a91', N'SuperAdmin', N'SUPERADMIN', N'1bf25b8f-509a-4ba3-a1cb-a55e0a1d43a0', N'', 0, 0, NULL, '2025-10-30 11:18:23.427', NULL, '2025-10-30 11:18:23.526');

INSERT INTO [identity].[Role] (Id, Name, NormalizedName, ConcurrencyStamp, Description, CanDeleted, IsDeleted, CreatedBy, Created, LastModifiedBy, LastModified)
VALUES(N'fff5dc7d-f568-469c-9202-c97bae822b49', N'Member', N'MEMBER', NULL, N'', 0, 0, NULL, '2025-10-30 11:19:23.321', NULL, '2025-10-30 11:19:23.415');

--Super Admin
INSERT INTO [identity].[User]
(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, IsDeleted, CreatedBy, Created, LastModifiedBy, LastModified)
VALUES(N'9ffa6354-81a7-4226-8628-a2ca6a47f907', N'superadmin', N'SUPERADMIN', N'superadmin@gmail.com', N'SUPERADMIN@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEN52yYgVqWirnx5Gotn4sP/sgtJlHrUYh47MumVG6XhKSjAIXhLd85Ap7uhH57VzsA==', N'P2KZ7ADZKEB7BP6EH7B2ZDNV7T7KYKLV', N'a0841550-48e1-4a3f-9ab3-6e4513fa60f6', NULL, 1, 0, NULL, 0, 0, N'TranBao', N'Super Administrator', 0, NULL, '2025-10-30 11:20:23.426', NULL, '2025-10-30 11:20:23.428');

INSERT INTO [identity].UserRoles
(UserId, RoleId)
VALUES(N'9ffa6354-81a7-4226-8628-a2ca6a47f907', N'fabe2807-af78-468a-aeb7-a5ffe9045a91');

INSERT INTO [identity].UserRoles
(UserId, RoleId)
VALUES(N'9ffa6354-81a7-4226-8628-a2ca6a47f907', N'fff5dc7d-f568-469c-9202-c97bae822b49');

--Member
INSERT INTO [identity].[User]
(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, IsDeleted, CreatedBy, Created, LastModifiedBy, LastModified)
VALUES(N'2af6e62f-ea68-4492-8502-faa44fee5daa', N'member', N'MEMBER', N'member@gmail.com', N'MEMBER@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEN52yYgVqWirnx5Gotn4sP/sgtJlHrUYh47MumVG6XhKSjAIXhLd85Ap7uhH57VzsA==', N'KB2JMIWJQ7MLUIFRIE3753COMLD4N2A6', N'8b0badce-ed53-4306-ab7a-f7cbeae50882', NULL, 1, 0, NULL, 0, 0, N'Basic', N'User', 0, NULL, '2025-10-30 11:22:24.185', NULL, '2025-10-30 11:22:24.185');

INSERT INTO [identity].UserRoles
(UserId, RoleId)
VALUES(N'2af6e62f-ea68-4492-8502-faa44fee5daa', N'fff5dc7d-f568-469c-9202-c97bae822b49');