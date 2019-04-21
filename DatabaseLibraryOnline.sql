

CREATE DATABASE LibraryOnline
GO
USE LibraryOnline
GO
CREATE TABLE ROLES
( id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
  name NVARCHAR(50) NOT NULL,
  describe NVARCHAR(MAX),
);
GO

CREATE TABLE SUBJECTEBOOK
( id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
subebook_id NVARCHAR(10) NOT NULL, 
  name NVARCHAR(50) NOT NULL,
);
GO
CREATE TABLE SUBJECTESSAY
( id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
subessay_id NVARCHAR(10) NOT NULL, 
  name NVARCHAR(50) NOT NULL,
);
GO
CREATE TABLE SUBJECTTHESIS
( id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
subthesis_id NVARCHAR(10) NOT NULL, 
  name NVARCHAR(50) NOT NULL,
);
GO
CREATE TABLE USERS
( id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
  username NVARCHAR(50) NOT NULL,
  pass NVARCHAR(50) NOT NULL,
  rode_id INT NOT NULL,
  fullname NVARCHAR(50),
  code_id NVARCHAR(10),
  class_id NVARCHAR(10),
  CONSTRAINT fk_USERS_ROLES 
   FOREIGN KEY (rode_id)
   REFERENCES ROLES (id) 
);

CREATE TABLE EBOOKS
( id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
  ebook_id NVARCHAR(10) NOT NULL,
  title NVARCHAR(MAX) NOT NULL,
  author NVARCHAR(MAX) NOT NULL,
  year NVARCHAR(10),
  describe NVARCHAR(MAX),
  filename NVARCHAR(MAX),
  date_upload DATETIME,
  user_id INT,
  sub_id INT,
  CONSTRAINT fk_EBOOKS_USERS 
   FOREIGN KEY (user_id)
   REFERENCES USERS (id),
    CONSTRAINT fk_EBOOKS_SUBJECTEBOOK
   FOREIGN KEY (sub_id)
   REFERENCES SUBJECTEBOOK (id) 
);
GO
CREATE TABLE ESSAYS
( id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
  essay_id NVARCHAR(10) NOT NULL,
  title NVARCHAR(MAX) NOT NULL,
  instructor NVARCHAR(MAX) NOT NULL,
  executor1 NVARCHAR(50),
  executor2 NVARCHAR(50),
  describe NVARCHAR(MAX),
  filename NVARCHAR(MAX),
  date_upload DATETIME,
  user_id INT,
  sub_id INT,
  CONSTRAINT fk_ESSAYS_USERS 
   FOREIGN KEY (user_id)
   REFERENCES USERS (id) ,
   CONSTRAINT fk_EBOOKS_SUBJECTESSAY
   FOREIGN KEY (sub_id)
   REFERENCES SUBJECTESSAY (id) 
);
GO
CREATE TABLE THESIS
( id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
  thesis_id NVARCHAR(10) NOT NULL,
  title NVARCHAR(MAX) NOT NULL,
  instructor NVARCHAR(MAX) NOT NULL,
  executor1 NVARCHAR(50),
  executor2 NVARCHAR(50),
  describe NVARCHAR(MAX),
  filename NVARCHAR(MAX),
  date_upload DATETIME,
  user_id INT,
  sub_id INT,
   CONSTRAINT fk_THESIS_USERS 
   FOREIGN KEY (user_id)
   REFERENCES USERS (id) ,
   CONSTRAINT fk_EBOOKS_SUBJECTTHESIS
   FOREIGN KEY (sub_id)
   REFERENCES SUBJECTTHESIS (id) 
);
GO



USE LibraryOnline
GO
GO
if exists (select * from sysobjects WHERE name = 'fn_NextID' AND type = 'fn')
    drop function fn_NextID
GO
-- @lastid là mã cuối cùng (fixwidth)
-- @prefix là tiền tố mã: vd HS0001 => HS
-- @size là số lượng ký tự trong mã HS0001 => 6
CREATE function fn_NextID (@lastid varchar(10),@prefix varchar(10),@size int)
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
if exists (select * from sysobjects WHERE name = 'tr_NextID_EBOOK' AND type = 'TR')
    drop trigger tr_NextID_EBOOK
GO
create trigger tr_NextID_EBOOK on EBOOKS
for insert
as
    begin
        DECLARE @lastid nvarchar(10)
        SET @lastid  = (SELECT TOP 1 ebook_id from EBOOKS order by ebook_id desc)
        UPDATE EBOOKS set ebook_id = dbo.fn_NextID (@lastid,'EB',7) where ebook_id = ''
    end
GO

if exists (select * from sysobjects WHERE name = 'tr_NextID_ESSAY' AND type = 'TR')
    drop trigger tr_NextID_ESSAY
GO
create trigger tr_NextID_ESSAY on ESSAYS
for insert
as
    begin
        DECLARE @lastid nvarchar(10)
        SET @lastid  = (SELECT TOP 1 essay_id from ESSAYS order by essay_id desc)
        UPDATE ESSAYS set essay_id = dbo.fn_NextID (@lastid,'ES',7) where essay_id = ''
    end
GO

if exists (select * from sysobjects WHERE name = 'tr_NextID_THESIS' AND type = 'TR')
    drop trigger tr_NextID_THESIS
GO
create trigger tr_NextID_THESIS on THESIS
for insert
as
    begin
        DECLARE @lastid nvarchar(10)
        SET @lastid  = (SELECT TOP 1 thesis_id from THESIS order by thesis_id desc)
        UPDATE THESIS set thesis_id = dbo.fn_NextID (@lastid,'TH',7) where thesis_id = ''
    end
GO


if exists (select * from sysobjects WHERE name = 'tr_NextID_SUBJECTEBOOK' AND type = 'TR')
    drop trigger tr_NextID_SUBJECTEBOOK
GO
create trigger tr_NextID_SUBJECTEBOOK on SUBJECTEBOOK
for insert
as
    begin
        DECLARE @lastid nvarchar(10)
        SET @lastid  = (SELECT TOP 1 subebook_id from SUBJECTEBOOK order by subebook_id desc)
        UPDATE SUBJECTEBOOK set subebook_id = dbo.fn_NextID (@lastid,'SE',7) where subebook_id = ''
    end
GO
--insert SUBJECTEBOOK (subebook_id,name)
--values ('','aaaaaaaaa')
--select * from SUBJECTEBOOK




if exists (select * from sysobjects WHERE name = 'tr_NextID_SUBJECTESSAY' AND type = 'TR')
    drop trigger tr_NextID_SUBJECTESSAY
GO
create trigger tr_NextID_SUBJECTESSAY on SUBJECTESSAY
for insert
as
    begin
        DECLARE @lastid nvarchar(10)
        SET @lastid  = (SELECT TOP 1 subessay_id from SUBJECTESSAY order by subessay_id desc)
        UPDATE SUBJECTESSAY set subessay_id = dbo.fn_NextID (@lastid,'SS',7) where subessay_id = ''
    end
GO

--insert SUBJECTESSAY (subessay_id,name)
--values ('','aaaaaaaaa')
--select * from SUBJECTESSAY

if exists (select * from sysobjects WHERE name = 'tr_NextID_SUBJECTTHESIS' AND type = 'TR')
    drop trigger tr_NextID_SUBJECTTHESIS
GO
create trigger tr_NextID_SUBJECTTHESIS on SUBJECTTHESIS
for insert
as
    begin
        DECLARE @lastid nvarchar(10)
        SET @lastid  = (SELECT TOP 1 subthesis_id from SUBJECTTHESIS order by subthesis_id desc)
        UPDATE SUBJECTTHESIS set subthesis_id = dbo.fn_NextID (@lastid,'ST',7) where subthesis_id = ''
    end
GO

