
namespace Applespace.Models
{
    public class Produtos
    {
        public int codBarra { get; set; }
        public string? descricao { get; set; }
        public decimal valor { get; set; }
        public string? nome { get; set; }
        public int estoque { get; set; }
        public string? img { get; set; }
        public bool emDestaque { get; set; }

        public int idCate { get; set; }
        public Categorias? Categoria { get; set; }

        public int Id_Cate { get; set; }

    }
}
