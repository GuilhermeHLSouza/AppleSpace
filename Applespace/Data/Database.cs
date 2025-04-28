using MySql.Data.MySqlClient;

namespace Applespace.Data
{
    public class Database
    {
        // Valor fixo "CONST" servindo como conexão para  banco de dados
        private readonly string connectionString = "server=localhost;port=3306;database=DbAppleSpace;user=root;password=12345678";

        public MySqlConnection GetConnection()
        {
            // Testando a conexão
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}

