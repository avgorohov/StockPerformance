CREATE TABLE [dbo].[Stocks]
(
	[Id] INT IDENTITY NOT NULL, 
    [Symbol] NVARCHAR(20) NOT NULL, 
    CONSTRAINT [PK_Stocks] PRIMARY KEY ([Id]) 
)

GO

CREATE INDEX [IX_Stocks_Symbol] ON [dbo].[Stocks] ([Symbol])
