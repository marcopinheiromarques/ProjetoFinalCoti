CREATE TABLE [dbo].[Usuarios] (
    [IdUsuario]    INT           IDENTITY (1, 1) NOT NULL,
    [Login]        VARCHAR (50)  NOT NULL,
    [Email]        VARCHAR (100) NOT NULL,
    [Senha]        VARCHAR (150) NOT NULL,
    [DataCriacao]  DATETIME      NOT NULL,
    [AlterouSenha] BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([IdUsuario] ASC)
);

GO

CREATE TABLE [dbo].[Contatos] (
    [IdContato] INT           IDENTITY (1, 1) NOT NULL,
    [Nome]      VARCHAR (150) NOT NULL,
    [Email]     VARCHAR (150) NOT NULL,
    [Telefone]  VARCHAR (50)  NOT NULL,
    [IdUsuario] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([IdContato] ASC),
    FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuarios] ([IdUsuario])
);

GO

CREATE TABLE [dbo].[Tarefas] (
    [IdTarefa]    INT           IDENTITY (1, 1) NOT NULL,
    [Nome]        VARCHAR (255) NOT NULL,
    [DataEntrega] DATETIME      NOT NULL,
    [Descricao]   TEXT          NOT NULL,
    [IdUsuario]   INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([IdTarefa] ASC),
    FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuarios] ([IdUsuario])
);

GO

