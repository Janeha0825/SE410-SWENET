use dotwiki
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[topic]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[topic]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[topichistory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[topichistory]
GO

CREATE TABLE [dbo].[topic] (
	[topicpk] [uniqueidentifier] NOT NULL ,
	[content] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[name] [char] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[updatedon] [datetime] NULL ,
	[wikiset] [char] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[topichistory] (
	[topichistorypk] [uniqueidentifier] NOT NULL ,
	[topicfk] [uniqueidentifier] NOT NULL ,
	[content] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[name] [char] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[updatedon] [datetime] NULL ,
	[wikiset] [char] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[topic] WITH NOCHECK ADD 
	CONSTRAINT [PK_topic] PRIMARY KEY  CLUSTERED 
	(
		[topicpk]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[topichistory] WITH NOCHECK ADD 
	CONSTRAINT [PK_topichistory] PRIMARY KEY  CLUSTERED 
	(
		[topichistorypk]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[topic] ADD 
	CONSTRAINT [DF_topic_topicpk] DEFAULT (newid()) FOR [topicpk]
GO

 CREATE  INDEX [topicwikiset] ON [dbo].[topic]([wikiset]) ON [PRIMARY]
GO

 CREATE  INDEX [topicname] ON [dbo].[topic]([name]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[topichistory] ADD 
	CONSTRAINT [DF_topichistory_topichistorypk] DEFAULT (newid()) FOR [topichistorypk]
GO

 CREATE  INDEX [topichistorytopicfk] ON [dbo].[topichistory]([topicfk]) ON [PRIMARY]
GO

 CREATE  INDEX [topichistorywikiset] ON [dbo].[topichistory]([wikiset]) ON [PRIMARY]
GO

