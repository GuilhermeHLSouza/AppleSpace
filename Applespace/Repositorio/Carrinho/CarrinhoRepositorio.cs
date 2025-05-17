using Applespace.Models;
using MySql.Data.MySqlClient;
using Applespace.Data;

namespace Applespace.Repositorio.Carrinho
{
    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {
        private readonly Database _db;

        public CarrinhoRepositorio(Database db)
        {
            _db = db;
        }

        public void AdicionarCarrinho(int codBarra, int quantidade, int idCliente)
        {
            using (var conn = _db.GetConnection())
            {
                string checkSql = @"SELECT Id_Carrinho, Quantidade FROM Carrinho 
                                    WHERE Cod_Barra = @codBarra AND Id_Cliente = @idCliente";
                MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@codBarra", codBarra);
                checkCmd.Parameters.AddWithValue("@idCliente", idCliente);

                using (var reader = checkCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int idCarrinho = reader.GetInt32("Id_Carrinho");
                        int qtdAtual = reader.GetInt32("Quantidade");

                        reader.Close();

                        string updateSql = @"UPDATE Carrinho SET Quantidade = @novaQtd 
                                             WHERE Id_Carrinho = @idCarrinho";
                        MySqlCommand Cmd = new MySqlCommand(updateSql, conn);
                        Cmd.Parameters.AddWithValue("@novaQtd", qtdAtual + quantidade);
                        Cmd.Parameters.AddWithValue("@idCarrinho", idCarrinho);
                        Cmd.ExecuteNonQuery();
                        return;
                    }
                }

                string Preco = "SELECT Preco FROM Produtos WHERE Cod_Barra = @cod";
                MySqlCommand precoCmd = new MySqlCommand(Preco, conn);
                precoCmd.Parameters.AddWithValue("@cod", codBarra);
                decimal valor = Convert.ToDecimal(precoCmd.ExecuteScalar());

                string insertSql = @"INSERT INTO Carrinho (Cod_Barra, Quantidade, Valor, Id_Cliente)
                                     VALUES (@codBarra, @quantidade, @valor, @idCliente)";
                MySqlCommand insertCmd = new MySqlCommand(insertSql, conn);
                insertCmd.Parameters.AddWithValue("@codBarra", codBarra);
                insertCmd.Parameters.AddWithValue("@quantidade", quantidade);
                insertCmd.Parameters.AddWithValue("@valor", valor);
                insertCmd.Parameters.AddWithValue("@idCliente", idCliente);
                insertCmd.ExecuteNonQuery();
            }
        }

        public List<Carrinhos> ListarCarrinho(int id)
        {
            List<Carrinhos> carrinhos = new List<Carrinhos>();
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = @"SELECT * FROM Carrinho 
                               INNER JOIN Produtos ON Carrinho.Cod_Barra = Produtos.Cod_Barra 
                               WHERE Id_Cliente = @idCliente";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idCliente", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Carrinhos c = new Carrinhos
                        {
                            idCarrinho = reader.GetInt32("Id_Carrinho"),
                            quantidade = reader.GetInt32("Quantidade"),
                            preco = reader.GetDouble("Valor"),
                            codBarra = reader.GetInt32("Cod_Barra"),
                            idCliente = reader.GetInt32("Id_Cliente"),
                            produtos = new Produtos
                            {
                                nome = reader.GetString("Nome"),
                                valor = reader.GetDecimal("Preco"),
                                img = reader.GetString("Img")
                            }
                        };
                        carrinhos.Add(c);
                    }
                    return carrinhos;
                }
            }
        }

        public void RemoverQtdCarrinho(int id)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string verificaQuantidadeSql = @"SELECT Quantidade FROM Carrinho WHERE Id_Carrinho = @id";
                MySqlCommand verificaCmd = new MySqlCommand(verificaQuantidadeSql, conn);
                verificaCmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = verificaCmd.ExecuteReader();

                int quantidade = 0;
                while (reader.Read())
                {
                    quantidade = reader.GetInt32("Quantidade");
                }
                reader.Close();

                if (quantidade <= 1)
                {
                    string deleteSql = @"DELETE FROM Carrinho WHERE Id_Carrinho = @id";
                    MySqlCommand deleteCmd = new MySqlCommand(deleteSql, conn);
                    deleteCmd.Parameters.AddWithValue("@id", id);
                    deleteCmd.ExecuteNonQuery();
                }
                else
                {
                    string updateSql = @"UPDATE Carrinho SET Quantidade = Quantidade - 1 WHERE Id_Carrinho = @id";
                    MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@id", id);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoverCarrinho(int id)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string deleteSql = @"DELETE FROM Carrinho WHERE Id_Carrinho = @id";
                MySqlCommand deleteCmd = new MySqlCommand(deleteSql, conn);
                deleteCmd.Parameters.AddWithValue("@id", id);
                deleteCmd.ExecuteNonQuery();
            }
        }

        public void AdicionarQtdCarrinho(int id)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string verificaQuantidadeSql = @"SELECT Quantidade FROM Carrinho WHERE Id_Carrinho = @id";
                MySqlCommand verificaCmd = new MySqlCommand(verificaQuantidadeSql, conn);
                verificaCmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = verificaCmd.ExecuteReader();

                int quantidade = 0;
                while (reader.Read())
                {
                    quantidade = reader.GetInt32("Quantidade");
                }
                reader.Close();

                if (quantidade <= 10)
                {
                    string updateSql = @"UPDATE Carrinho SET Quantidade = Quantidade + 1 WHERE Id_Carrinho = @id";
                    MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@id", id);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }

        public Cupom? BuscarCupomPorCodigo(string codigo)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = @"SELECT * FROM Cupons 
                               WHERE Codigo = @codigo 
                               AND Ativo = 1 
                               AND Expiracao >= NOW() 
                               LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Cupom
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Codigo = reader["Codigo"].ToString(),
                            Tipo = reader["Tipo"].ToString(),
                            Valor = Convert.ToDecimal(reader["Valor"]),
                            Expiracao = Convert.ToDateTime(reader["Expiracao"]),
                            Ativo = Convert.ToBoolean(reader["Ativo"])
                        };
                    }
                }
            }

            return null;
        }
    }
}