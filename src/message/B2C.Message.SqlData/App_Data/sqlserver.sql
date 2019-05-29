USE [master]
GO
/****** Object:  Database [B2C_Message]    Script Date: 01/29/2019 11:19:53 ******/
CREATE DATABASE [b2c_Message] ;
GO
USE [b2c_Message]
GO
/****** Object:  Table [dbo].[mobile_code]    Script Date: 01/29/2019 11:19:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mobile_code](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AggregateId] [uniqueidentifier] NULL,
	[Mobile] [bigint] NULL,
	[UsageType] [tinyint] NULL,
	[UsageCode] [nvarchar](10) NULL,
	[ExpireTime] [datetime] NULL,
	[UsageStatus] [tinyint] NULL,
	[ClientIP] [nvarchar](15) NULL,
	[Platform] [tinyint] NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](20) NULL,
	[EditDate] [datetime] NULL,
	[Editor] [nvarchar](20) NULL,
	[Version] [int] NULL,
 CONSTRAINT [PK_mobile_code] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_CreateDate] ON [dbo].[mobile_code] 
(
	[CreateDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_Mobile] ON [dbo].[mobile_code] 
(
	[Mobile] ASC
)
INCLUDE ( [UsageType],
[ExpireTime]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [UNQ_AggregateId] ON [dbo].[mobile_code] 
(
	[AggregateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[email_code]    Script Date: 01/29/2019 11:19:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[email_code](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AggregateId] [uniqueidentifier] NULL,
	[Email] [nvarchar](50) NULL,
	[UsageType] [tinyint] NULL,
	[UsageCode] [nvarchar](10) NULL,
	[ExpireTime] [datetime] NULL,
	[UsageStatus] [tinyint] NULL,
	[ClientIP] [nvarchar](15) NULL,
	[Platform] [tinyint] NULL,
	[CreateDate] [datetime] NULL,
	[Creator] [nvarchar](20) NULL,
	[EditDate] [datetime] NULL,
	[Editor] [nvarchar](20) NULL,
	[Version] [int] NULL,
 CONSTRAINT [PK_email_code] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_Email] ON [dbo].[email_code] 
(
	[Email] ASC
)
INCLUDE ( [UsageType],
[ExpireTime]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [UNQ_AggregateId] ON [dbo].[email_code] 
(
	[AggregateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
