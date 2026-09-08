using System.ComponentModel.DataAnnotations;

namespace MyPDV.Dtos;

public sealed class CriarProdutoRequest
{
    [Required]
    public string Nome { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue)]
    public int Estoque { get; set; }
}
