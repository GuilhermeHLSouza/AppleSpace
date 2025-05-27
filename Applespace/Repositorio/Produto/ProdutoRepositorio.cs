using Applespace.Data;
using Applespace.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using MySql.Data.MySqlClient;

namespace Applespace.Repositorio.Produto
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly Database _db;

        public ProdutoRepositorio(Database db)
        {
            _db = db;
        }

        public IEnumerable<Produtos> MostrarProdutos()
        {
            List<Produtos> produtoList = new List<Produtos>();
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = "SELECT * FROM Produtos";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        produtoList.Add(new Produtos
                        {
                            CodBarra = reader.GetInt32("Cod_Barra"),
                            Valor = reader.GetDecimal("Preco"),
                            Descricao = reader.GetString("Descricao"),
                            Nome = reader.GetString("Nome"),
                            Estoque = reader.GetInt32("Estoque"),
                            IdCate = reader.GetInt32("id_Cate"),
                            Img = reader.GetString("Img"),
                            EmDestaque = reader.GetBoolean("EmDestaque")
                        });
                    }
                }
            }
            return produtoList;
        }

        public void EditarProdutos(Produtos produto)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string Sql = @"UPDATE Produtos 
                       SET Nome=@nome, 
                           Preco=@preco, 
                           Descricao=@descricao, 
                           Id_Cate=@idCate, 
                           Estoque=@estoque, 
                           Img=@img, 
                           EmDestaque=@destaque
                       WHERE Cod_Barra=@codigo";
                MySqlCommand cmd = new MySqlCommand(Sql, conn);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Valor;
                cmd.Parameters.Add("@descricao", MySqlDbType.Text).Value = produto.Descricao;
                cmd.Parameters.Add("@estoque", MySqlDbType.Int32).Value = produto.Estoque;
                cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.CodBarra;
                cmd.Parameters.Add("@idCate", MySqlDbType.Int32).Value = produto.IdCate;
                cmd.Parameters.Add("@img", MySqlDbType.VarChar).Value = produto.Img;
                cmd.Parameters.Add("@destaque", MySqlDbType.Bit).Value = produto.EmDestaque;
                cmd.ExecuteNonQuery();
            }
        }

        public void Adicionar(Produtos produto)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string Sql = @"INSERT INTO Produtos 
                        (Nome, Preco, Descricao, Estoque, Id_Cate, Img, EmDestaque)
                       VALUES 
                        (@nome, @preco, @descricao, @estoque, @idCate, @img, @emDestaque)";

                MySqlCommand cmd = new MySqlCommand(Sql, conn);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Valor;
                cmd.Parameters.Add("@descricao", MySqlDbType.Text).Value = produto.Descricao;
                cmd.Parameters.Add("@estoque", MySqlDbType.Int32).Value = produto.Estoque;
                cmd.Parameters.Add("@idCate", MySqlDbType.Int32).Value = produto.IdCate;
                cmd.Parameters.Add("@img", MySqlDbType.VarChar).Value = produto.Img;
                cmd.Parameters.Add("@emDestaque", MySqlDbType.Bit).Value = produto.EmDestaque;

                cmd.ExecuteNonQuery();
            }
        }

        public bool RemoverProdutos(int id)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                // 1. Remove todas as entradas do carrinho com esse produto
                string sqlCarrinho = "DELETE FROM Carrinho WHERE Cod_Barra = @cod";
                MySqlCommand cmdCarrinho = new MySqlCommand(sqlCarrinho, conn);
                cmdCarrinho.Parameters.AddWithValue("@cod", id);
                cmdCarrinho.ExecuteNonQuery();

                // 2. Agora deleta o produto
                string sqlProduto = "DELETE FROM Produtos WHERE Cod_Barra = @cod";
                MySqlCommand cmdProduto = new MySqlCommand(sqlProduto, conn);
                cmdProduto.Parameters.AddWithValue("@cod", id);
                int linhasAfetadas = cmdProduto.ExecuteNonQuery();

                return linhasAfetadas > 0;
            }
        }

        public Produtos ListarProduto(int id)
        {
            Produtos produto = null;
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = @"
                                SELECT * FROM Produtos
                                WHERE Produtos.Cod_Barra = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        produto = new Produtos
                        {
                            CodBarra = reader.GetInt32("Cod_Barra"),
                            Valor = reader.GetDecimal("Preco"),
                            Descricao = reader.GetString("Descricao"),
                            Nome = reader.GetString("Nome"),
                            Img = reader.GetString("Img"),
                            Estoque = reader.GetInt32("Estoque"),
                            IdCate = reader.GetInt32("id_Cate"),
                            EmDestaque = reader.GetBoolean("EmDestaque"),
                        };
                    }
                }
            }
            return produto;
        }
    }
}
