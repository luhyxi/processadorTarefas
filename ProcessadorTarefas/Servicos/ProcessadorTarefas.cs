using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Servicos;
using SOLID_Example.Interfaces;

namespace ProcessadorTarefas.Development_Stuff
{
    internal class ProcessadorTarefas : IProcessadorTarefas
    {
        private GerenciadorTarefas gerenciadorTarefas;
        private ObservableCollection<Tarefa> TarefasObservableCollection; 

        private ProcessadorTarefas(GerenciadorTarefas gerenciadorTarefas, IRepository<Tarefa> tarefaRepository)
        {
            this.gerenciadorTarefas = gerenciadorTarefas;
        
            // Convert IRepository<T> to IEnumerable<T> before passing to ObservableCollection<T> constructor
            TarefasObservableCollection = new ObservableCollection<Tarefa>(tarefaRepository.GetAll()); 
            TarefasObservableCollection.CollectionChanged += TarefasObservableCollectionCollectionChanged;
        }

        private void TarefasObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        public static ProcessadorTarefas Criar(GerenciadorTarefas gerenciadorTarefas, IRepository<Tarefa> tarefaRepository)
        {
            return new ProcessadorTarefas(gerenciadorTarefas, tarefaRepository);
        }

        public async Task Iniciar()
        {
            // TODO: implement logic
        }

        public async Task Encerrar()
        {
            // TODO: implement logic
        }
    }
}
