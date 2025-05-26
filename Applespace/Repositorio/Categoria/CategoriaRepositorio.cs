using Applespace.Data;
using Applespace.Models;
using MySql.Data.MySqlClient;

namespace Applespace.Repositorio.Categoria
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly Database _db;

        public CategoriaRepositorio(Database db)
        {
            _db = db;
        }

        public IEnumerable<Categorias> MostrarCate()
        {
            List<Categorias> CateList = new List<Categorias>();

            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = "SELECT * FROM Categorias";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CateList.Add(new Categorias
                        {
                            IdCategoria = reader.GetInt32("Id_Cate"),
                            NomeCategoria = reader.GetString("Nome_Cate"),
                        });
                    }
                }
            }

            return CateList;
        }
    }
}