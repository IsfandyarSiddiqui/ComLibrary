CREATE TABLE [dbo].[Users] (
    [lastActionDate] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [userId]         INT           IDENTITY (1, 1) NOT NULL,
    [fullName]       NVARCHAR (64) NOT NULL,
    [preferedName]   NVARCHAR (32) NOT NULL,
    [password]       NVARCHAR (64) NOT NULL,
    [email]          NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([userId] ASC)
);

