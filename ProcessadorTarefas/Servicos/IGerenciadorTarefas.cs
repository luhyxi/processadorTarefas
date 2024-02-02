using System.Data;
using ProcessadorTarefas.Entidades;
using SOLID_Example.Interfaces;

namespace ProcessadorTarefas.Servicos
{
    internal interface IGerenciadorTarefas
    {
        Task<Tarefa> Criar();
        Task<Tarefa> Consultar(int idTarefa);
        Task Cancelar(int idTarefa);
        Task<IEnumerable<Tarefa>> ListarAtivas(IRepository<Tarefa> repositorio);
        Task<IEnumerable<Tarefa>> ListarInativas(IRepository<Tarefa> repositorio);
    }

    public class GerenciadorTarefas : IGerenciadorTarefas
    {
        public Task<Tarefa> Criar()
        {
            throw new NotImplementedException();
        }

        public Task<Tarefa> Consultar(int idTarefa)
        {
            throw new NotImplementedException();
        }

        public Task Cancelar(int idTarefa)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Tarefa>> ListarAtivas(IRepository<Tarefa> repositorio) => 
            Task.FromResult(repositorio.GetAll()
                .Where(tarefa => tarefa.Estado == EstadoTarefa.EmExecucao || tarefa.Estado == EstadoTarefa.EmPausa));
        public Task<IEnumerable<Tarefa>> ListarInativas(IRepository<Tarefa> repositorio) =>
            Task.FromResult(repositorio.GetAll()
                .Where(tarefa => tarefa.Estado == EstadoTarefa.Cancelada || tarefa.Estado == EstadoTarefa.Concluida));
    }
}
