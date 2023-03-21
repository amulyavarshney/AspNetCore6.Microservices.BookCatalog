CREATE TABLE [dbo].[Book] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (100) NULL,
    [Author]      NVARCHAR (50)  NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_Book_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED ([Id] ASC)
);