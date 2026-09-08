using System.ComponentModel.DataAnnotations;

namespace MyPDV.Dtos;

public sealed class CriarItemPedidoRequest
{
    [Required]
    public string Produto { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantidade { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal ValorUnitario { get; set; }
}
