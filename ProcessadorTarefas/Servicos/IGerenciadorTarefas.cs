using System.Data;
using ProcessadorTarefas.Entidades;
using SOLID_Example.Interfaces;

namespace ProcessadorTarefas.Servicos
{
    internal interface IGerenciadorTarefas
    {
        Task Criar();
        Task<Tarefa> Consultar(int idTarefa);
        Task Cancelar(int idTarefa);
        Task<IEnumerable<Tarefa>> ListarAtivas(IRepository<Tarefa> repositorio);
        Task<IEnumerable<Tarefa>> ListarInativas(IRepository<Tarefa> repositorio);
    }


}
