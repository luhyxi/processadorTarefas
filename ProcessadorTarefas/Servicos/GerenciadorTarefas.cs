using ProcessadorTarefas.Development_Stuff;
using ProcessadorTarefas.Entidades;
using SOLID_Example.Interfaces;

namespace ProcessadorTarefas.Entity.Servicos
{
    /// <summary>
    /// Gerencia as operações de tarefas.
    /// </summary>
    public class GerenciadorTarefas : IGerenciadorTarefas
    {
        public IRepository<Tarefa> Repositorio;

        /// <summary>
        /// Inicializa uma nova instância da classe GerenciadorTarefas.
        /// </summary>
        /// <param name="repositorio">O repositório de tarefas.</param>
        public GerenciadorTarefas(IRepository<Tarefa> repositorio)
        {
            Repositorio = repositorio;
        }
        
        public Task Criar()
        {
            try
            {
                Tarefa newTarefa = new Tarefa();
                Repositorio.Add(newTarefa);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine($"Ocorreu o seguinte erro ao tentar criar a tarefa: {e}");
                
                throw;
            }
        }


        public Task<Tarefa> Consultar(int idTarefa)
        {
            try
            {
                Tarefa foundTarefa = Repositorio.GetById(idTarefa);
                return Task.FromResult(foundTarefa);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocorreu o seguinte erro ao tentar consultar a tarefa: {e}");
                throw;
            }
        }


        public Task Cancelar(int idTarefa)
        {
            var tarefaConsultada = Consultar(idTarefa).Result;
            Repositorio.Update(tarefaConsultada, tarefa =>
            {
                tarefa.CancelarTarefa();
                return tarefa;
            });
            return Task.FromResult(tarefaConsultada);
        }

        public Task<IEnumerable<Tarefa>> ListarAtivas(IRepository<Tarefa> _repositorio) =>
            Task.FromResult(_repositorio.GetAll()
                .Where(tarefa => tarefa.Estado == EstadoTarefa.EmExecucao || tarefa.Estado == EstadoTarefa.EmPausa));
        
        public Task<IEnumerable<Tarefa>> ListarInativas(IRepository<Tarefa> _repositorio) =>
            Task.FromResult(_repositorio.GetAll()
                .Where(tarefa => tarefa.Estado == EstadoTarefa.Cancelada || tarefa.Estado == EstadoTarefa.Concluida));
        
        
        public async Task RodarTodasSubtarefasAsync()
        {
            var tarefas = Repositorio.GetAll().ToList();
            foreach (var tarefa in tarefas)
            {
                await RodarSubtarefasPorId(tarefa.Id);
            }
        }
        
        public async Task RodarSubtarefasPorId(int idTarefa)
        {
            try
            {
                // Consulta a tarefa pelo ID
                var tarefa = await Consultar(idTarefa);

                // Verifica se a tarefa foi encontrada
                if (tarefa != null)
                {
                    tarefa.AdicionarSubtarefasPendentes();
                    await tarefa.RunSubtarefas();
                }
                else
                {
                    Console.WriteLine($"Tarefa com ID {idTarefa} não encontrada.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocorreu o seguinte erro ao tentar rodar as subtarefas da tarefa com ID {idTarefa}: {e}");
                throw;
            }
        }
        
        public async Task CancelaTodasTarefas()
        {
            try
            {
                // Obtém todas as tarefas do repositório
                var tarefas = Repositorio.GetAll();
                // Percorre cada tarefa e chama o método Cancelar
                foreach (var tarefa in tarefas)
                {
                    await Cancelar(tarefa.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocorreu o seguinte erro ao tentar cancelar todas as tarefas: {e}");
                throw;
            }
        }


    }
}