﻿using static ProcessadorTarefas.Entidades.Tarefa;

namespace ProcessadorTarefas.Entidades
{
    public interface ITarefa
    {
        int Id { get; }
        EstadoTarefa Estado { get; }
        DateTime IniciadaEm { get; }
        DateTime EncerradaEm { get; }
        IEnumerable<Subtarefa> SubtarefasPendentes { get; }
        IEnumerable<Subtarefa> SubtarefasExecutadas { get; }
    }

    public class Tarefa : ITarefa
    {
        public Tarefa(IEnumerable<Subtarefa> subtarefasPendentes, IEnumerable<Subtarefa> subtarefasExecutadas)
        {
            Id = Interlocked.Increment(ref _idCounter);
            SubtarefasPendentes = subtarefasPendentes;
            SubtarefasExecutadas = subtarefasExecutadas;
        }

        static private int _idCounter = 0; 
        public int Id { get; private set; }
        public EstadoTarefa Estado { get; set; }
        public DateTime IniciadaEm { get; set; }
        public DateTime EncerradaEm { get; set; }
        public IEnumerable<Subtarefa> SubtarefasPendentes { get; set; }
        public IEnumerable<Subtarefa> SubtarefasExecutadas { get; set; }
    }

}
