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
    public async Task<ActionResult<PedidoResponse>> Atualizar(
        int id,
        AtualizarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            PedidoResponse? pedido = await service.AtualizarAsync(id, request, cancellationToken);
            return pedido is null ? NotFound() : Ok(pedido);
        }
        catch (PedidoFechadoException)
        {
            return Conflict();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desativar(
        int id,
        CancellationToken cancellationToken)
    {
        bool desativado = await service.DesativarAsync(id, cancellationToken);
        return desativado ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/fechar")]
    public async Task<ActionResult<PedidoResponse>> Fechar(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            PedidoResponse? pedido = await service.FecharAsync(id, cancellationToken);
            return pedido is null ? NotFound() : Ok(pedido);
        }
        catch (PedidoFechadoException)
        {
            return Conflict();
        }
        catch (PedidoSemItensException)
        {
            return BadRequest();
        }
    }

    [HttpPost("importacao")]
    public async Task<ActionResult<ImportarPedidosResponse>> Importar(
        IReadOnlyCollection<CriarPedidoRequest> requests,
        CancellationToken cancellationToken)
    {
        try
        {
            ImportarPedidosResponse resultado = await service.ImportarAsync(requests, cancellationToken);
            return Ok(resultado);
        }
        catch (ImportacaoInvalidaException excecao)
        {
            return BadRequest(excecao.Message);
        }
    }

    [HttpGet("resumo")]
    public ActionResult ObterResumo()
    {
        throw new NotImplementedException();
    }
}
