using API.Dtos;
using Application.Commands.PedidosCommands.CriarPedido;
using Domain.Pedido.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;

namespace API.Controllers.Pedidos
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PedidosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] PedidoRequestViewModel request)
        {
            var command = new CriarPedidoCommand(EnderecoEntrega.Criar(request.Cep, request.Bairro, request.Estado, request.Cidade, request.Complemento));

            var pedidoId = await _mediator.Send(command);

            return Ok(new { id = pedidoId });
        }
    }
}
