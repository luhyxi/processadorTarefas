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
        table.AddColumn("Subtarefas Executadas");
        table.AddColumn("Subtarefas Concluidas");
        table.AddColumn("Subtarefas Não Concluidas");



        var tarefasAtivas = _gerenciadorTarefas.Repositorio.GetAll();

        foreach (var tarefa in tarefasAtivas)
        {
            string subtarefasPendentesCount = tarefa.SubtarefasPendentes?.Count().ToString() ?? "0";
            string subtarefasExecutadasCount = tarefa.SubtarefasExecutadas?.Count().ToString() ?? "0";
            string subtarefasConcCount = tarefa.SubtarefasConcluidas?.Count().ToString() ?? "0";
            string subtarefasNaoConcCount = tarefa.SubtarefasNaoConcluidas?.Count().ToString() ?? "0";



            table.AddRow(tarefa.Id.ToString(), tarefa.Estado.ToString(), tarefa.IniciadaEm.ToString(), tarefa.EncerradaEm.ToString(), subtarefasPendentesCount, subtarefasExecutadasCount, subtarefasConcCount, subtarefasNaoConcCount);
        }

        AnsiConsole.Render(table);
    }



}