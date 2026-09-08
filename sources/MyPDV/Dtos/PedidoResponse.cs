using MyPDV.Schemas.Enums;

namespace MyPDV.Dtos;

public sealed class PedidoResponse
{
    public int Id { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public Status Status { get; set; }

    public decimal Total { get; set; }

    public DateTime CriadoEm { get; set; }

    public List<ItemPedidoResponse> Itens { get; set; } = [];
}
