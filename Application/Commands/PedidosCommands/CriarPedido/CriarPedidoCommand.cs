using Domain.Pedido.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.PedidosCommands.CriarPedido
{
    public sealed class CriarPedidoCommand : IRequest<CriarPedidoResultDto>
    {
        public EnderecoEntrega EnderecoEntrega { get; }

        public CriarPedidoCommand(
            EnderecoEntrega enderecoEntrega)
        {
            EnderecoEntrega = enderecoEntrega;
        }
    }
}
