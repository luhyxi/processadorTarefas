using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Servicos;

namespace ProcessadorTarefas.Development_Stuff;

internal class ProcessadorTarefas : IProcessadorTarefas
{
    private List<Tarefa> tarefas = new List<Tarefa>();

    public async Task Iniciar()
    {
        // Implement logic to start tasks asynchronously
        foreach (var tarefa in tarefas)
        {
            await IniciarTarefaAsync();
        }
    }

    public async Task CancelarTarefa(int idTarefa)
    {
        // Implement logic to cancel a specific task asynchronously
        var tarefa = tarefas.FirstOrDefault(t => t.Id == idTarefa);
        if (tarefa != null)
        {
            await CancelarTarefaAsync(tarefa);
        }
    }

    public async Task Encerrar()
    {
        // Implement logic to asynchronously close tasks
        foreach (var tarefa in tarefas)
        {
            await EncerrarTarefaAsync();
        }
    }

    // Additional methods to perform the asynchronous operations on Tarefa instances
    private async Task IniciarTarefaAsync()
    {
        var tarefaCriada = new Tarefa();
        tarefas.Add(tarefaCriada);
    }

    private async Task CancelarTarefaAsync(Tarefa tarefa)
    {

    }

    private async Task EncerrarTarefaAsync()
    {
        var tarefasTerminadas = tarefas.Where(x => x.Estado == EstadoTarefa.Concluida).ToList();
        tarefas.RemoveAll(x => tarefasTerminadas.Contains(x));
    }
}