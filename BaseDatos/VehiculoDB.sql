USE [master]
GO
/****** Objeto: Database [DB_Vehiculo] Fecha de script: 1/9/2026 20:08:16 ******/
CREATE DATABASE [DB_Vehiculo]

GO

CREATE LOGIN [usuario_librosA]
WITH PASSWORD = 'admin',
CHECK_POLICY = OFF,
CHECK_EXPIRATION = OFF
GO

USE [DB_Vehiculo]
GO

CREATE USER [usuario_librosA] FOR LOGIN [usuario_librosA] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [usuario_librosA]
GO
ALTER ROLE [db_datareader] ADD MEMBER [usuario_librosA]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [usuario_librosA]
GO

/****** Objeto: Table [dbo].[CategoriaCache] Fecha de script: 1/9/2026 20:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriaCache](
	[Idcategoria] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Idcategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Vehiculo] Fecha de script: 1/9/2026 20:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehiculo](
	[IdVehiculo] [int] IDENTITY(1,1) NOT NULL,
	[idcategoria] [int] NOT NULL,
	[Marca] [varchar](100) NOT NULL,
	[precio] [decimal](18, 2) NOT NULL,
	[stock] [int] NOT NULL,
	[estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVehiculo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[CategoriaCache] ([Idcategoria], [Nombre]) VALUES (1, N'Sedán')
INSERT [dbo].[CategoriaCache] ([Idcategoria], [Nombre]) VALUES (2, N'SUV')
INSERT [dbo].[CategoriaCache] ([Idcategoria], [Nombre]) VALUES (3, N'Camioneta')
INSERT [dbo].[CategoriaCache] ([Idcategoria], [Nombre]) VALUES (4, N'cualquier')
INSERT [dbo].[CategoriaCache] ([Idcategoria], [Nombre]) VALUES (6, N'Motocicletas')
INSERT [dbo].[CategoriaCache] ([Idcategoria], [Nombre]) VALUES (7, N'star')
GO
SET IDENTITY_INSERT [dbo].[Vehiculo] ON 

INSERT [dbo].[Vehiculo] ([IdVehiculo], [idcategoria], [Marca], [precio], [stock], [estado]) VALUES (1, 1, N'Toyota Corolla', CAST(25000.00 AS Decimal(18, 2)), 15, 1)
INSERT [dbo].[Vehiculo] ([IdVehiculo], [idcategoria], [Marca], [precio], [stock], [estado]) VALUES (2, 2, N'Ford Explorer', CAST(45000.00 AS Decimal(18, 2)), 5, 1)
INSERT [dbo].[Vehiculo] ([IdVehiculo], [idcategoria], [Marca], [precio], [stock], [estado]) VALUES (3, 3, N'Chevrolet Silverado', CAST(55000.00 AS Decimal(18, 2)), 8, 1)
INSERT [dbo].[Vehiculo] ([IdVehiculo], [idcategoria], [Marca], [precio], [stock], [estado]) VALUES (4, 1, N'toyota nn', CAST(99999.00 AS Decimal(18, 2)), 99, 1)
INSERT [dbo].[Vehiculo] ([IdVehiculo], [idcategoria], [Marca], [precio], [stock], [estado]) VALUES (5, 0, N'audi', CAST(11111.00 AS Decimal(18, 2)), 11, 1)
INSERT [dbo].[Vehiculo] ([IdVehiculo], [idcategoria], [Marca], [precio], [stock], [estado]) VALUES (6, 1, N'Prueba Consumidor', CAST(15000.00 AS Decimal(18, 2)), 10, 1)
INSERT [dbo].[Vehiculo] ([IdVehiculo], [idcategoria], [Marca], [precio], [stock], [estado]) VALUES (7, 6, N'Yamaha', CAST(0.00 AS Decimal(18, 2)), 0, 0)
INSERT [dbo].[Vehiculo] ([IdVehiculo], [idcategoria], [Marca], [precio], [stock], [estado]) VALUES (8, 7, N'haiku', CAST(80000.00 AS Decimal(18, 2)), 10, 1)
SET IDENTITY_INSERT [dbo].[Vehiculo] OFF
GO
ALTER TABLE [dbo].[Vehiculo] ADD  DEFAULT ((1)) FOR [estado]
GO
USE [master]
GO
ALTER DATABASE [DB_Vehiculo] SET  READ_WRITE 
GO
