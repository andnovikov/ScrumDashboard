SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ScrumTask](
	[ID] [int] NULL,
	[ExternalID] [int] NULL,
	[Title] [varchar](255) NULL,
	[Description] [varchar](500) NULL,
	[Category] [varchar](255) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO