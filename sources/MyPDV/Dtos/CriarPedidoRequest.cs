using System.ComponentModel.DataAnnotations;

namespace MyPDV.Dtos;

public sealed class CriarPedidoRequest
{
    [Required]
    public string Cliente { get; set; } = string.Empty;

    [MinLength(1)]
    public List<CriarItemPedidoRequest> Itens { get; set; } = [];
}
