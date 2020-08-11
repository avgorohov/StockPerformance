CREATE TABLE [dbo].[StockHistoricalDatas]
(
	[Id] INT IDENTITY NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [Price] DECIMAL(8, 2) NOT NULL, 
    [StockId] INT NOT NULL, 
    CONSTRAINT [PK_StockHistoricalDatas] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_StockHistoricalDatas_Stocks] FOREIGN KEY ([StockId]) REFERENCES [Stocks]([Id]) ON DELETE CASCADE
)
