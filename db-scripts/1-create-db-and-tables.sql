USE master
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'WhichTrainAreYouDB')
BEGIN
    CREATE DATABASE [WhichTrainAreYouDB]
END
GO

USE [WhichTrainAreYouDB]
GO

CREATE TABLE Train (
[TrainId] int IDENTITY(1,1) NOT NULL,
[TrainName] varchar(64) NOT NULL,
[Description] varchar(256) NOT NULL,
CONSTRAINT [PK_Train_TrainId] PRIMARY KEY ([TrainId])
);

CREATE TABLE Question (
[QuestionId] int IDENTITY(1,1) NOT NULL,
[Content] varchar(512) NOT NULL,
[TrainId] int,
CONSTRAINT [PK_Question_QuestionId] PRIMARY KEY ([QuestionId]),
CONSTRAINT [FK_Question_Train] FOREIGN KEY (TrainId) REFERENCES [Train]([TrainId]) 
);

CREATE TABLE AppUser (
[UserId] int IDENTITY(1,1) NOT NULL,
[Username] varchar(64) NOT NULL UNIQUE,
[PasswordHash] varchar(256) NOT NULL,
[Salt] varchar(64) NOT NULL,
[Score] int,
[TrainId] int,
CONSTRAINT [PK_AppUser_UserId] PRIMARY KEY ([UserId]),
CONSTRAINT [UQ_AppUser_Username] UNIQUE ([Username]),
CONSTRAINT [FK_AppUser_Train] FOREIGN KEY (TrainId) REFERENCES [Train]([TrainId]) 
);

