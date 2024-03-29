USE [LibraryOnlineFinal]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_NextID]    Script Date: 12-Jun-19 21:29:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- @lastid là mã cuối cùng (fixwidth)
-- @prefix là tiền tố mã: vd HS0001 => HS
-- @size là số lượng ký tự trong mã HS0001 => 6
--function tạo mã tự động
CREATE function [dbo].[fn_NextID] (@lastid varchar(10),@prefix varchar(10),@size int)
  returns varchar(10)
as
    BEGIN
        IF(@lastid = '')
            set @lastid = @prefix + REPLICATE (0,@size - LEN(@prefix))
        declare @num_nextid int, @nextid varchar(10)
        set @lastid = LTRIM(RTRIM(@lastid))
        -- number next id
        set @num_nextid = replace(@lastid,@prefix,'') + 1
        -- bo di so luong ky tu tien to
        set @size = @size - len(@prefix)
        -- replicate số lượng số 0 REPLICATE(0,3) => 000
        set @nextid = @prefix + REPLICATE (0,@size - LEN(@prefix))
        set @nextid = @prefix + RIGHT(REPLICATE(0, @size) + CONVERT (VARCHAR(MAX), @num_nextid), @size)
        return @nextid
    END;
GO
/****** Object:  Table [dbo].[Ebook]    Script Date: 12-Jun-19 21:29:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ebook](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ebook_id] [nvarchar](10) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[author] [nvarchar](max) NOT NULL,
	[year] [nvarchar](10) NULL,
	[describe] [nvarchar](max) NULL,
	[filename] [nvarchar](max) NULL,
	[date_upload] [datetime] NULL,
	[user_id] [int] NULL,
	[sub_id] [int] NULL,
	[countView] [int] NULL,
	[countDowload] [int] NULL,
 CONSTRAINT [PK__Ebook__3213E83F8F059D3E] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Essay]    Script Date: 12-Jun-19 21:29:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Essay](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[essay_id] [nvarchar](10) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[instructor] [nvarchar](max) NOT NULL,
	[executor1] [nvarchar](50) NULL,
	[executor2] [nvarchar](50) NULL,
	[describe] [nvarchar](max) NULL,
	[filename] [nvarchar](max) NULL,
	[date_upload] [datetime] NULL,
	[user_id] [int] NULL,
	[sub_id] [int] NULL,
	[course] [nchar](10) NULL,
	[countView] [int] NULL,
	[countDowload] [int] NULL,
 CONSTRAINT [PK__Essay__3213E83F232F7A9A] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RateStar]    Script Date: 12-Jun-19 21:29:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RateStar](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[book_id] [nvarchar](10) NOT NULL,
	[usename] [nvarchar](50) NULL,
	[user_id] [int] NOT NULL,
	[rate] [int] NOT NULL,
 CONSTRAINT [PK__RateStar__3213E83FD0923CCF] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 12-Jun-19 21:29:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[describe] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchFile]    Script Date: 12-Jun-19 21:29:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchFile](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[book_id] [nvarchar](10) NOT NULL,
	[title] [nvarchar](max) NULL,
	[author] [nvarchar](max) NULL,
	[year] [nvarchar](10) NULL,
	[instructor] [nvarchar](max) NULL,
	[executor1] [nvarchar](50) NULL,
	[executor2] [nvarchar](50) NULL,
	[describe] [nvarchar](max) NULL,
	[filename] [nvarchar](max) NULL,
	[date_upload] [datetime] NULL,
	[user_id] [int] NULL,
	[sub_id] [int] NULL,
	[username] [nvarchar](50) NULL,
	[type] [nchar](10) NULL,
 CONSTRAINT [PK__SearchFi__3213E83FB0380BD9] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject_Ebook]    Script Date: 12-Jun-19 21:29:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject_Ebook](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subebook_id] [nvarchar](10) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject_Essay]    Script Date: 12-Jun-19 21:29:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject_Essay](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subessay_id] [nvarchar](10) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject_Thesis]    Script Date: 12-Jun-19 21:29:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject_Thesis](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subthesis_id] [nvarchar](10) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Thesis]    Script Date: 12-Jun-19 21:29:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thesis](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[thesis_id] [nvarchar](10) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[instructor] [nvarchar](max) NOT NULL,
	[executor1] [nvarchar](50) NULL,
	[executor2] [nvarchar](50) NULL,
	[describe] [nvarchar](max) NULL,
	[filename] [nvarchar](max) NULL,
	[date_upload] [datetime] NULL,
	[user_id] [int] NULL,
	[sub_id] [int] NULL,
	[cource] [nchar](10) NULL,
	[countView] [int] NULL,
	[countDowload] [int] NULL,
 CONSTRAINT [PK__Thesis__3213E83FB4C6150E] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Time]    Script Date: 12-Jun-19 21:29:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Time](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[bookid] [nchar](10) NULL,
	[bookname] [nvarchar](50) NULL,
	[time] [nchar](10) NOT NULL,
	[date] [datetime] NULL,
 CONSTRAINT [PK_Time] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12-Jun-19 21:29:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[role_id] [int] NOT NULL,
	[fullname] [nvarchar](50) NULL,
	[mssv] [nvarchar](10) NULL,
	[class_id] [nvarchar](10) NULL,
	[resetPasswordCode] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Ebook] ON 

INSERT [dbo].[Ebook] ([id], [ebook_id], [title], [author], [year], [describe], [filename], [date_upload], [user_id], [sub_id], [countView], [countDowload]) VALUES (59, N'EB00001', N'aaaaaaaaaa', N'aqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq', N'1999', N'', N'BaiGiangATBMTT.pdf', CAST(N'2019-06-09T22:26:10.040' AS DateTime), 1, 5, 2, 1)
INSERT [dbo].[Ebook] ([id], [ebook_id], [title], [author], [year], [describe], [filename], [date_upload], [user_id], [sub_id], [countView], [countDowload]) VALUES (63, N'EB00003', N'l', N'l', N'1999', N'', N'Chapter 3 - Software Security.pdf', CAST(N'2019-06-10T22:39:12.237' AS DateTime), 9, 1, 0, 0)
INSERT [dbo].[Ebook] ([id], [ebook_id], [title], [author], [year], [describe], [filename], [date_upload], [user_id], [sub_id], [countView], [countDowload]) VALUES (64, N'EB00004', N'a', N'l', N'1999', N'', N'Chapter 6 - Access Control.pdf', CAST(N'2019-06-10T22:42:59.657' AS DateTime), 9, 2, 0, 0)
INSERT [dbo].[Ebook] ([id], [ebook_id], [title], [author], [year], [describe], [filename], [date_upload], [user_id], [sub_id], [countView], [countDowload]) VALUES (65, N'EB00005', N'adfdgfh', N'aaa', N'1999', N'', N'Chapter 8 - Malicious Code.pdf', CAST(N'2019-06-10T22:43:23.050' AS DateTime), 1, 2, 0, 0)
INSERT [dbo].[Ebook] ([id], [ebook_id], [title], [author], [year], [describe], [filename], [date_upload], [user_id], [sub_id], [countView], [countDowload]) VALUES (66, N'EB00006', N'aaa', N'qqq', N'1999', N'', N'Chapter 10 - Cryptography.pdf', CAST(N'2019-06-11T22:37:06.293' AS DateTime), 9, 6, 1, 0)
INSERT [dbo].[Ebook] ([id], [ebook_id], [title], [author], [year], [describe], [filename], [date_upload], [user_id], [sub_id], [countView], [countDowload]) VALUES (67, N'EB00007', N'Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  ', N'Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  ', N'1999', N'', N'Chapter 3 - Operation System Security_2.pdf', CAST(N'2019-06-12T20:50:17.973' AS DateTime), 9, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[Ebook] OFF
SET IDENTITY_INSERT [dbo].[Essay] ON 

INSERT [dbo].[Essay] ([id], [essay_id], [title], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [course], [countView], [countDowload]) VALUES (22, N'ES00002', N'a', N'i', N'i', N'', N'', N'Chapter 0 - Introduction to Information Security.pdf', CAST(N'2019-06-04T21:22:54.280' AS DateTime), 1, 3, N'i         ', 0, 0)
INSERT [dbo].[Essay] ([id], [essay_id], [title], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [course], [countView], [countDowload]) VALUES (23, N'ES00003', N'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', N'â', N'a', N'a', N'', N'Chapter 1 - Computer Security Concepts.pdf', CAST(N'2019-06-09T22:31:34.180' AS DateTime), 1, 3, N'i         ', 0, 0)
INSERT [dbo].[Essay] ([id], [essay_id], [title], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [course], [countView], [countDowload]) VALUES (25, N'ES00004', N'adjadha kdjskdja dkadk ldlad', N'a', N'i', N'o', N'', N'Chapter 8 - Malicious Code.pdf', CAST(N'2019-06-12T20:55:52.793' AS DateTime), 9, 3, N'i         ', 0, 0)
SET IDENTITY_INSERT [dbo].[Essay] OFF
SET IDENTITY_INSERT [dbo].[RateStar] ON 

INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (1, N'EB00001', N'Lý Hải', 1, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (4, N'ES00001', N'Lý Hải', 1, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (5, N'TH00002', N'Lý Hải', 1, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (6, N'EB00005', N'Lý Hải', 1, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (7, N'EB00003', N'Lý Hải', 1, 3)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (9, N'EB00004', N'aa', 5, 5)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (10, N'TH00001', N'Lý Hải', 1, 5)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (11, N'EB00008', N'Lý Hải', 1, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (13, N'EB00028', N'Lý Hải', 1, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (14, N'EB00001', N'q', 39, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (15, N'ES00003', N'q', 39, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (16, N'ES00003', N'Lý Hải', 1, 5)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (17, N'EB00005', N'Tèo Em', 9, 2)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (18, N'EB00002', N'Tèo Em', 9, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (19, N'ES00003', N'Tèo Em', 9, 3)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (20, N'TH00002', N'Tèo Em', 9, 5)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (21, N'EB00002', N'q', 39, 1)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (22, N'TH00002', N'q', 39, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (23, N'EB00006', N'Tèo Em', 9, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (24, N'EB00007', N'Tèo Em', 9, 4)
INSERT [dbo].[RateStar] ([id], [book_id], [usename], [user_id], [rate]) VALUES (25, N'EB00007', N'Lý Hải', 1, 3)
SET IDENTITY_INSERT [dbo].[RateStar] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([id], [name], [describe]) VALUES (1, N'Admin', NULL)
INSERT [dbo].[Role] ([id], [name], [describe]) VALUES (2, N'Giảng viên', NULL)
INSERT [dbo].[Role] ([id], [name], [describe]) VALUES (3, N'Sinh viên', NULL)
SET IDENTITY_INSERT [dbo].[Role] OFF
SET IDENTITY_INSERT [dbo].[SearchFile] ON 

INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (8, N'EB00004', N'aâgggghfgh', N'â', N'â', N'', N'', N'', N'aa', N'BaiGiangATBMTT.pdf', CAST(N'2019-04-28T21:38:30.500' AS DateTime), 1, 1, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (17, N'EB00005', N'aqqô', N'a', N'1999', N'', N'', N'', N'a', N'BaiGiangATBMTT.pdf', CAST(N'2019-04-28T22:31:57.437' AS DateTime), 1, 1, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (50, N'EB00009', N'a', N'a', N'1999', N'', N'', N'', N'a', N'Chapter 0 - Introduction to Information Security.pdf', CAST(N'2019-05-14T23:09:11.537' AS DateTime), 1, 9, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (55, N'EB00013', N'aaaaaaaaaa', N'aaaaaaaaaaa', N'1999', N'', N'', N'', N'aaaaaaaaa', N'BaiGiangATBMTT.pdf', CAST(N'2019-05-18T11:37:14.780' AS DateTime), 1, 1, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (75, N'TH00001', N'a', N'', N'a         ', N'ai', N'ap', N'a', N'aaaa', N'Chapter 3 - Software Security.pdf', CAST(N'2019-06-03T20:01:53.760' AS DateTime), 1, 1, NULL, N'thesis    ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (91, N'ES00001', N'a', N'', N'', N'a', N'i', N'', N'', N'BaiGiangATBMTT.pdf', CAST(N'2019-06-04T21:21:47.820' AS DateTime), 1, 3, NULL, N'essay     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (92, N'ES00002', N'a', N'', N'', N'i', N'i', N'', N'', N'Chapter 0 - Introduction to Information Security.pdf', CAST(N'2019-06-04T21:22:54.280' AS DateTime), 1, 3, NULL, N'essay     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (93, N'TH00002', N'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', N'', N'a         ', N'a', N'a', N'', N'a', N'Chapter 3 - Operation System Security_2.pdf', CAST(N'2019-06-04T21:28:09.113' AS DateTime), 1, 1, NULL, N'thesis    ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (104, N'EB00009', N'p', N'a', N'1999', N'', N'', N'', N'', N'Chapter 3 - LAB - Software Security_ Smashing Attack.pdf', CAST(N'2019-06-09T22:07:09.413' AS DateTime), 1, 6, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (105, N'EB00010', N'ooooooooooooooooooooooooooooooooooooooooooooooo', N'aaa', N'1999', N'', N'', N'', N'', N'Chapter 3 - Software Security.pdf', CAST(N'2019-06-09T22:08:12.490' AS DateTime), 1, 1, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (112, N'EB00001', N'aaaaaaaaaa', N'aqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq', N'1999', N'', N'', N'', N'', N'BaiGiangATBMTT.pdf', CAST(N'2019-06-09T22:26:10.040' AS DateTime), 1, 5, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (115, N'ES00003', N'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', N'', N'', N'â', N'a', N'a', N'', N'Chapter 1 - Computer Security Concepts.pdf', CAST(N'2019-06-09T22:31:34.180' AS DateTime), 1, 3, NULL, N'essay     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (117, N'EB00003', N'l', N'l', N'1999', N'', N'', N'', N'', N'Chapter 3 - Software Security.pdf', CAST(N'2019-06-10T22:39:12.237' AS DateTime), 9, 1, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (118, N'EB00004', N'a', N'l', N'1999', N'', N'', N'', N'', N'Chapter 6 - Access Control.pdf', CAST(N'2019-06-10T22:42:59.657' AS DateTime), 9, 2, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (119, N'EB00005', N'adfdgfh', N'aaa', N'1999', N'', N'', N'', N'', N'Chapter 8 - Malicious Code.pdf', CAST(N'2019-06-10T22:43:23.050' AS DateTime), 1, 2, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (120, N'EB00006', N'aaa', N'qqq', N'1999', N'', N'', N'', N'', N'Chapter 10 - Cryptography.pdf', CAST(N'2019-06-11T22:37:06.293' AS DateTime), 9, 6, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (123, N'EB00007', N'Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  ', N'Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  Lối Sống Tối Giản Của Người Nhật  ', N'1999', N'', N'', N'', N'', N'Chapter 3 - Operation System Security_2.pdf', CAST(N'2019-06-12T20:50:17.973' AS DateTime), 9, 1, NULL, N'ebook     ')
INSERT [dbo].[SearchFile] ([id], [book_id], [title], [author], [year], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [username], [type]) VALUES (124, N'ES00004', N'adjadha kdjskdja dkadk ldlad', N'', N'', N'a', N'i', N'o', N'', N'Chapter 8 - Malicious Code.pdf', CAST(N'2019-06-12T20:55:52.793' AS DateTime), 9, 3, NULL, N'essay     ')
SET IDENTITY_INSERT [dbo].[SearchFile] OFF
SET IDENTITY_INSERT [dbo].[Subject_Ebook] ON 

INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (1, N'SE00001', N'Lap trinh')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (2, N'SE00002', N'aaa1')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (3, N'SE00003', N'czczcz1')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (5, N'SE00005', N'Lập trình')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (6, N'SE00006', N'Bảo mật')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (7, N'SE00007', N'aaaaaaaaaaaaaaa')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (8, N'SE00008', N'sdaasdass')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (9, N'SE00009', N'aaqw')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (10, N'SE00010', N'aaaa')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (12, N'SE00012', N'aaaaaaaa')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (13, N'SE00013', N'w')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (14, N'SE00014', N'q')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (15, N'SE00015', N'aaa')
INSERT [dbo].[Subject_Ebook] ([id], [subebook_id], [name]) VALUES (16, N'SE00016', N'aaaas')
SET IDENTITY_INSERT [dbo].[Subject_Ebook] OFF
SET IDENTITY_INSERT [dbo].[Subject_Essay] ON 

INSERT [dbo].[Subject_Essay] ([id], [subessay_id], [name]) VALUES (3, N'SS00002', N'aaaaaad1')
INSERT [dbo].[Subject_Essay] ([id], [subessay_id], [name]) VALUES (4, N'SS00003', N'a')
INSERT [dbo].[Subject_Essay] ([id], [subessay_id], [name]) VALUES (5, N'SS00004', N'aâ')
SET IDENTITY_INSERT [dbo].[Subject_Essay] OFF
SET IDENTITY_INSERT [dbo].[Subject_Thesis] ON 

INSERT [dbo].[Subject_Thesis] ([id], [subthesis_id], [name]) VALUES (1, N'ST00001', N'Lap trinh')
INSERT [dbo].[Subject_Thesis] ([id], [subthesis_id], [name]) VALUES (3, N'ST00003', N'czczcz1a')
INSERT [dbo].[Subject_Thesis] ([id], [subthesis_id], [name]) VALUES (4, N'ST00004', N'dấdas')
SET IDENTITY_INSERT [dbo].[Subject_Thesis] OFF
SET IDENTITY_INSERT [dbo].[Thesis] ON 

INSERT [dbo].[Thesis] ([id], [thesis_id], [title], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [cource], [countView], [countDowload]) VALUES (28, N'TH00001', N'a', N'ai', N'ap', N'a', N'aaaa', N'Chapter 3 - Software Security.pdf', CAST(N'2019-06-03T20:01:53.760' AS DateTime), 1, 1, N'a         ', 0, 0)
INSERT [dbo].[Thesis] ([id], [thesis_id], [title], [instructor], [executor1], [executor2], [describe], [filename], [date_upload], [user_id], [sub_id], [cource], [countView], [countDowload]) VALUES (30, N'TH00002', N'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', N'a', N'a', N'', N'a', N'Chapter 3 - Operation System Security_2.pdf', CAST(N'2019-06-04T21:28:09.113' AS DateTime), 1, 1, N'a         ', 1, 0)
SET IDENTITY_INSERT [dbo].[Thesis] OFF
SET IDENTITY_INSERT [dbo].[Time] ON 

INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (25, 1, N'EB00008   ', N'addsd', N'00:00:04  ', CAST(N'2019-05-12T21:30:27.360' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (26, 1, N'EB00001   ', N'aaaaaa', N'00:03:56  ', CAST(N'2019-05-15T22:25:02.840' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (27, 1, N'EB00001   ', N'aaaaaa', N'00:00:36  ', CAST(N'2019-05-15T22:25:41.200' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (28, 5, N'EB00001   ', N'aaaaaa', N'00:01:09  ', CAST(N'2019-05-15T22:37:31.500' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (29, 5, N'EB00001   ', N'aaaaaa', N'00:00:11  ', CAST(N'2019-05-15T22:37:43.087' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (30, 5, N'EB00001   ', N'aaaaaa', N'00:00:02  ', CAST(N'2019-05-15T22:37:52.757' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (31, 5, N'EB00001   ', N'aaaaaa', N'00:00:02  ', CAST(N'2019-05-15T22:37:55.860' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (32, 1, N'EB00001   ', N'aaaaaa', N'00:01:10  ', CAST(N'2019-05-15T22:39:36.670' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (33, 1, N'EB00001   ', N'aaaaaa', N'00:00:03  ', CAST(N'2019-05-15T22:39:41.617' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (34, 1, N'EB00001   ', N'aaaaaa', N'00:00:03  ', CAST(N'2019-05-15T22:39:48.023' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (35, 1, N'EB00001   ', N'aaaaaa', N'00:00:08  ', CAST(N'2019-05-15T22:39:58.000' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (36, 1, N'EB00001   ', N'aaaaaa', N'00:00:14  ', CAST(N'2019-05-15T22:40:13.500' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (37, 1, N'EB00001   ', N'aaaaaa', N'00:03:19  ', CAST(N'2019-05-15T23:01:17.117' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (40, 39, N'EB00028   ', N'g', N'00:00:08  ', CAST(N'2019-06-05T22:52:22.123' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (41, 1, N'EB00001   ', N'www12', N'00:01:19  ', CAST(N'2019-06-05T22:59:35.253' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (42, 39, N'EB00001   ', N'aaaaaaaaaa', N'00:00:55  ', CAST(N'2019-06-09T23:46:21.863' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (43, 39, N'EB00001   ', N'aaaaaaaaaa', N'00:00:11  ', CAST(N'2019-06-09T23:46:33.350' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (44, 1, N'EB00001   ', N'aaaaaaaaaa', N'00:00:05  ', CAST(N'2019-06-10T22:22:36.440' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (45, 9, N'EB00006   ', N'aaa', N'00:00:05  ', CAST(N'2019-06-12T20:48:16.273' AS DateTime))
INSERT [dbo].[Time] ([id], [userid], [bookid], [bookname], [time], [date]) VALUES (46, 9, N'EB00006   ', N'aaa', N'00:00:02  ', CAST(N'2019-06-12T20:48:22.303' AS DateTime))
SET IDENTITY_INSERT [dbo].[Time] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [username], [password], [role_id], [fullname], [mssv], [class_id], [resetPasswordCode]) VALUES (1, N'admin@fit.hcmute.edu.vn', N'202cb962ac59075b964b07152d234b70', 1, N'Lý Hải', N'14500444', N'', NULL)
INSERT [dbo].[Users] ([id], [username], [password], [role_id], [fullname], [mssv], [class_id], [resetPasswordCode]) VALUES (5, N'gv@hcmute.edu.vn', N'1cc39ffd758234422e1f75beadfc5fb2', 2, N'aaaa', N'14201423', N'1420142A', NULL)
INSERT [dbo].[Users] ([id], [username], [password], [role_id], [fullname], [mssv], [class_id], [resetPasswordCode]) VALUES (6, N'lcdatmin97@gmail.com', N'1f32aa4c9a1d2ea010adcf2348166a04', 1, N'KK', N'15110101', N'151102B', N'')
INSERT [dbo].[Users] ([id], [username], [password], [role_id], [fullname], [mssv], [class_id], [resetPasswordCode]) VALUES (9, N'abc2@hcmute.edu.vn', N'202cb962ac59075b964b07152d234b70', 2, N'Tèo Em', N'15110102', N'151101A', NULL)
INSERT [dbo].[Users] ([id], [username], [password], [role_id], [fullname], [mssv], [class_id], [resetPasswordCode]) VALUES (10, N'abc3@hcmute.edu.vn', N'd41d8cd98f00b204e9800998ecf8427e', 3, N'Tèo Chị', N'15110106', N'151103A', NULL)
INSERT [dbo].[Users] ([id], [username], [password], [role_id], [fullname], [mssv], [class_id], [resetPasswordCode]) VALUES (39, N'gv4@hcmute.edu.vn', N'202cb962ac59075b964b07152d234b70', 3, N'q', N'', N'', NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
ALTER TABLE [dbo].[Ebook]  WITH CHECK ADD  CONSTRAINT [fk_EBOOKS_SUBJECTEBOOK] FOREIGN KEY([sub_id])
REFERENCES [dbo].[Subject_Ebook] ([id])
GO
ALTER TABLE [dbo].[Ebook] CHECK CONSTRAINT [fk_EBOOKS_SUBJECTEBOOK]
GO
ALTER TABLE [dbo].[Ebook]  WITH CHECK ADD  CONSTRAINT [fk_EBOOKS_USERS] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Ebook] CHECK CONSTRAINT [fk_EBOOKS_USERS]
GO
ALTER TABLE [dbo].[Essay]  WITH CHECK ADD  CONSTRAINT [fk_EBOOKS_SUBJECTESSAY] FOREIGN KEY([sub_id])
REFERENCES [dbo].[Subject_Essay] ([id])
GO
ALTER TABLE [dbo].[Essay] CHECK CONSTRAINT [fk_EBOOKS_SUBJECTESSAY]
GO
ALTER TABLE [dbo].[Essay]  WITH CHECK ADD  CONSTRAINT [fk_ESSAYS_USERS] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Essay] CHECK CONSTRAINT [fk_ESSAYS_USERS]
GO
ALTER TABLE [dbo].[RateStar]  WITH CHECK ADD  CONSTRAINT [FK_RateStar_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[RateStar] CHECK CONSTRAINT [FK_RateStar_Users]
GO
ALTER TABLE [dbo].[Thesis]  WITH CHECK ADD  CONSTRAINT [fk_EBOOKS_SUBJECTTHESIS] FOREIGN KEY([sub_id])
REFERENCES [dbo].[Subject_Thesis] ([id])
GO
ALTER TABLE [dbo].[Thesis] CHECK CONSTRAINT [fk_EBOOKS_SUBJECTTHESIS]
GO
ALTER TABLE [dbo].[Thesis]  WITH CHECK ADD  CONSTRAINT [fk_THESIS_USERS] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Thesis] CHECK CONSTRAINT [fk_THESIS_USERS]
GO
ALTER TABLE [dbo].[Time]  WITH CHECK ADD  CONSTRAINT [FK_Time_Users] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Time] CHECK CONSTRAINT [FK_Time_Users]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [fk_USERS_ROLES] FOREIGN KEY([role_id])
REFERENCES [dbo].[Role] ([id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [fk_USERS_ROLES]
GO
