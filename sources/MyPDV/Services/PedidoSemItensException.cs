namespace MyPDV.Services;

public sealed class PedidoSemItensException(int id)
    : Exception($"O pedido {id} não possui itens e não pode ser fechado.");
