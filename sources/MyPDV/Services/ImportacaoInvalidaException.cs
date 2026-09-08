namespace MyPDV.Services;

public sealed class ImportacaoInvalidaException(string mensagem) : Exception(mensagem);
