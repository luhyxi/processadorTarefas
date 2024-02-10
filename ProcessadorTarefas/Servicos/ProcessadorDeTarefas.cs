using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Entity.Servicos;
using SOLID_Example.Interfaces;

namespace ProcessadorTarefas.Servicos
{
    public class ProcessadorDeTarefas : IProcessadorTarefas
    {
        private readonly GerenciadorTarefas gerenciadorTarefas;
        private readonly IRepository<Tarefa> tarefaRepository;

        private CancellationTokenSource cancellationTokenSource;
        private bool isProcessing;

        public ProcessadorDeTarefas(GerenciadorTarefas gerenciadorTarefas, IRepository<Tarefa> tarefaRepository)
        {
            this.gerenciadorTarefas = gerenciadorTarefas;
            this.tarefaRepository = tarefaRepository;
        }

        private async Task ProcessarSubtarefasDeTarefa(Tarefa tarefa, CancellationToken cancellationToken)
        {
            await tarefa.RunSubtarefas();
        }

        public async Task Iniciar()
        {
            if (isProcessing)
                return;

            cancellationTokenSource = new CancellationTokenSource();
            isProcessing = true;

            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var tarefas = tarefaRepository.GetAll() ?? Enumerable.Empty<Tarefa>();

                    var processingTasks = tarefas.Select(tarefa => ProcessarSubtarefasDeTarefa(tarefa, cancellationTokenSource.Token));
                    await Task.WhenAll(processingTasks);

                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                isProcessing = false;
            }
        }

        public async Task Encerrar()
        {
            cancellationTokenSource?.Cancel();
            await Task.Delay(1000);
        }
    }
}
