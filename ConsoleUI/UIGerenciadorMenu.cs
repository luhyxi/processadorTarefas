using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Entity.Servicos;
using Spectre.Console;

namespace ConsoleUI
{
    public class UIGerenciadorMenu
    {
        private readonly GerenciadorTarefas _gerenciadorTarefas;
        public UIGerenciadorMenu(GerenciadorTarefas gerenciadorTarefas)
        {
            _gerenciadorTarefas = gerenciadorTarefas;
        }

        public async Task ShowMenu()
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an operation to perform:")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Criar tarefa",
                        "Cancelar tarefa Por ID",
                        "Cancelar todas tarefas",
                        "Rodar tarefa por ID",
                        "Rodar todas tarefas",
                        "Listar todas as tarefas ativas",
                        "Listar todas as tarefas inativas",
                        "Sair"
                    }));

            Console.WriteLine($"User input: '{option}'");
            option = option.Trim();
            
            switch (option)
            {
                case "Criar tarefa":
                    await _gerenciadorTarefas.Criar();
                    break;
                case "Cancelar tarefa Por ID":
                    int cancelTaskId = AnsiConsole.Ask<int>("Coloque o ID da Tarefa a ser cancelada:");
                    await _gerenciadorTarefas.Cancelar(cancelTaskId);
                    break;
                case "Listar todas as tarefas ativas":
                    RenderAlternativeTable(await _gerenciadorTarefas.ListarAtivas(_gerenciadorTarefas.Repositorio));
                    break;
                case "Listar todas as tarefas inativas":
                    RenderAlternativeTable(await _gerenciadorTarefas.ListarInativas(_gerenciadorTarefas.Repositorio));
                    break;
                case "Rodar todas tarefas":
                    await _gerenciadorTarefas.RodarTodasSubtarefasAsync();
                    break;
                case "Cancelar todas tarefas":
                    await _gerenciadorTarefas.CancelaTodasTarefas();
                    break;
                case "Rodar tarefa por ID":
                    int runTaskId = AnsiConsole.Ask<int>("Coloque o ID da Task a ser rodada:");
                    await _gerenciadorTarefas.RodarSubtarefasPorId(runTaskId);
                    break;
                case "Sair":
                    Environment.Exit(0);
                    break;
                default:
                    AnsiConsole.WriteLine($"Error: Invalid option selected.");
                    break;
            }

            await ShowMenu(); // Show menu again after operation completion
        }

        private void RenderAlternativeTable(IEnumerable<Tarefa> tasks)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Estado");
            table.AddColumn("Iniciada em");
            table.AddColumn("Encerrada em");
            table.AddColumn("Subtarefas Pendentes");

            foreach (var task in tasks)
            {
                string subtarefasPendentesCount = task.SubtarefasPendentes?.Count().ToString() ?? "0";

                table.AddRow(
                    task.Id.ToString(),
                    task.Estado.ToString(),
                    task.IniciadaEm.ToString(CultureInfo.CurrentCulture) ?? "N/A",
                    task.EncerradaEm?.ToString() ?? "N/A",
                    subtarefasPendentesCount);
            }
            
            AnsiConsole.Render(table);
        }
    }
}
