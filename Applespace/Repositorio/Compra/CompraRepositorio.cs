using Applespace.Data;
using Applespace.Models;
using MySql.Data.MySqlClient;

namespace Applespace.Repositorio.Compra
{
    public class CompraRepositorio : ICompraRepositorio
    {
        private readonly Database _db;

        public CompraRepositorio(Database db)
        {
            _db = db;
        }

        public void RegistroEndereco(int idCliente, int cep, int numero, string rua, string bairro, string complemento)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = @"INSERT INTO Enderecos 
                               (CEP, Numero, Rua, Bairro, Complemento, Id_Usuario) 
                               VALUES 
                               (@cep, @num, @rua, @bairro, @complemento, @idUsuario)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cep", cep);
                cmd.Parameters.AddWithValue("@num", numero);
                cmd.Parameters.AddWithValue("@rua", rua);
                cmd.Parameters.AddWithValue("@bairro", bairro);
                cmd.Parameters.AddWithValue("@complemento", complemento);
                cmd.Parameters.AddWithValue("@idUsuario", idCliente);

                cmd.ExecuteNonQuery();
            }
        }

        public void SelectUsuario(Clientes cliente)
        {
            if (cliente != null)
            {
                using (MySqlConnection conn = _db.GetConnection())
                {
                    string sql = @"SELECT * FROM Usuarios WHERE Email = @email AND Senha = @senha";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@email", cliente.Email);
                    cmd.Parameters.AddWithValue("@senha", cliente.Senha);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente.IdCliente = reader.GetInt32("Id_Usuario");
                            cliente.Senha = reader.GetString("Senha");
                            cliente.Nome = reader.GetString("Nome");
                            cliente.Email = reader.GetString("Email");
                            cliente.CPF = reader.GetString("Cpf");
                            cliente.Telefone = reader.GetString("Telefone");
                        }
                    }
                }
            }
        }

        public void Venda(Pedido pedido)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = @"INSERT INTO Pedido (Forma_Pgm, Statu, Id_Carrinho) 
                       VALUES (@forma, @status, @idCarrinho)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@forma", pedido.FormaPagamento);
                cmd.Parameters.AddWithValue("@status", pedido.Status);
                cmd.Parameters.AddWithValue("@idCarrinho", pedido.IdCarrinho);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
