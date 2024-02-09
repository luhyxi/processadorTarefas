using ProcessadorTarefas.Entidades;

namespace SOLID_Example.Interfaces
{
    public interface IRepository<T>
    {
        List<Tarefa> _tarefas { get; set; }
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Add(T entity);
        Task Update (T entity);
        Task Update(T entity, Func<T, T> Modificacao);

    }
    
}
