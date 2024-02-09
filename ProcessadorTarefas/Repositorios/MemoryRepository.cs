using System.Collections;
using ProcessadorTarefas.Entidades;

namespace SOLID_Example.Interfaces;

public class MemoryRepository:IRepository<Tarefa>
{
    private List<Tarefa> _tarefas;
    public IEnumerable<Tarefa> GetAll() => _tarefas;
    public void Add(Tarefa entity) => _tarefas.Add(entity);

    public Tarefa? GetById(int id) => _tarefas.FirstOrDefault(t => t.Id == id);

    public void Update(Tarefa entity)
    {
        int index = _tarefas.FindIndex(t => t.Id == entity.Id);

        if (index != -1)
            _tarefas[index] = entity;
        else
            throw new ArgumentException("Tarefa não encontrada.");
    }

    public void Update(Tarefa entity, Func<Tarefa, Tarefa> Modificacao)
    {
        Tarefa modifiedEntity = Modificacao(entity);
        Update(modifiedEntity);
    }
}