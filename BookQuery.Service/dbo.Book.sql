CREATE TABLE [dbo].[Book] (
    [Id]          INT            NOT NULL,
    [Title]       NVARCHAR (50)  NULL,
    [Description] NVARCHAR (100) NULL,
    [Author]      NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Table_2] PRIMARY KEY CLUSTERED ([Id] ASC)
);