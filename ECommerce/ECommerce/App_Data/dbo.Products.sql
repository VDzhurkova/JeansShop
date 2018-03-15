CREATE TABLE [dbo].[Products] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (MAX)   NOT NULL,
    [Price]       DECIMAL (18, 2) NULL,
    [Image]       VARCHAR(MAX)           NULL,
    [Category]    VARCHAR (MAX)   NULL,
    [Quantity]    INT             NULL,
    [InStock]     BIT             NOT NULL,
    [Description] VARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
);

