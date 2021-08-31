﻿CREATE TABLE IF NOT EXISTS [User] (
	Id NVARCHAR(255) PRIMARY KEY,
	Username NVARCHAR(255) NOT NULL UNIQUE,
   	FullName NVARCHAR(255) NOT NULL,
	PasswordHash NVARCHAR(255),
	SecurityStamp NVARCHAR(255),
	Email NVARCHAR(255),
	Active TINYINT NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS [Client] (
	ClientId NVARCHAR(255) PRIMARY KEY,
	ClientName NVARCHAR(255) NOT NULL UNIQUE,
	ClientSecret NVARCHAR(255) NOT NULL,
   	LogoUri NVARCHAR(255),
	RequireClientSecret TINYINT NOT NULL DEFAULT 1,
	AllowedUris TEXT
);