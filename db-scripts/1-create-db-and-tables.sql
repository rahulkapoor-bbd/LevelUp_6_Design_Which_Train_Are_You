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
('Class 5M2', 'The Class 5M2 and Class 5M2A are electric multiple units that are used to provide commuter rail services by Metrorail in the major cities of South Africa. The original Class 5M2 trainsets were built for the South African Railways by Metro-Cammell between 1958 and 1960. Most of those in use today, however, are of Class 5M2A, built to the same design by Union Carriage & Wagon (UCW) from 1962 to 1985; a total of 4,447 coaches were built by UCW.'),
('SBB RABe 514', 'The RABe 514 is a four-car double decker electrical multiple unit used by the Swiss Federal Railways for the Zürich S-Bahn. It is part of the Siemens Desiro Double Deck product family. The trains are also referred to as DTZ which stands for the German word Doppelstocktriebzug (English: double decker multiple unit).'),
('Class E956', 'The Class E956 (E956形), branded "ALFA-X", is a ten-car experimental Shinkansen train operated by East Japan Railway Company (JR East) in Japan to test technology to be incorporated into future trains operating at speeds of up to 360 km/h. The name is an acronym for "Advanced Labs for Frontline Activity in rail eXperimentation". The first train was unveiled on May 9, 2019.'),
('Mercury', 'Mercury was the name used by the New York Central Railroad for a family of daytime streamliner passenger trains operating between midwestern cities. The Mercury train sets were designed by the noted industrial designer Henry Dreyfuss, and are considered a prime example of Streamline Moderne design. The success of the Mercury led to Dreyfuss getting the commission for the 1938 redesign of the NYCs flagship, the 20th Century Limited, one of the most famous trains in the United States of America.');

CREATE TABLE Question (
[QuestionId] int IDENTITY(1,1) NOT NULL,
[Content] varchar(512) NOT NULL,
[TrainId] int NOT NULL,
[IsPositive] bit NOT NULL,
CONSTRAINT [PK_Question_QuestionId] PRIMARY KEY ([QuestionId]),
CONSTRAINT [FK_Question_Train] FOREIGN KEY (TrainId) REFERENCES [Train]([TrainId]) 
);

INSERT INTO Question (Content, TrainId, IsPositive)
VALUES
('I always have kind things to say to other people', 3, 1),
('I am usually prepared for every situation', 1, 1),
('I feel comfortable around new people', 5, 1),
('I often feel down or sad', 2, 1),
('I am good at understanding how I am feeling', 4, 1),
('I consider myself the life of the party', 5, 1),
('I am good at reading other peoples body language', 3, 1),
('There are many things that I dont like about myself', 5, 1),
('My moods change easily', 2, 1),
('I have good control over my emotions', 2, 0),
('I treat everyone else with kindness and empathy', 3, 1),
('I would rather get tasks done sooner than later', 1, 1),
('I am skilled at handling social situations', 5, 1),
('I often feel troubled by negative thoughts', 2, 1),
('I accept people for who they are', 4, 1),
('I experience emotions very vividly', 4, 1),
('I usually take care of other people before I take care of myself', 3, 1),
('I often worry about what could go wrong', 2, 1),
('I consider myself charming', 5, 1),
('I start arguments just for the fun of it sometimes', 3, 0),
('I often worry that I am not good enough', 2, 1),
('I am flourishing', 4, 1),
('I find it difficult to get started with work', 1, 0),
('I prefer to remain in the background instead of standing out', 5, 0),
('I dont often feel sad', 2, 0),
('I am strongly affected by the suffering of other people', 3, 1),
('I will readily stop what I am doing to help other people', 3, 1),
('I often change my plans', 1, 0),
('I feel comfortable with who I am', 4, 1),
('I often ponder why I am feeling the way I am', 4, 1),
('I avoid philosophical discussions', 4, 0);

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