using MyPDV.Schemas.Enums;

namespace MyPDV.Schemas.Entities;

public sealed class Pedido
{
    public int Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Aberto;
    public decimal Total { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;
    public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}
