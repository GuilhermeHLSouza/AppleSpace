create database DbAppleSpace;
use DbAppleSpace;


CREATE TABLE Administradores (
Id_Adm int PRIMARY KEY auto_increment,
Telefone int,
Nome varchar(50) NOT NULL,
Senha varchar(25) NOT NULL,
Email varchar(150) NOT NULL
);

CREATE TABLE Enderecos (
Id_Endereco smallint PRIMARY KEY auto_increment,
CEP int NOT NULL,
Numero tinyint NOT NULL,
Rua varchar(75) NOT NULL,
Bairro varchar(50) NOT NULL,
Complemento varchar(50),
Id_Cliente int
);

CREATE TABLE Transportadoras (
Id_Tranportadora tinyint PRIMARY KEY auto_increment,
Nome varchar(75) NOT NULL,
Cnpj bigint NOT NULL,
Telefone int,
Email varchar(150) NOT NULL
);

CREATE TABLE Entregas (
Id_Entrega smallint PRIMARY KEY auto_increment,
Statu varchar(25) NOT NULL,
Id_Tranportadora tinyint,
Id_venda tinyint,
Id_Endereco tinyint,
FOREIGN KEY(Id_Tranportadora) REFERENCES Transportadoras (Id_Tranportadora)
);

CREATE TABLE Categoria (
Id_Cate int PRIMARY KEY auto_increment,
Nome_Cate varchar(50)
);

CREATE TABLE Venda (
Id_Venda smallint PRIMARY KEY auto_increment,
Forma_Pgm varchar(25) NOT NULL,
Statu varchar(25) NOT NULL,
Id_Carrinho int
);

CREATE TABLE Clientes (
Id_Cliente int PRIMARY KEY auto_increment,
Nome  varchar(75) NOT NULL,
Email varchar(150) NOT NULL,
Cpf bigint NOT NULL,
Telefone  int,
Senha varchar(25) NOT NULL
);

CREATE TABLE Produtos (
Cod_Barra int PRIMARY KEY auto_increment,
Descricao varchar(250),
Preco double(8,2) NOT NULL,
Nome varchar(50) NOT NULL,
Estoque smallint NOT NULL,
Img varchar(500),
Id_Adm int,
Id_Cate int,
FOREIGN KEY(Id_Adm) REFERENCES Administradores (Id_Adm),
FOREIGN KEY(Id_Cate) REFERENCES Categoria (Id_Cate)
);

CREATE TABLE Carrinho (
Id_Carrinho int PRIMARY KEY auto_increment,
Quantidade smallint NOT NULL,
valor double(9,2) NOT NULL,
Cod_Barra int,
Id_Cliente int,
FOREIGN KEY(Cod_Barra) REFERENCES Produtos (Cod_Barra),
FOREIGN KEY(Id_Cliente) REFERENCES Clientes (Id_Cliente)
);

ALTER TABLE Venda ADD FOREIGN KEY(Id_Carrinho) REFERENCES Carrinho (Id_Carrinho);
ALTER TABLE Produtos MODIFY Descricao TEXT;
ALTER TABLE Produtos ADD EmDestaque BOOLEAN NOT NULL DEFAULT FALSE; 

ALTER TABLE Entregas ADD FOREIGN KEY (Id_Venda) REFERENCES Venda(Id_Venda);
ALTER TABLE Entregas ADD FOREIGN KEY (Id_Endereco) REFERENCES Endereos(Id_Endereco);

