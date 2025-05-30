﻿namespace Applespace.Models
{
    public class Cupom
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Tipo { get; set; } // porcentagem, frete ou valorfixo
        public decimal Valor { get; set; }
        public DateTime Expiracao { get; set; }
        public bool Ativo { get; set; }
    }
}