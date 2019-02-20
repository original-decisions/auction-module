begin tran
IF schema_id('auction') IS NULL
    EXECUTE('CREATE SCHEMA [auction]')

IF schema_id('dbo') IS NULL
    EXECUTE('CREATE SCHEMA [dbo]')
IF schema_id('AspNet') IS NULL
    EXECUTE('CREATE SCHEMA [AspNet]')
IF schema_id('conv') IS NULL
    EXECUTE('CREATE SCHEMA [conv]')
	IF schema_id('attach') IS NULL
    EXECUTE('CREATE SCHEMA [attach]')
	IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[auction].[AuctionBids]') AND type in (N'U'))
	begin
CREATE TABLE [auction].[AuctionBids] (
    [AuctionId] [int] NOT NULL,
    [UserId] [int] NOT NULL,
    [Value] [decimal](18, 2) NOT NULL,
    [Description] [nvarchar](max) NOT NULL,
	[EstimatedStartDate] [datetime] null,
	[EstimatedEndDate] [datetime] null,
    [IsSelected] [bit] NOT NULL,
    CONSTRAINT [PK_auction.AuctionBids] PRIMARY KEY ([AuctionId], [UserId])
)
CREATE INDEX [IX_AuctionId] ON [auction].[AuctionBids]([AuctionId])
CREATE INDEX [IX_UserId] ON [auction].[AuctionBids]([UserId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[auction].[Auctions]') AND type in (N'U'))
	begin
CREATE TABLE [auction].[Auctions] (
    [Id] [int] NOT NULL IDENTITY,
    [UserId] [int] NOT NULL,
    [StartDate] [datetime],
    [EndDate] [datetime],
    [Step] [decimal](18, 2),
    [InitialPrice] [decimal](18, 2),
    [AutoClosePrice] [decimal](18, 2),
    [AuctionTypeId] [int] NOT NULL,
    [ConversationId] [int],
    [Description] [nvarchar](max) NOT NULL,
    [StateId] [int] NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_auction.Auctions] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UserId] ON [auction].[Auctions]([UserId])
CREATE INDEX [ix_Auction_StartDateEndDate] ON [auction].[Auctions]([StartDate], [EndDate])
CREATE INDEX [IX_AuctionTypeId] ON [auction].[Auctions]([AuctionTypeId])
CREATE INDEX [IX_ConversationId] ON [auction].[Auctions]([ConversationId])
CREATE INDEX [IX_StateId] ON [auction].[Auctions]([StateId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[auction].[States]') AND type in (N'U'))
begin
CREATE TABLE [auction].[States] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_auction.States] PRIMARY KEY ([Id])
)
end

	
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[conv].[Conversations]') AND type in (N'U'))
	begin
CREATE TABLE [conv].[Conversations] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [ConversationTypeId] [int] NOT NULL,
    [UserStartedId] [int] NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_conv.Conversations] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ConversationTypeId] ON [conv].[Conversations]([ConversationTypeId])
CREATE INDEX [IX_UserStartedId] ON [conv].[Conversations]([UserStartedId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[conv].[ConversationTypes]') AND type in (N'U'))
	begin
CREATE TABLE [conv].[ConversationTypes] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_conv.ConversationTypes] PRIMARY KEY ([Id])
)
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[auction].[AuctionTypes]') AND type in (N'U'))
	begin
CREATE TABLE [auction].[AuctionTypes] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_auction.AuctionTypes] PRIMARY KEY ([Id])
)
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[Users]') AND type in (N'U'))
	begin
CREATE TABLE [AspNet].[Users] (
    [Id] [int] NOT NULL IDENTITY,
    [Rating] [decimal](18, 2) NOT NULL,
    [ProfilePicturePath] [nvarchar](max),
    [FirstName] [nvarchar](max),
    [LastName] [nvarchar](max),
    [Patronymic] [nvarchar](max),
    [DateUpdated] [datetime],
    [LastActivityDate] [datetime],
    [LastLogin] [datetime],
    [RemindInDays] [int] NOT NULL,
    [DateRegistration] [datetime] NOT NULL,
    [Description] [nvarchar](max),
    [Email] [nvarchar](256),
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max),
    [SecurityStamp] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEndDateUtc] [datetime],
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    [UserName] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_AspNet.Users] PRIMARY KEY ([Id])
)

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNet].[Users]([UserName])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserClaims]') AND type in (N'U'))
	begin
CREATE TABLE [AspNet].[UserClaims] (
    [Id] [int] NOT NULL IDENTITY,
    [UserId] [int] NOT NULL,
    [ClaimType] [nvarchar](max),
    [ClaimValue] [nvarchar](max),
    CONSTRAINT [PK_AspNet.UserClaims] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserClaims]([UserId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserLogins]') AND type in (N'U'))
	begin
CREATE TABLE [AspNet].[UserLogins] (
    [LoginProvider] [nvarchar](128) NOT NULL,
    [ProviderKey] [nvarchar](128) NOT NULL,
    [UserId] [int] NOT NULL,
    CONSTRAINT [PK_AspNet.UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey], [UserId])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserLogins]([UserId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserRoles]') AND type in (N'U'))
	begin
CREATE TABLE [AspNet].[UserRoles] (
    [UserId] [int] NOT NULL,
    [RoleId] [int] NOT NULL,
    CONSTRAINT [PK_AspNet.UserRoles] PRIMARY KEY ([UserId], [RoleId])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserRoles]([UserId])
CREATE INDEX [IX_RoleId] ON [AspNet].[UserRoles]([RoleId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[auction].[AuctionVideos]') AND type in (N'U'))
	begin
 CREATE TABLE [auction].[AuctionVideos] (
        [AuctionId] [int] NOT NULL,
        [VideoId] [int] NOT NULL,
        [Title] [nvarchar](max) NOT NULL,
        [Description] [nvarchar](max) NOT NULL,
        CONSTRAINT [PK_auction.AuctionVideos] PRIMARY KEY ([AuctionId], [VideoId])
    )
    CREATE INDEX [IX_AuctionId] ON [auction].[AuctionVideos]([AuctionId])
    CREATE INDEX [IX_VideoId] ON [auction].[AuctionVideos]([VideoId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[attach].[Attachments]') AND type in (N'U'))
	begin
CREATE TABLE [attach].[Attachments] (
    [Id] [int] NOT NULL IDENTITY,
    [AttachmentTypeId] [int] NOT NULL,
    [ExtensionId] [int] NOT NULL,
    [Content] [varbinary](max),
    [IsShared] [bit] NOT NULL,
    [PublicUri] [nvarchar](max),
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_attach.Attachments] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_AttachmentTypeId] ON [attach].[Attachments]([AttachmentTypeId])
CREATE INDEX [IX_ExtensionId] ON [attach].[Attachments]([ExtensionId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[attach].[AttachmentTypes]') AND type in (N'U'))
	begin
CREATE TABLE [attach].[AttachmentTypes] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_attach.AttachmentTypes] PRIMARY KEY ([Id])
)
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[attach].[Extensions]') AND type in (N'U'))
	begin
CREATE TABLE [attach].[Extensions] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_attach.Extensions] PRIMARY KEY ([Id])
)
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[auction].[AuctionSkills]') AND type in (N'U'))
	begin
    CREATE TABLE [auction].[AuctionSkills] (
        [CategoryId] [int] NOT NULL,
        [AuctionId] [int] NOT NULL,
        CONSTRAINT [PK_auction.AuctionSkills] PRIMARY KEY ([CategoryId], [AuctionId])
    )
    CREATE INDEX [IX_CategoryId] ON [auction].[AuctionSkills]([CategoryId])
    CREATE INDEX [IX_AuctionId] ON [auction].[AuctionSkills]([AuctionId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
	begin
CREATE TABLE [dbo].[Categories] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [IsApproved] [bit] NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Categories] PRIMARY KEY ([Id])
)
CREATE INDEX [ix_Categories_Name] ON [dbo].[Categories]([Name], [IsApproved])
CREATE INDEX [ix_Categories_IsApproved] ON [dbo].[Categories]([IsApproved])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[auction].[AuctionLayouts]') AND type in (N'U'))
	begin
CREATE TABLE [auction].[AuctionLayouts] (
        [AuctionId] [int] NOT NULL,
        [LayoutId] [int] NOT NULL,
        [Title] [nvarchar](max) NOT NULL,
        [Description] [nvarchar](max) NOT NULL,
        CONSTRAINT [PK_auction.AuctionLayouts] PRIMARY KEY ([AuctionId], [LayoutId])
    )
    CREATE INDEX [IX_AuctionId] ON [auction].[AuctionLayouts]([AuctionId])
    CREATE INDEX [IX_LayoutId] ON [auction].[AuctionLayouts]([LayoutId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[Roles]') AND type in (N'U'))
	begin
CREATE TABLE [AspNet].[Roles] (
    [Id] [int] NOT NULL IDENTITY,
    [InRoleId] [int],
    [Scope] [nvarchar](max),
    [Name] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_AspNet.Roles] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_InRoleId] ON [AspNet].[Roles]([InRoleId])
CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNet].[Roles]([Name])
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.AuctionBids_auction.Auctions_AuctionId')
		begin
ALTER TABLE [auction].[AuctionBids] ADD CONSTRAINT [FK_auction.AuctionBids_auction.Auctions_AuctionId] FOREIGN KEY ([AuctionId]) REFERENCES [auction].[Auctions] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.AuctionBids_AspNet.Users_UserId')
		begin
ALTER TABLE [auction].[AuctionBids] ADD CONSTRAINT [FK_auction.AuctionBids_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end 
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_conv.Conversations_conv.ConversationTypes_ConversationTypeId')
		begin
		ALTER TABLE [conv].[Conversations] ADD CONSTRAINT [FK_conv.Conversations_conv.ConversationTypes_ConversationTypeId] FOREIGN KEY ([ConversationTypeId]) REFERENCES [conv].[ConversationTypes] ([Id])
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_conv.Conversations_AspNet.Users_UserStartedId')
		begin
ALTER TABLE [conv].[Conversations] ADD CONSTRAINT [FK_conv.Conversations_AspNet.Users_UserStartedId] FOREIGN KEY ([UserStartedId]) REFERENCES [AspNet].[Users] ([Id])
end
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.Auctions_auction.AuctionTypes_AuctionTypeId')
		begin
ALTER TABLE [auction].[Auctions] ADD CONSTRAINT [FK_auction.Auctions_auction.AuctionTypes_AuctionTypeId] FOREIGN KEY ([AuctionTypeId]) REFERENCES [auction].[AuctionTypes] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.Auctions_AspNet.Users_UserId')
		begin
ALTER TABLE [auction].[Auctions] ADD CONSTRAINT [FK_auction.Auctions_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserClaims_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserClaims] ADD CONSTRAINT [FK_AspNet.UserClaims_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserLogins_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserLogins] ADD CONSTRAINT [FK_AspNet.UserLogins_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserRoles_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserRoles] ADD CONSTRAINT [FK_AspNet.UserRoles_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserRoles_AspNet.Roles_RoleId')
		begin
ALTER TABLE [AspNet].[UserRoles] ADD CONSTRAINT [FK_AspNet.UserRoles_AspNet.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNet].[Roles] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.AuctionVideos_auction.Auctions_AuctionId')
		begin
ALTER TABLE [auction].[AuctionVideos] ADD CONSTRAINT [FK_auction.AuctionVideos_auction.Auctions_AuctionId] FOREIGN KEY ([AuctionId]) REFERENCES [auction].[Auctions] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.AuctionVideos_attach.Attachments_VideoId')
		begin
	ALTER TABLE [auction].[AuctionVideos] ADD CONSTRAINT [FK_auction.AuctionVideos_attach.Attachments_VideoId] FOREIGN KEY ([VideoId]) REFERENCES [attach].[Attachments] ([Id])
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_attach.Attachments_attach.AttachmentTypes_AttachmentTypeId')
		begin
ALTER TABLE [attach].[Attachments] ADD CONSTRAINT [FK_attach.Attachments_attach.AttachmentTypes_AttachmentTypeId] FOREIGN KEY ([AttachmentTypeId]) REFERENCES [attach].[AttachmentTypes] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_attach.Attachments_attach.Extensions_ExtensionId')
		begin
ALTER TABLE [attach].[Attachments] ADD CONSTRAINT [FK_attach.Attachments_attach.Extensions_ExtensionId] FOREIGN KEY ([ExtensionId]) REFERENCES [attach].[Extensions] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.AuctionSkills_auction.Auctions_AuctionId')
		begin
ALTER TABLE [auction].[AuctionSkills] ADD CONSTRAINT [FK_auction.AuctionSkills_auction.Auctions_AuctionId] FOREIGN KEY ([AuctionId]) REFERENCES [auction].[Auctions] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.AuctionSkills_dbo.Categories_CategoryId')
		begin
	ALTER TABLE [auction].[AuctionSkills] ADD CONSTRAINT [FK_auction.AuctionSkills_dbo.Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.AuctionLayouts_auction.Auctions_AuctionId')
		begin
ALTER TABLE [auction].[AuctionLayouts] ADD CONSTRAINT [FK_auction.AuctionLayouts_auction.Auctions_AuctionId] FOREIGN KEY ([AuctionId]) REFERENCES [auction].[Auctions] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.AuctionLayouts_attach.Attachments_LayoutId')
		begin
	ALTER TABLE [auction].[AuctionLayouts] ADD CONSTRAINT [FK_auction.AuctionLayouts_attach.Attachments_LayoutId] FOREIGN KEY ([LayoutId]) REFERENCES [attach].[Attachments] ([Id])
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.Roles_AspNet.Roles_InRoleId')
		begin
ALTER TABLE [AspNet].[Roles] ADD CONSTRAINT [FK_AspNet.Roles_AspNet.Roles_InRoleId] FOREIGN KEY ([InRoleId]) REFERENCES [AspNet].[Roles] ([Id])
end 
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_auction.Auctions_auction.States_StateId')
		begin
ALTER TABLE [auction].[Auctions] ADD CONSTRAINT [FK_auction.Auctions_auction.States_StateId] FOREIGN KEY ([StateId]) REFERENCES [auction].[States] ([Id])
end
commit tran