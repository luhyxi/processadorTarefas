using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProcessadorTarefas.Development_Stuff;

namespace ProcessadorTarefas.Entidades
{
    public interface ITarefa
    {
        int Id { get; }
        EstadoTarefa Estado { get; set; }
        DateTime IniciadaEm { get; set; }
        DateTime? EncerradaEm { get; set; }
        IEnumerable<Subtarefa> SubtarefasPendentes { get; set; }
        IEnumerable<Subtarefa> SubtarefasExecutadas { get; }
    }

    public class Tarefa : ITarefa
    {
        private static readonly object IdLock = new object();
        private static int _idCounter = 0;

        public Tarefa()
        {
            lock (IdLock)
            {
                Id = ++_idCounter;
            }

            Estado = EstadoTarefa.Criada;
            IniciadaEm = DateTime.Now;
            SubtarefasIniciais = GerarQuantidadeSubtarefas().ToList();
            foreach (var subtarefa in SubtarefasIniciais)
            {
                subtarefa.CountdownFinished += OnSubtarefaCountdownFinished;
            }
        }

        public int Id { get; private set; }
        public EstadoTarefa Estado { get; set; }
        public DateTime IniciadaEm { get; set; }
        public DateTime? EncerradaEm { get; set; }
        public IEnumerable<Subtarefa> SubtarefasPendentes { get; set; }

        public IEnumerable<Subtarefa> SubtarefasConcluidas =>
            SubtarefasPendentes?.Where(x => x.DuracaoSeconds == 0) ?? Enumerable.Empty<Subtarefa>();
        public IEnumerable<Subtarefa> SubtarefasNaoConcluidas =>
            SubtarefasPendentes?.Where(x => x.DuracaoSeconds > 0) ?? Enumerable.Empty<Subtarefa>();
        public IEnumerable<Subtarefa> SubtarefasIniciais { get; private set; }

        public IEnumerable<Subtarefa> SubtarefasExecutadas =>
            SubtarefasPendentes?.Where(x => x.DuracaoSeconds > 0).Take(QuantSubtarefasProcessadas) ?? Enumerable.Empty<Subtarefa>();


        private int QuantSubtarefasProcessadas => Math.Min(3, SubtarefasPendentes.Count(x => x.DuracaoSeconds > 0));

        private IEnumerable<Subtarefa> GerarQuantidadeSubtarefas()
        {
            var random = new Random();
            return Enumerable.Range(1, random.Next(1, 30)).Select(_ => new Subtarefa());
        }

        private void OnSubtarefaCountdownFinished(object sender, EventArgs e)
        {
            if (sender is Subtarefa subtarefa && SubtarefasPendentes.All(x => x.DuracaoSeconds == 0))
            {
                EncerrarTarefa();
            }
        }

        public async Task RunSubtarefas()
        {
            try
            {
                // Ensure SubtarefasPendentes is initialized
                if (SubtarefasPendentes == null)
                {
                    throw new InvalidOperationException("SubtarefasPendentes is null.");
                }

                // Start countdown for each subtask
                foreach (var subtarefa in SubtarefasIniciais)
                {
                    subtarefa.CountdownFinished += OnSubtarefaCountdownFinished;
                    await Task.Run(() => subtarefa.RunCountdown());
                }

                // Wait for all subtasks to complete
                while (SubtarefasPendentes.Any())
                {
                    await Task.Delay(100); // Adjust delay as needed
                }

                // Once all subtasks are completed, encerrar a tarefa
                EncerrarTarefa();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while running subtasks: {ex}");
                throw;
            }
        }

        public void EncerrarTarefa()
        {
            EncerradaEm = DateTime.Now;
            Estado = EstadoTarefa.Concluida;
        }

        public void CancelarTarefa()
        {
            EncerradaEm = DateTime.Now;
            Estado = EstadoTarefa.Cancelada;
        }
    }
}