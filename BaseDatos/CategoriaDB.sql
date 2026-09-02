USE [master]
GO
/****** Objeto: Database [DB_Categoria] Fecha de script: 1/9/2026 20:51:24 ******/
CREATE DATABASE [DB_Categoria]
GO

CREATE LOGIN [usuario_inventariosA]
WITH PASSWORD = 'admin12',
CHECK_POLICY = OFF,
CHECK_EXPIRATION = OFF
GO

USE [DB_Categoria]
GO

CREATE USER [usuario_inventariosA] FOR LOGIN [usuario_inventariosA] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [usuario_inventariosA]
GO
ALTER ROLE [db_datareader] ADD MEMBER [usuario_inventariosA]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [usuario_inventariosA]
GO
/****** Objeto: Table [dbo].[Categoria] Fecha de script: 1/9/2026 20:51:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categoria](
	[Idcategoria] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](255) NULL,
	[estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Idcategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categoria] ADD  DEFAULT ((1)) FOR [estado]
GO
USE [master]
GO
ALTER DATABASE [DB_Categoria] SET  READ_WRITE 
GO
