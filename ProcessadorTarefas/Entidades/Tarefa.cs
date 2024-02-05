namespace ProcessadorTarefas.Entidades
{
    public interface ITarefa
    {
        int Id { get; }
        EstadoTarefa Estado { get; set; }
        DateTime IniciadaEm { get; set; }
        DateTime EncerradaEm { get; set; }
        IEnumerable<Subtarefa> SubtarefasPendentes { get; set; }
        IEnumerable<Subtarefa> SubtarefasExecutadas { get; }
    }

    public class Tarefa : ITarefa
    {
        private static int _idCounter = 0;

        public Tarefa()
        {
            Id = Interlocked.Increment(ref _idCounter);
            Estado = EstadoTarefa.Criada;
            IniciadaEm = DateTime.Now;
            SubtarefasPendentes = GerarQuantidadeSubtarefas();
        }

        public int Id { get; private set; }
        public EstadoTarefa Estado { get; set; }

        public DateTime IniciadaEm { get; set; }
        public DateTime EncerradaEm { get; set; }
        public IEnumerable<Subtarefa> SubtarefasPendentes { get; set; }
        public int QuantSubtarefasProcessadas => Random.Shared.Next(10, 30);

        /// <summary>
        /// Gerador de subtarefas sendo executadas baseado em cada Subtarefa que contem a Duracao não zerada
        /// </summary>
        public IEnumerable<Subtarefa> SubtarefasExecutadas =>
            SubtarefasPendentes.TakeWhile(x => x.DuracaoZerada is false)
                .Take(QuantSubtarefasProcessadas);
        
        /// <summary>
        /// Gerador aleatório da quantidade de subtarefas a serem 'feitas' para a tarefa
        /// </summary>
        private int QuantidadeSubtarefas => Random.Shared.Next(10, 100);

        /// <summary>
        /// Gerador de subtarefas conforme a quantidade gerada aleatoriamente 
        /// </summary>
        public IEnumerable<Subtarefa> GerarQuantidadeSubtarefas() =>
            Enumerable.Range(1, QuantidadeSubtarefas)
                .Select(_ => new Subtarefa());
        
        /// <summary>
        /// Encerra todas a tarefa se todas as subtarefas foram finalizadas
        /// </summary>
        public void EncerrarTarefa()
        {
            if (SubtarefasPendentes.All(x => x.DuracaoZerada is true))
            {
                EncerradaEm = DateTime.Now;
                Estado = EstadoTarefa.Concluida;
            }
        }

        /// <summary>
        /// Cancela a tarefa, somente usada pelo ProcessadorTarefas
        /// </summary>
        public void CancelarTarefa()
        {
            EncerradaEm = DateTime.Now;
            Estado = EstadoTarefa.Cancelada;
        }
        
        // Metodos async abaixo
        
        public async Task RodarSubTarefas()
        {
            var updateTasks = SubtarefasExecutadas.Select(subtarefa => subtarefa.UpdateDurationAsync());
            await Task.WhenAll(updateTasks);
        }

    }
}