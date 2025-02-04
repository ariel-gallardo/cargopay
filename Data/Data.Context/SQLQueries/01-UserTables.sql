CREATE DATABASE cargopay_db;
GO
USE cargopay_db;

CREATE TABLE users (
    id BIGINT PRIMARY KEY IDENTITY(1,1),
    email NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    name NVARCHAR(50) NULL,
    created_at DATETIME DEFAULT GETUTCDATE(),
    updated_at DATETIME NULL,
    deleted_at DATETIME NULL,
);