using System.Collections;
using ProcessadorTarefas.Entidades;

namespace SOLID_Example.Interfaces
{
    public class MemoryRepository : IRepository<Tarefa>
    {
        public List<Tarefa> _tarefas { get; set; } // Make the list private

        public MemoryRepository()
        {
            _tarefas = new List<Tarefa>(); // Initialize the list
        }

        public IEnumerable<Tarefa> GetAll() => _tarefas;

        public void Add(Tarefa entity)
        {
            _tarefas.Add(entity); // Add the entity to the list
        }

        public Tarefa? GetById(int id) => _tarefas.FirstOrDefault(t => t.Id == id);

        public async Task Update(Tarefa entity) // Add 'async' to the method signature
        {
            int index = _tarefas.FindIndex(t => t.Id == entity.Id);

            if (index != -1)
                _tarefas[index] = entity;
            else
                throw new ArgumentException("Tarefa não encontrada.");
        }

        public async Task Update(Tarefa entity, Func<Tarefa, Tarefa> Modificacao)
        {
            Tarefa modifiedEntity = Modificacao(entity);
            await Update(modifiedEntity);
        }

    }

}
