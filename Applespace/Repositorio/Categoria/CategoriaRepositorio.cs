using Applespace.Data;
using Applespace.Models;
using MySql.Data.MySqlClient;

namespace Applespace.Repositorio.Categoria
{
    public class CategoriaRepositorio
    {
        Database _db = new Database();
        public IEnumerable<Categorias> MostrarCate()
        {
            List<Categorias> CateList = new List<Categorias>();

            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = "SELECT * FROM Produtos";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CateList.Add(new Categorias
                        {
                            idCategoria = reader.GetInt32("Id_Cate"),
                            nomeCategoria = reader.GetString("Nome_Cate"),
                        });
                    }
                }
            }


            return CateList;
        }
    }
}
