using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Entity.Servicos;
using SOLID_Example.Interfaces;

namespace ProcessadorTarefas.Servicos
{
    public class ProcessadorDeTarefas : IProcessadorTarefas
    {
        private readonly GerenciadorTarefas gerenciadorTarefas;
        private readonly ObservableCollection<Tarefa> TarefasObservableCollection;

        private ProcessadorDeTarefas(GerenciadorTarefas gerenciadorTarefas, IRepository<Tarefa> tarefaRepository)
        {
            this.gerenciadorTarefas = gerenciadorTarefas;

            var tarefas = tarefaRepository.GetAll() ?? Enumerable.Empty<Tarefa>();
            TarefasObservableCollection = new ObservableCollection<Tarefa>(tarefas);
            TarefasObservableCollection.CollectionChanged += TarefasObservableCollectionCollectionChanged;
        }

        private void TarefasObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Handle added items
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // Handle removed items
                    break;
                case NotifyCollectionChangedAction.Move:
                    // Handle moved items
                    break;
                case NotifyCollectionChangedAction.Reset:
                    // Handle collection reset
                    break;
            }
        }

        public static ProcessadorDeTarefas Criar(GerenciadorTarefas gerenciadorTarefas, IRepository<Tarefa> tarefaRepository)
        {
            return new ProcessadorDeTarefas(gerenciadorTarefas, tarefaRepository);
        }

        public async Task Iniciar()
        {
            await gerenciadorTarefas.RodarTodasSubtarefasAsync();
            // Implement other necessary logic
        }

        public async Task Encerrar()
        {
            await gerenciadorTarefas.CancelaTodasTarefas();
            // Implement other necessary logic
        }
    }


}
