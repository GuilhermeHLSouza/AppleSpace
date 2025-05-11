using Applespace.Data;
using Applespace.Models;
using MySql.Data.MySqlClient;

namespace Applespace.Repositorio.Produto
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly Database _db = new Database();

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
                            codBarra = reader.GetInt32("Cod_Barra"),
                            valor = reader.GetDecimal("Preco"),
                            descricao = reader.GetString("Descricao"),
                            nome = reader.GetString("Nome"),
                            estoque = reader.GetInt32("Estoque"),
                            idAdm = reader.GetInt32("id_Adm"),
                            idCate = reader.GetInt32("id_Cate"),
                        });
                    }
                }
            }
            return produtoList;
        }
        public void EditarProdutos(Produtos produto) {

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
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.nome;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.valor;
                cmd.Parameters.Add("@descricao", MySqlDbType.Text).Value = produto.descricao;
                cmd.Parameters.Add("@estoque", MySqlDbType.Int32).Value = produto.estoque;
                cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.codBarra;
                cmd.Parameters.Add("@idCate", MySqlDbType.Int32).Value = produto.idCate;
                cmd.Parameters.Add("@img", MySqlDbType.VarChar).Value = produto.img;
                cmd.Parameters.Add("@destaque", MySqlDbType.Bit).Value = produto.emDestaque;
                cmd.ExecuteNonQuery();
            }
        }
        public void Adicionar(Produtos produto)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string Sql = @"INSERT INTO Produtos 
                        (Nome, Preco, Descricao, Estoque, Id_Adm, Id_Cate, Img, EmDestaque)
                       VALUES 
                        (@nome, @preco, @descricao, @estoque, @idAdm, @idCate, @img, @emDestaque)";

                MySqlCommand cmd = new MySqlCommand(Sql, conn);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.nome;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.valor;
                cmd.Parameters.Add("@descricao", MySqlDbType.Text).Value = produto.descricao;
                cmd.Parameters.Add("@estoque", MySqlDbType.Int32).Value = produto.estoque;
                cmd.Parameters.Add("@idAdm", MySqlDbType.Int32).Value = produto.idAdm;
                cmd.Parameters.Add("@idCate", MySqlDbType.Int32).Value = produto.idCate;
                cmd.Parameters.Add("@img", MySqlDbType.VarChar).Value = produto.img;
                cmd.Parameters.Add("@emDestaque", MySqlDbType.Bit).Value = produto.emDestaque;

                cmd.ExecuteNonQuery();
            }
        }
        public bool RemoverProdutos(int id) {
            using (MySqlConnection conn = _db.GetConnection())
            {
     

                string sql = "DELETE FROM produtos WHERE Cod_Barra = @codBarra";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@codBarra", id);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                return linhasAfetadas > 0; 
            }
        }
        public Produtos ListarProduto(int id)
        {
            Produtos produto = null;
            List<Produtos> prod = new List<Produtos>();
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
                            codBarra = reader.GetInt32("Cod_Barra"),
                            valor = reader.GetDecimal("Preco"),
                            descricao = reader.GetString("Descricao"),
                            nome = reader.GetString("Nome"),
                            img = reader.GetString("Img"),
                            estoque = reader.GetInt32("Estoque"),
                            idCate = reader.GetInt32("id_Cate"),
                            idAdm = reader.GetInt32("id_Adm"),
                            emDestaque = reader.GetBoolean("EmDestaque"),
                        };
                    }
                }
            }
            return produto;
        }
    }
}
