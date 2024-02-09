namespace ProcessadorTarefas.Servicos
{
    internal interface IProcessadorTarefas
    {
        Task Iniciar();
        Task Encerrar();
    }
}
