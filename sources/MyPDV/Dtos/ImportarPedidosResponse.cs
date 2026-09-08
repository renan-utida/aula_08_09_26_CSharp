namespace MyPDV.Dtos;

public sealed class ImportarPedidosResponse
{
    public int Quantidade { get; set; }

    public List<int> Ids { get; set; } = [];
}
