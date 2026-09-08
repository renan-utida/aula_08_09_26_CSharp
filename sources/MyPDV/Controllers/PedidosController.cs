using Microsoft.AspNetCore.Mvc;
using MyPDV.Dtos;
using MyPDV.Services;

namespace MyPDV.Controllers;

[ApiController]
[Route("api/pedidos")]
public sealed class PedidosController(IPedidoService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PedidoResponse>>> Listar(
        CancellationToken cancellationToken)
    {
        return Ok(await service.ListarAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoResponse>> Obter(
        int id,
        CancellationToken cancellationToken)
    {
        PedidoResponse? pedido = await service.ObterAsync(id, cancellationToken);
        return pedido is null ? NotFound() : Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<PedidoResponse>> Criar(
        CriarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        PedidoResponse pedido = await service.CriarAsync(
            request,
            cancellationToken);

        return CreatedAtAction(nameof(Obter), new { id = pedido.Id }, pedido);
    }

    [HttpPatch("{id:int}")]
    public ActionResult<PedidoResponse> Atualizar(
        int id,
        AtualizarPedidoRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Desativar(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id:int}/fechar")]
    public ActionResult<PedidoResponse> Fechar(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("importacao")]
    public ActionResult Importar(IReadOnlyCollection<CriarPedidoRequest> requests)
    {
        throw new NotImplementedException();
    }

    [HttpGet("resumo")]
    public ActionResult ObterResumo()
    {
        throw new NotImplementedException();
    }
}
