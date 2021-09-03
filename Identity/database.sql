﻿CREATE TABLE IF NOT EXISTS [User] (
	[Id] NVARCHAR(255) PRIMARY KEY,
	[Username] NVARCHAR(255) NOT NULL UNIQUE,
   	[FullName] NVARCHAR(255) NOT NULL,
	[PasswordHash] NVARCHAR(255),
	[SecurityStamp] NVARCHAR(255),
	[Email] NVARCHAR(255),
	[Active] TINYINT NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS [Client] (
	[ClientId] NVARCHAR(255) PRIMARY KEY,
	[ClientName] NVARCHAR(255) NOT NULL UNIQUE,
	[ClientSecret] NVARCHAR(255) NOT NULL,
   	[LogoUri] NVARCHAR(255),
	[RequireClientSecret] TINYINT NOT NULL DEFAULT 1,
	[AllowedUris] TEXT
);

CREATE TABLE IF NOT EXISTS [Api] (
	[Id] NVARCHAR(255) PRIMARY KEY,
	[Name] NVARCHAR(255) NOT NULL UNIQUE,
   	[Secret] NVARCHAR(255) NOT NULL,
	[DisplayName] NVARCHAR(255),
	[Enabled] TINYINT NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS [PersistedGrant] (
	[Key] NVARCHAR(255) PRIMARY KEY,
	[Type] NVARCHAR(255),
	[SubjectId] NVARCHAR(255),
	[SessionId] NVARCHAR(255),
	[ClientId] NVARCHAR(255),
	[Description] NVARCHAR(255),
	[CreationTime] NVARCHAR(255),
	[Expiration] NVARCHAR(255),
	[ConsumedTime] NVARCHAR(255),
	[Data] NVARCHAR(255)
);