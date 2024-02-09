namespace ProcessadorTarefas.Entity.Servicos
{
    internal interface IProcessadorTarefas
    {
        Task Iniciar();
        Task Encerrar();
    }
}
