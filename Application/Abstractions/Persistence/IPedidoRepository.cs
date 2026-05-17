using Domain.Pedido.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions.Persistence
{
    public interface IPedidoRepository
    {
        Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default);
    }
}
