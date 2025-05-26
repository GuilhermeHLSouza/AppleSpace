namespace Applespace.Models
{
    public class Produtos
    {
        public int CodBarra { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Estoque { get; set; }
        public string Img { get; set; }
        public bool EmDestaque { get; set; }
        public int IdCate { get; set; }

        public int IdCategoria { get; set; }  // Chave estrangeira
        public Categorias Categoria { get; set; }  // Propriedade de navegação
    }
}
