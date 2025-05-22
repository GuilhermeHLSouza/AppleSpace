CREATE DATABASE DbAppleSpace;
USE DbAppleSpace;

-- Tabela de Usuários
CREATE TABLE Usuarios (
    Id_Usuario INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(75) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Cpf VARCHAR(15) NOT NULL UNIQUE,
    Telefone VARCHAR(12) NOT NULL,
    Senha VARCHAR(25) NOT NULL,
    Adm BOOLEAN DEFAULT FALSE
);

-- Tabela de Endereços
CREATE TABLE Enderecos (
    Id_Endereco SMALLINT PRIMARY KEY AUTO_INCREMENT,
    CEP INT NOT NULL,
    Numero TINYINT NOT NULL,
    Rua VARCHAR(75) NOT NULL,
    Bairro VARCHAR(50) NOT NULL,
    Complemento VARCHAR(50),
    Id_Usuario INT,
    FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(Id_Usuario)
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
    Id_Cate INT,
    FOREIGN KEY (Id_Cate) REFERENCES Categoria(Id_Cate)
);

-- Tabela de Carrinho
CREATE TABLE Carrinho (
    Id_Carrinho INT PRIMARY KEY AUTO_INCREMENT,
    Quantidade SMALLINT NOT NULL,
    Valor DOUBLE(9,2) NOT NULL,
    Cod_Barra INT,
    Id_Usuario INT,
    FOREIGN KEY (Cod_Barra) REFERENCES Produtos(Cod_Barra),
    FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(Id_Usuario)
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

-- Tabela dos Cupons
CREATE TABLE Cupons (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Codigo VARCHAR(50) NOT NULL UNIQUE,
    Tipo ENUM('porcentagem', 'frete', 'valorfixo') NOT NULL,
    Valor DECIMAL(10,2),
    Expiracao DATETIME,
    Ativo BOOLEAN DEFAULT TRUE
);

-- Inserts Categorias
INSERT INTO Categoria(Nome_Cate)
values ("iPhones");

INSERT INTO Categoria(Nome_Cate)
values ("iPads");

INSERT INTO Categoria(Nome_Cate)
values ("Apple Watchs");

INSERT INTO Categoria(Nome_Cate)
values ("AirPods");

INSERT INTO Categoria(Nome_Cate)
values ("MacBooks");



-- Categoria 1: iPhones
INSERT INTO Produtos (Nome, Descricao, Preco, Estoque, Img, EmDestaque, Id_Cate)
VALUES (
  'iPhone 15 Pro', 
  'O novo iPhone com chip A17 Pro e design de titânio.', 
  7999.00, 
  50, 
  'https://imgs.casasbahia.com.br/55067421/1g.jpg?imwidth=1000', 
  TRUE,  
  1
);

-- Categoria 2: iPads
INSERT INTO Produtos (Nome, Descricao, Preco, Estoque, Img, EmDestaque, Id_Cate)
VALUES (
  'iPad Air (M2)', 
  'iPad leve e potente com chip M2 e tela Liquid Retina.', 
  5999.00, 
  30, 
  'https://http2.mlstatic.com/D_NQ_NP_996392-MLU77326285741_062024-O.webp', 
  FALSE, 
  2
);

-- Categoria 3: Apple Watchs
INSERT INTO Produtos (Nome, Descricao, Preco, Estoque, Img, EmDestaque, Id_Cate)
VALUES (
  'Apple Watch Series 9', 
  'Relógio com tela always-on e sensor de temperatura.', 
  3999.00, 
  20, 
  'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/watch-card-40-s9-202403?wid=600&hei=600&fmt=jpeg&qlt=95&.v=1707850634814', 
  TRUE, 
  3
);

-- Categoria 4: AirPods
INSERT INTO Produtos (Nome, Descricao, Preco, Estoque, Img, EmDestaque, Id_Cate)
VALUES (
  'AirPods Pro (2ª geração)', 
  'Fones com cancelamento ativo de ruído, chip H2 e estojo USB-C.', 
  2499.00, 
  40, 
  'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/MME73?wid=532&hei=582&fmt=png-alpha&.v=1632861342000', 
  FALSE, 
  4
);

-- Categoria 5: MacBooks
INSERT INTO Produtos (Nome, Descricao, Preco, Estoque, Img, EmDestaque, Id_Cate)
VALUES (
  'MacBook Pro 14" (M3)', 
  'Notebook com chip M3, tela Liquid Retina XDR e até 22h de bateria.', 
  13999.00, 
  15, 
  'https://www.goimports.com.br/image/catalog/0%20macm3/m.3PRO/mbp14-m3-max-pro-spaceblack-gallery1-202310.png', 
  TRUE, 
  5
);
INSERT INTO Produtos (Nome, Descricao, Preco, Estoque, Img, EmDestaque, Id_Cate)
VALUES (
  'iPhone 16 Pro',
  'Smartphone premium com chip A18 Pro, câmera tripla de 48MP e design em titânio.',
  7999.00,
  50,
  'https://imgs.casasbahia.com.br/1569849158/1xg.jpg?imwidth=500',
  TRUE,
  1
);
