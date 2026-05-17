using Application.Commands.PedidosCommands.CriarPedido;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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

        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObterPedido(long id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoCommand command)
        {
            var pedidoId = await _mediator.Send(command);

            return CreatedAtAction(nameof(ObterPedido), new { id = pedidoId }, new { Id = pedidoId });
        }
    }
}
