CREATE DATABASE DbAppleSpace;
USE DbAppleSpace;

-- Tabela de Administradores
CREATE TABLE Administradores (
    Id_Adm INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(50) NOT NULL,
    Telefone INT,
    Email VARCHAR(150) NOT NULL,
    Senha VARCHAR(25) NOT NULL
);

-- Tabela de Clientes
CREATE TABLE Clientes (
    Id_Cliente INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(75) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Cpf BIGINT NOT NULL,
    Telefone INT,
    Senha VARCHAR(25) NOT NULL
);

-- Tabela de Endereços
CREATE TABLE Enderecos (
    Id_Endereco SMALLINT PRIMARY KEY AUTO_INCREMENT,
    CEP INT NOT NULL,
    Numero TINYINT NOT NULL,
    Rua VARCHAR(75) NOT NULL,
    Bairro VARCHAR(50) NOT NULL,
    Complemento VARCHAR(50),
    Id_Cliente INT,
    FOREIGN KEY (Id_Cliente) REFERENCES Clientes(Id_Cliente)
);

-- Tabela de Transportadoras
CREATE TABLE Transportadoras (
    Id_Tranportadora TINYINT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(75) NOT NULL,
    Cnpj BIGINT NOT NULL,
    Telefone INT,
    Email VARCHAR(150) NOT NULL
);

-- Tabela de Categorias
CREATE TABLE Categoria (
    Id_Cate INT PRIMARY KEY AUTO_INCREMENT,
    Nome_Cate VARCHAR(50)
);

-- Tabela de Produtos
CREATE TABLE Produtos (
    Cod_Barra INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(50) NOT NULL,
    Descricao TEXT,
    Preco DOUBLE(8,2) NOT NULL,
    Estoque SMALLINT NOT NULL,
    Img VARCHAR(500),
    EmDestaque BOOLEAN NOT NULL DEFAULT FALSE,
    Id_Adm INT,
    Id_Cate INT,
    FOREIGN KEY (Id_Adm) REFERENCES Administradores(Id_Adm),
    FOREIGN KEY (Id_Cate) REFERENCES Categoria(Id_Cate)
);

-- Tabela de Carrinho
CREATE TABLE Carrinho (
    Id_Carrinho INT PRIMARY KEY AUTO_INCREMENT,
    Quantidade SMALLINT NOT NULL,
    Valor DOUBLE(9,2) NOT NULL,
    Cod_Barra INT,
    Id_Cliente INT,
    FOREIGN KEY (Cod_Barra) REFERENCES Produtos(Cod_Barra),
    FOREIGN KEY (Id_Cliente) REFERENCES Clientes(Id_Cliente)
);

-- Tabela de Vendas
CREATE TABLE Venda (
    Id_Venda SMALLINT PRIMARY KEY AUTO_INCREMENT,
    Forma_Pgm VARCHAR(25) NOT NULL,
    Statu VARCHAR(25) NOT NULL,
    Id_Carrinho INT,
    FOREIGN KEY (Id_Carrinho) REFERENCES Carrinho(Id_Carrinho)
);

-- Tabela de Entregas
CREATE TABLE Entregas (
    Id_Entrega SMALLINT PRIMARY KEY AUTO_INCREMENT,
    Statu VARCHAR(25) NOT NULL,
    Id_Tranportadora TINYINT,
    Id_Venda SMALLINT,
    Id_Endereco SMALLINT,
    FOREIGN KEY (Id_Tranportadora) REFERENCES Transportadoras(Id_Tranportadora),
    FOREIGN KEY (Id_Venda) REFERENCES Venda(Id_Venda),
    FOREIGN KEY (Id_Endereco) REFERENCES Enderecos(Id_Endereco)
);

-- Tabela de Cupons
CREATE TABLE Cupons (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Codigo VARCHAR(50) NOT NULL UNIQUE,
    Tipo VARCHAR(20), 
    Valor DECIMAL(10,2), 
    Expiracao DATETIME,
    Ativo BOOLEAN DEFAULT TRUE
);
