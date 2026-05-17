namespace API.Controllers.Pedidos
{
    public class PedidoRequestDto
    {
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
    }
}
