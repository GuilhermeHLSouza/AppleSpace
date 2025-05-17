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

        public void AtualizarCliente(Clientes clientes)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = @"UPDATE Clientes 
                       SET Nome = @nome, 
                           Email = @email, 
                           Cpf = @cpf, 
                           Telefone = @telefone, 
                           Senha = @senha
                       WHERE Id_Cliente = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = clientes.nome;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = clientes.email;
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = clientes.CPF;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = clientes.telefone;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = clientes.senha;
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = clientes.idCliente;

                cmd.ExecuteNonQuery();
            }
        }

        public void cadastrar(Clientes cliente)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string Sql = @"insert into Clientes (Nome, Email, Cpf, Telefone, Senha)
                                values (@nome, @email, @cpf, @telefone, @senha)";
                MySqlCommand cmd = new MySqlCommand(Sql, conn);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cliente.nome;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.email;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = cliente.telefone;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = cliente.senha;
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cliente.CPF;
                cmd.ExecuteNonQuery();
            }
        }

        public Clientes Login(string email, string senha)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = "select * from Clientes where Email = @email and Senha = @senha";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = senha;

                MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                Clientes Adm = new Clientes();

                while (dr.Read())
                {
                    Adm.email = Convert.ToString(dr["Email"]);
                    Adm.senha = Convert.ToString(dr["Senha"]);
                    Adm.idCliente = dr.GetInt32("Id_Cliente");
                }
                return Adm;
            }
        }

        public Administradores LoginAdm(string nome, string senha, int id)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = "select * from Administradores where Nome = @nome and Senha = @senha and Id_Adm = @idAdm";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = senha;
                cmd.Parameters.Add("@idAdm", MySqlDbType.Int32).Value = id;

                MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                Administradores Adm = new Administradores();

                while (dr.Read())
                {
                    Adm.IdAdm = Convert.ToInt32(dr["Id_Adm"]);
                    Adm.Nome = Convert.ToString(dr["Nome"]);
                    Adm.Senha = Convert.ToString(dr["Senha"]);
                }
                return Adm;
            }
        }
    }
}
