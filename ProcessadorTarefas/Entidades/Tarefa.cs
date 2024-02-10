using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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

    [DebuggerDisplay("Id = {Id}, Estado = {Estado}")]
    public class Tarefa : ITarefa
    {
        private static int _idCounter = 0;

        public Tarefa()
        {
            Id = Interlocked.Increment(ref _idCounter);
            Estado = EstadoTarefa.Criada;
            IniciadaEm = DateTime.Now;
            SubtarefasPendentes = new List<Subtarefa>();
            SubtarefasExecutadas = new List<Subtarefa>();
        }

        public int Id { get; private set; }
        public EstadoTarefa Estado { get; set; }
        public DateTime IniciadaEm { get; set; }
        public DateTime? EncerradaEm { get; set; }
        public IEnumerable<Subtarefa> SubtarefasPendentes { get; set; }
        public IEnumerable<Subtarefa> SubtarefasExecutadas { get; set; }

        private const int MaxSubtarefasProcessadas = 3;
        private const int MaxSubtarefasDuranteCriacao = 20;

        public void AdicionarSubtarefasPendentes()
        {
            if (Estado == EstadoTarefa.Concluida || Estado == EstadoTarefa.Cancelada)
            {
                return;
            }
            var random = new Random();
            var subtarefasList = (List<Subtarefa>)SubtarefasPendentes;
            subtarefasList.AddRange(Enumerable.Range(1, random.Next(1, MaxSubtarefasDuranteCriacao)).Select(_ => new Subtarefa()));
            SubtarefasPendentes = subtarefasList;
        }


        private void OnSubtarefaCountdownFinished(object sender, EventArgs e)
        {
            var subtarefa = sender as Subtarefa;
            if (subtarefa != null)
            {
                SubtarefasPendentes.Where(s => s.IsFinished).ToList();
            }

            if (!SubtarefasPendentes.Any())
            {
                EncerrarTarefa();
            }
            else
            {
                RunSubtarefas();
            }
        }

        
        public async Task RunSubtarefas()
        {
            try
            {
                if (Estado == EstadoTarefa.Concluida || Estado == EstadoTarefa.Cancelada)
                {
                    return;
                }
                if (Estado != EstadoTarefa.Criada && Estado != EstadoTarefa.EmPausa)
                {
                    var subtarefasList = SubtarefasPendentes.ToList();
                    subtarefasList.RemoveAll(s => s.IsFinished);
                    SubtarefasPendentes = subtarefasList;
                }

                if (!SubtarefasPendentes.Any())
                {
                    EncerrarTarefa();
                    return;
                }

                MudarEstado(EstadoTarefa.EmExecucao);

                var subtarefasExecutadas = SubtarefasPendentes.OrderBy(s => s.DuracaoSeconds).Take(MaxSubtarefasProcessadas);

                foreach (var subtarefa in subtarefasExecutadas)
                {
                    subtarefa.CountdownFinished += OnSubtarefaCountdownFinished;
                    subtarefa.RunCountdown();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while running subtasks: {ex}");
                throw;
            }
        }

        private void MudarEstado(EstadoTarefa novoEstado)
        {
            Estado = novoEstado;
        }

        private void EncerrarTarefa()
        {
            Estado = EstadoTarefa.Concluida;
            EncerradaEm = DateTime.Now;
        }
        public void CancelarTarefa()
        {
            MudarEstado(EstadoTarefa.Cancelada);
            EncerradaEm = DateTime.Now;
        }
    }
}