/*****************************************************************************************
 HOW TO USE (SQL Server):
 1) Open in SSMS.
 2) Run this whole script (F5). It will create the database and all objects.
 3) Adjust database name if you want (default: BlogDb).
******************************************************************************************/

-- 1) Create database (adjust name if needed)
IF DB_ID('BlogDb') IS NULL
BEGIN
  CREATE DATABASE BlogDb;
END
GO

-- 2) Use database
USE BlogDb;
GO

-----------------------------------------------------------------------------------
-- 3) Tables
------------------------------------------------------------------------------------------

-- Users
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
CREATE TABLE dbo.Users (
    id INT IDENTITY(1,1) CONSTRAINT PK_Users PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL CONSTRAINT UQ_Users_Email UNIQUE,
    password_hash NVARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT SYSUTCDATETIME()
);
GO

-- Posts
IF OBJECT_ID('dbo.Posts', 'U') IS NOT NULL DROP TABLE dbo.Posts;
CREATE TABLE dbo.Posts (
    id INT IDENTITY(1,1) CONSTRAINT PK_Posts PRIMARY KEY,
    user_id INT NOT NULL,
    title NVARCHAR(200) NOT NULL,
    content NVARCHAR(MAX) NOT NULL,
    created_at DATETIME2 DEFAULT SYSUTCDATETIME(),
    updated_at DATETIME2 DEFAULT SYSUTCDATETIME(),
    is_published BIT DEFAULT 0
);
GO

-- Comments
IF OBJECT_ID('dbo.Comments', 'U') IS NOT NULL DROP TABLE dbo.Comments;
CREATE TABLE dbo.Comments (
    id INT IDENTITY(1,1) CONSTRAINT PK_Comments PRIMARY KEY,
    post_id INT NOT NULL,
    user_id INT NOT NULL,
    content NVARCHAR(MAX) NOT NULL,
    created_at DATETIME2 DEFAULT SYSUTCDATETIME(),
    is_deleted BIT DEFAULT 0
);
GO

-- Tags
IF OBJECT_ID('dbo.Tags', 'U') IS NOT NULL DROP TABLE dbo.Tags;
CREATE TABLE dbo.Tags (
    id INT IDENTITY(1,1) CONSTRAINT PK_Tags PRIMARY KEY,
    name NVARCHAR(50) NOT NULL CONSTRAINT UQ_Tags_Name UNIQUE,
    created_at DATETIME2 DEFAULT SYSUTCDATETIME()
);
GO

-- Post_Tags (N:M)
IF OBJECT_ID('dbo.Post_Tags', 'U') IS NOT NULL DROP TABLE dbo.Post_Tags;
CREATE TABLE dbo.Post_Tags (
    post_id INT NOT NULL,
    tag_id  INT NOT NULL,
    CONSTRAINT PK_Post_Tags PRIMARY KEY (post_id, tag_id)
);
GO

------------------------------------------------------------------------------------------
-- 4) Foreign Keys (with cascade rules)
------------------------------------------------------------------------------------------

-- Posts → Users (If you delete a user then his post will be delete too)
ALTER TABLE dbo.Posts WITH CHECK
ADD CONSTRAINT FK_Posts_Users
FOREIGN KEY (user_id) REFERENCES dbo.Users(id)
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

-- Comments → Posts (if you delete a post, his comments will too)
ALTER TABLE dbo.Comments WITH CHECK
ADD CONSTRAINT FK_Comments_Posts
FOREIGN KEY (post_id) REFERENCES dbo.Posts(id)
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

-- Comments → Users (if you delete a user, his comments will to)
ALTER TABLE dbo.Comments WITH CHECK
ADD CONSTRAINT FK_Comments_Users
FOREIGN KEY (user_id) REFERENCES dbo.Users(id)
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

-- Post_Tags → Posts (If you delete a post, its tags are cleared)
ALTER TABLE dbo.Post_Tags WITH CHECK
ADD CONSTRAINT FK_PostTags_Posts
FOREIGN KEY (post_id) REFERENCES dbo.Posts(id)
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

-- Post_Tags → Tags (If you delete a tag, its relationships are cleared)
ALTER TABLE dbo.Post_Tags WITH CHECK
ADD CONSTRAINT FK_PostTags_Tags
FOREIGN KEY (tag_id) REFERENCES dbo.Tags(id)
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO

------------------------------------------------------------------------------------------
-- 6) Helpful Indexes
------------------------------------------------------------------------------------------

CREATE INDEX IX_Posts_UserId     ON dbo.Posts(user_id);
CREATE INDEX IX_Comments_PostId  ON dbo.Comments(post_id);
CREATE INDEX IX_Comments_UserId  ON dbo.Comments(user_id);
CREATE INDEX IX_PostTags_TagId   ON dbo.Post_Tags(tag_id);
GO

------------------------------------------------------------------------------------------
-- 7) Sample seed data (optional) — uncomment to test
------------------------------------------------------------------------------------------
-- INSERT INTO dbo.Users(name, email, password_hash) VALUES
-- ('Alice', 'alice@example.com', 'x'),
-- ('Bob', 'bob@example.com', 'y');
--
-- INSERT INTO dbo.Tags(name) VALUES
-- ('tecnologia'),
-- ('dev');
--
-- INSERT INTO dbo.Posts(user_id, title, content, is_published) VALUES
-- (1, 'Primer post', 'Contenido...', 1),
-- (1, 'Segundo post', 'Contenido...', 0);
--
-- INSERT INTO dbo.Comments(post_id, user_id, content) VALUES
-- (1, 2, '¡Buen post!');
--
-- INSERT INTO dbo.Post_Tags(post_id, tag_id) VALUES
-- (1, 1), (1, 2);
-- GO

PRINT 'BlogDb created (tables, FKs, indexes).';
