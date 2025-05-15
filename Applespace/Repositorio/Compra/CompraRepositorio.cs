using Applespace.Data;
using Applespace.Libraries.LoginClientes;
using Applespace.Models;
using MySql.Data.MySqlClient;

namespace Applespace.Repositorio.Compra
{
    public class CompraRepositorio : ICompraRepositorio
    {
        private readonly Database db = new Database();
        public void Venda()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                string sql = @"INSERT INTO Vendas (CEP, Numero, Rua, Bairro, Complemeto, Id_Cliente)
                            VALUES (@cep, @num, @rua, @bairro, @complemento, @idCliente)"
            }
        }

        public void RegistroEndereço(int idCliente, int cep, int numero, string rua, string bairro, string complemento)
        {
            using(MySqlConnection conn = db.GetConnection())
            {
                string sql = @"INSERT INTO Enderecos (CEP, Numero, Rua, Bairro, Complemeto, Id_Cliente)
                            VALUES (@cep, @num, @rua, @bairro, @complemento, @idCliente)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cep", cep);
                cmd.Parameters.AddWithValue("@num", numero);
                cmd.Parameters.AddWithValue("@rua", rua);
                cmd.Parameters.AddWithValue("@bairro", bairro);
                cmd.Parameters.AddWithValue("@complemento", complemento);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);

            }
        }

        public void SelectUsuario(Clientes cliente)
        {

            if (cliente != null)
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    string sql = @"SELECT * FROM Clientes WHERE Clientes.Email = @email and Clientes.Senha = @senha";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@email", cliente.email);
                    cmd.Parameters.AddWithValue("@senha", cliente.senha);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Clientes
                            {
                                idCliente = reader.GetInt32("Id_Cliente"),
                                senha = reader.GetString("Senha"),
                                nome = reader.GetString("Nome"),
                                email = reader.GetString("Email"),
                                CPF = reader.GetInt32("Cpf"),
                                telefone = reader.GetInt32("Telefone")
                            };
                        }
                    }
                }
            }
        }
    }
}
