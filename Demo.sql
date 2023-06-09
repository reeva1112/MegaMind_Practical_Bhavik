USE [Demo]
GO
/****** Object:  Table [dbo].[tblCity]    Script Date: 4/30/2023 10:37:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCity](
	[Id] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[CityName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblState]    Script Date: 4/30/2023 10:37:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblState](
	[Id] [int] NOT NULL,
	[StateName] [varchar](50) NULL,
 CONSTRAINT [PK_tblState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserRegistration]    Script Date: 4/30/2023 10:37:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserRegistration](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Address] [varchar](2000) NULL,
	[StateId] [int] NULL,
	[CityId] [int] NULL,
 CONSTRAINT [PK_tblUserRegistration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tblCity] ([Id], [StateId], [CityName]) VALUES (1, 1, N'Surat')
INSERT [dbo].[tblCity] ([Id], [StateId], [CityName]) VALUES (2, 1, N'Bardoli')
INSERT [dbo].[tblCity] ([Id], [StateId], [CityName]) VALUES (3, 1, N'Baroda')
INSERT [dbo].[tblCity] ([Id], [StateId], [CityName]) VALUES (4, 2, N'Mumbai')
INSERT [dbo].[tblCity] ([Id], [StateId], [CityName]) VALUES (5, 2, N'Pune')
GO
INSERT [dbo].[tblState] ([Id], [StateName]) VALUES (1, N'Gujarat')
INSERT [dbo].[tblState] ([Id], [StateName]) VALUES (2, N'Maharashtra')
GO
SET IDENTITY_INSERT [dbo].[tblUserRegistration] ON 

INSERT [dbo].[tblUserRegistration] ([Id], [Name], [Email], [Phone], [Address], [StateId], [CityId]) VALUES (3, N'Amitkumar', N'PatelAmit@gmail.com', N'+91-9988774455', N'Rishbh society, near-ankur school, mumbai, maharastra', 2, 4)
INSERT [dbo].[tblUserRegistration] ([Id], [Name], [Email], [Phone], [Address], [StateId], [CityId]) VALUES (5, N'bhavik', N'bhavik.navik@gmail.com', N'+91-8866352276', N'dumas,gujarat', 1, 1)
INSERT [dbo].[tblUserRegistration] ([Id], [Name], [Email], [Phone], [Address], [StateId], [CityId]) VALUES (6, N'keval', N'kevl156@yahoo.com', N'+91-8877454545', N'ambatalavadi, katargam, surat', 1, 1)
INSERT [dbo].[tblUserRegistration] ([Id], [Name], [Email], [Phone], [Address], [StateId], [CityId]) VALUES (7, N'vandankumar ', N'vandan@ymail.com', N'+91-6658787877', N'A block, Silverstone appartment, pune, maharastra', 2, 5)
SET IDENTITY_INSERT [dbo].[tblUserRegistration] OFF
GO
/****** Object:  StoredProcedure [dbo].[SP_Delete_User]    Script Date: 4/30/2023 10:37:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Delete_User]   
    @Id	int
AS

BEGIN

	DELETE FROM tblUserRegistration WHERE Id=@Id

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Get_CityFromStateId]    Script Date: 4/30/2023 10:37:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Get_CityFromStateId]   
    @StateId int
AS

BEGIN

	select Id,CityName from tblCity   where StateId = @StateId

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Get_State]    Script Date: 4/30/2023 10:37:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Get_State]   
   
AS

BEGIN

	select Id,StateName from tblState   

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Get_User]    Script Date: 4/30/2023 10:37:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Get_User]   
    @Id	int
AS

BEGIN

	SELECT 
	ROW_NUMBER() OVER (ORDER BY U.Id ASC) AS SrNo,
	U.Id,U.Name,U.Email,U.Phone,U.Address,U.StateId,S.StateName,U.CityId,C.CityName 
	FROM tblUserRegistration U
	LEFT JOIN tblState S ON S.Id = U.StateId
	LEFT JOIN tblCity C ON C.Id = U.CityId
	WHERE (@Id = 0 OR U.Id=@Id)   
	ORDER BY U.Id DESC

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Insert_Update_User]    Script Date: 4/30/2023 10:37:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Insert_Update_User]   
    @Id	int,
	@Name varchar(50),
	@Email varchar(50),
	@Phone varchar(50),
	@Address varchar(2000),
	@StateId int,
	@CityId	int
AS

BEGIN
    IF @Id = 0
	BEGIN
		INSERT INTO tblUserRegistration(Name,Email,Phone,Address,StateId,CityId) VALUES (@Name,@Email,@Phone,@Address,@StateId,@CityId)		
	END

	ELSE

	BEGIN
		UPDATE tblUserRegistration SET Name = @Name,Email=@Email,Phone=@Phone,Address=@Address,StateId=@StateId,CityId=@CityId
		WHERE Id = @Id
	END

END
GO
