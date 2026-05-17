using Application.Abstractions.Persistence;
using Domain.Pedido.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.PedidosCommands.CriarPedido
{
    internal class CriarPedidoCommandHandler : IRequestHandler<CriarPedidoCommand, CriarPedidoResultDto>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public CriarPedidoCommandHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<CriarPedidoResultDto> Handle(CriarPedidoCommand command, CancellationToken cancellationToken)
        {
            var pedido = Pedido.Criar(
                command.EnderecoEntrega
            );

            await _pedidoRepository.AdicionarAsync(pedido, cancellationToken);

            return new CriarPedidoResultDto(
                pedido.NumeroPedido,
                pedido.DataCriacao,
                pedido.ValorTotal
            );
        }
    }
}
