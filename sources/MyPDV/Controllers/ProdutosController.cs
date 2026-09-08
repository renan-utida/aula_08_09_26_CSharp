using Microsoft.AspNetCore.Mvc;
using MyPDV.Dtos;
using MyPDV.Services;

namespace MyPDV.Controllers;

[ApiController]
[Route("api/produtos")]
public sealed class ProdutosController(IProdutoService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProdutoResponse>>> Listar(
        CancellationToken cancellationToken)
    {
        return Ok(await service.ListarAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoResponse>> Obter(
        int id,
        CancellationToken cancellationToken)
    {
        ProdutoResponse? produto = await service.ObterAsync(id, cancellationToken);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoResponse>> Criar(
        CriarProdutoRequest request,
        CancellationToken cancellationToken)
    {
        ProdutoResponse produto = await service.CriarAsync(
            request,
            cancellationToken);

        return CreatedAtAction(nameof(Obter), new { id = produto.Id }, produto);
    }
}
