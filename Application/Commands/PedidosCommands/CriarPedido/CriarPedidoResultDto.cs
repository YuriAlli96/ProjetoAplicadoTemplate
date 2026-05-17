using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.PedidosCommands.CriarPedido
{
    public sealed class CriarPedidoResultDto
    {
        public string NumeroPedido { get; }
        public DateTime DataCriacao { get; }
        public decimal ValorTotal { get; }

        public CriarPedidoResultDto(
            string numeroPedido,
            DateTime dataCriacao,
            decimal valorTotal)
        {
            NumeroPedido = numeroPedido;
            DataCriacao = dataCriacao;
            ValorTotal = valorTotal;
        }
    }
}
