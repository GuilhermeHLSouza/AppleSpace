namespace Applespace.Models
{
    public class Vendas
    {
        public int idVenda { get; set; }
        public string formPgm {  get; set; }
        public string status {  get; set; }
        public int idCarrinho {  get; set; }
        public Carrinhos Carrinhos { get; set; }
    }
}
