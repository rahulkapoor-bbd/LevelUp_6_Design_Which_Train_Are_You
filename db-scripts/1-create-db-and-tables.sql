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
[Description] varchar(512) NOT NULL,
CONSTRAINT [PK_Train_TrainId] PRIMARY KEY ([TrainId])
);

INSERT INTO Train (TrainName, Description)
VALUES
('Union Pacific 9000 Class', 'The Union Pacific Railroad 9000 Class was a class of 88 steam locomotives, built by ALCO for the Union Pacific between 1926 and 1930. The Union Pacific 9000 class was the only class of steam locomotives with a 4-12-2 wheel arrangement ever to be built.'),
('ABB ALP-44', 'The ABB ALP-44 was an electric locomotive which was built by Asea Brown Boveri of Sweden between 1989 and 1997 for the New Jersey Transit and SEPTA railway lines.'),
('SBB RABe 514', 'The RABe 514 is a four-car double decker electrical multiple unit used by the Swiss Federal Railways for the Zürich S-Bahn. It is part of the Siemens Desiro Double Deck product family. The trains are also referred to as DTZ which stands for the German word Doppelstocktriebzug (English: double decker multiple unit).'),
('Class E956', 'The Class E956 (E956形), branded "ALFA-X", is a ten-car experimental Shinkansen train operated by East Japan Railway Company (JR East) in Japan to test technology to be incorporated into future trains operating at speeds of up to 360 km/h. The name is an acronym for "Advanced Labs for Frontline Activity in rail eXperimentation". The first train was unveiled on May 9, 2019.'),
('Mercury', 'Mercury was the name used by the New York Central Railroad for a family of daytime streamliner passenger trains operating between midwestern cities. The Mercury train sets were designed by the noted industrial designer Henry Dreyfuss, and are considered a prime example of Streamline Moderne design. The success of the Mercury led to Dreyfuss getting the commission for the 1938 redesign of the NYCs flagship, the 20th Century Limited, one of the most famous trains in the United States of America.');

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
[TrainId] int,
CONSTRAINT [PK_AppUser_UserId] PRIMARY KEY ([UserId]),
CONSTRAINT [UQ_AppUser_Username] UNIQUE ([Username]),
CONSTRAINT [FK_AppUser_Train] FOREIGN KEY (TrainId) REFERENCES [Train]([TrainId]) 
);

