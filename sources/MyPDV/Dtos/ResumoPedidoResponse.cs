using MyPDV.Schemas.Enums;

namespace MyPDV.Dtos;

public sealed class ResumoPedidoResponse
{
    public Status Status { get; set; }

    public int Quantidade { get; set; }

    public decimal ValorTotal { get; set; }
}
