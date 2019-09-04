﻿namespace AgendaMedica.Domain.Entities
{
    public class Endereco
    {
        public int EnderecoId { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Rua { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public int Numero { get; set; }
    }
}