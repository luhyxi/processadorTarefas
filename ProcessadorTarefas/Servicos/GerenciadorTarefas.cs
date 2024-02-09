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

        /// <summary>
        /// Cria uma nova tarefa.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
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

        /// <summary>
        /// Consulta uma tarefa pelo ID.
        /// </summary>
        /// <param name="idTarefa">O ID da tarefa.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O valor da tarefa é a Tarefa encontrada.</returns>
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

        /// <summary>
        /// Cancela uma tarefa pelo ID.
        /// </summary>
        /// <param name="idTarefa">O ID da tarefa.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O valor da tarefa é a Tarefa cancelada.</returns>
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

        /// <summary>
        /// Lista as tarefas ativas.
        /// </summary>
        /// <param name="_repositorio">O repositório de tarefas.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O valor da tarefa é a lista de Tarefas ativas.</returns>
        public Task<IEnumerable<Tarefa>> ListarAtivas(IRepository<Tarefa> _repositorio) =>
            Task.FromResult(_repositorio.GetAll()
                .Where(tarefa => tarefa.Estado == EstadoTarefa.EmExecucao || tarefa.Estado == EstadoTarefa.EmPausa));

        /// <summary>
        /// Lista as tarefas inativas.
        /// </summary>
        /// <param name="_repositorio">O repositório de tarefas.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O valor da tarefa é a lista de Tarefas inativas.</returns>
        public Task<IEnumerable<Tarefa>> ListarInativas(IRepository<Tarefa> _repositorio) =>
            Task.FromResult(_repositorio.GetAll()
                .Where(tarefa => tarefa.Estado == EstadoTarefa.Cancelada || tarefa.Estado == EstadoTarefa.Concluida));

        /// <summary>
        /// Começa a fazer todas as subtarefas do repositorio.
        /// </summary>
        /// <returns>Começa a rodar todas as Subtarefas.</returns>
        
        public async Task RodarTodasSubtarefasAsync()
        {
            var tarefas = Repositorio.GetAll().ToList();
            foreach (var tarefa in tarefas)
            {
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
                    // Executa as subtarefas da tarefa
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

            
        /// <summary>
        /// Cancela todas tarefas do repositorio.
        /// </summary>
        public Task CancelaTodasTarefas()
        {
            foreach (var tarefa in Repositorio.GetAll())
            {
                Cancelar(tarefa.Id);
            }
            return Task.CompletedTask;
        }
        
    }
}