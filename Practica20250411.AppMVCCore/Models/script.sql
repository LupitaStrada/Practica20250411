-- Creación de la base de datos (Creación de la base de datos)
CREATE DATABASE Practica220250411DB;
GO

USE Practica220250411DB;
GO

-- Tabla: Bodegas
CREATE TABLE Bodegas (
    Id INT PRIMARY KEY IDENTITY(1,1), -- ID de la bodega
    Nombre VARCHAR(255) NOT NULL,        -- Nombre de la bodega
    Notas TEXT                                  -- Notas adicionales
);
GO

-- Tabla: Marcas
CREATE TABLE Marcas (
    Id INT PRIMARY KEY IDENTITY(1,1),    -- ID de la marca
    Nombre VARCHAR(255) NOT NULL,        -- Nombre de la marca
	ImagenBytes VARBINARY(MAX)           -- Logotipo de la marca
);
GO

-- Tabla: Productos
CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- ID del producto
    Nombre VARCHAR(255) NOT NULL,    -- Nombre del producto
    Descripcion TEXT,                          -- Descripción del producto
    Precio DECIMAL(10, 2) NOT NULL,            -- Precio del producto  
    BodegaId INT FOREIGN KEY REFERENCES Bodegas(Id), -- ID de la bodega
    MarcaId INT FOREIGN KEY REFERENCES Marcas(Id),    -- ID de la marca
    Notas TEXT                                  -- Notas adicionales
);
GO

-- Tabla: Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),    -- ID del usuario
    Nombre VARCHAR(255) NOT NULL,      -- Nombre de usuario
    Email VARCHAR(255) UNIQUE NOT NULL, -- Correo electrónico del usuario
    Password CHAR(32) NOT NULL,        -- Hash de la contraseña del usuario
    Rol VARCHAR(50) NOT NULL                -- Rol del usuario   
);
GO
-- PASWWORD default 12345
INSERT INTO Usuarios (Nombre, Email, Password, Rol)
VALUES ('Admin', 'admin@gmail.com', '827ccb0eea8a706c4c34a16891f84e7b', 'ADMINISTRADOR');