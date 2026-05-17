using Application.Abstractions.Persistence;
using Domain.Pedido.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        public Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Pedido?> ObterPorIdAsync(long pedidoId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
