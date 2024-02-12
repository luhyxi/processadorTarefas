using ProcessadorTarefas.Entity.Servicos;
using Spectre.Console;

namespace ConsoleUI;

public class TableTarefas
{
    private readonly GerenciadorTarefas _gerenciadorTarefas;

    public TableTarefas(GerenciadorTarefas gerenciadorTarefas)
    {
        _gerenciadorTarefas = gerenciadorTarefas;
    }

    public void RenderTable()
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Estado");
        table.AddColumn("Iniciada em");
        table.AddColumn("Encerrada em");
        table.AddColumn("Subtarefas Pendentes");



        var tarefasAtivas = _gerenciadorTarefas.Repositorio.GetAll();

        foreach (var tarefa in tarefasAtivas)
        {
            string subtarefasPendentesCount = tarefa.SubtarefasPendentes?.Count().ToString() ?? "0";



            table.AddRow(tarefa.Id.ToString(), tarefa.Estado.ToString(), tarefa.IniciadaEm.ToString(), tarefa.EncerradaEm.ToString(), subtarefasPendentesCount);
        }

        AnsiConsole.Render(table);
    }



}