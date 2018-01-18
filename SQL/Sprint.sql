SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sprint](
	[ID] [int] NULL,
	[TeamID] [int] NULL,
	[DateStart] [datetime] NULL,
	[DateEnd] [datetime] NULL
) ON [PRIMARY]

GO