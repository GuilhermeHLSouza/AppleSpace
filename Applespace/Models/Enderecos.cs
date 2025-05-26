namespace Applespace.Models
{
    public class Enderecos
    {
        public int IdEndereco { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }

        public int IdUsuario { get; set; }
        public Clientes Usuario { get; set; }
    }
}