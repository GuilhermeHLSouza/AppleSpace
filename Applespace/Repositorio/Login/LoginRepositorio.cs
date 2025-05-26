using MySql.Data.MySqlClient;
using Applespace.Data;
using Applespace.Models;
using System.Data;

namespace Applespace.Repositorio.Login
{
    public class LoginRepositorio : ILoginRepositorio
    {
        private readonly Database _db;

        public LoginRepositorio(Database db)
        {
            _db = db;
        }

        public void AtualizarCliente(Clientes cliente)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = @"UPDATE Usuarios 
                               SET Nome = @nome, 
                                   Email = @email, 
                                   Cpf = @cpf, 
                                   Telefone = @telefone, 
                                   Senha = @senha 
                               WHERE Id_Usuario = @id";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@email", cliente.Email);
                cmd.Parameters.AddWithValue("@cpf", cliente.CPF);
                cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@senha", cliente.Senha);
                cmd.Parameters.AddWithValue("@id", cliente.IdCliente);

                cmd.ExecuteNonQuery();
            }
        }

        public void Cadastrar(Clientes cliente)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = @"INSERT INTO Usuarios (Nome, Email, Cpf, Telefone, Senha, Adm) 
                               VALUES (@nome, @email, @cpf, @telefone, @senha, @adm)";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@email", cliente.Email);
                cmd.Parameters.AddWithValue("@cpf", cliente.CPF);
                cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@senha", cliente.Senha);
                cmd.Parameters.AddWithValue("@adm", cliente.Adm);

                cmd.ExecuteNonQuery();
            }
        }

        public Clientes Login(string email, string senha)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = "SELECT * FROM Usuarios WHERE Email = @Email AND Senha = @Senha";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senha);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Clientes
                        {
                            IdCliente = Convert.ToInt32(reader["Id_Usuario"]),
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            Senha = reader["Senha"].ToString(),
                            Telefone = reader["Telefone"].ToString(),
                            CPF = reader["Cpf"].ToString(),
                            Adm = Convert.ToBoolean(reader["Adm"])
                        };
                    }
                }
            }

            return null;
        }

        public Clientes BuscarPorEmail(string email)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = "SELECT * FROM Usuarios WHERE Email = @Email";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Clientes
                        {
                            IdCliente = Convert.ToInt32(reader["Id_Usuario"]),
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            Senha = reader["Senha"].ToString(),
                            Telefone = reader["Telefone"].ToString(),
                            CPF = reader["Cpf"].ToString(),
                            Adm = Convert.ToBoolean(reader["Adm"])
                        };
                    }
                }
            }

            return null;
        }
    }
}
