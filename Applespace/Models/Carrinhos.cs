namespace Applespace.Models
{
    public class Carrinhos
    {
        public int IdCarrinho { get; set; }
        public int Quantidade { get; set; }
        public double Valor { get; set; }
        public int CodBarra { get; set; }
        public int IdCliente { get; set; }

        public Produtos Produtos { get; set; }
    }
}
