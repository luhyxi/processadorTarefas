using System;
using System.Threading.Tasks;
using ProcessadorTarefas.Entity.Servicos;
using Spectre.Console;

namespace ConsoleUI
{
    public class GerenciadorMenu
    {
        private readonly GerenciadorTarefas _gerenciadorTarefas;

        public GerenciadorMenu(GerenciadorTarefas gerenciadorTarefas)
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
                        "1. Create Task",
                        "2. Get Task by ID",
                        "3. Cancel Task by ID",
                        "4. List Active Tasks",
                        "5. List Inactive Tasks",
                        "6. Run All Subtasks",
                        "7. Cancel All Tasks",
                        "8. Run Subtasks for Task by ID",
                        "9. Exit"
                    }));

            Console.WriteLine($"User input: '{option}'");
            option = option.Trim();
            
            switch (option)
            {
                case "1. Create Task":
                    await _gerenciadorTarefas.Criar();
                    break;
                case "2. Get Task by ID":
                    int taskId = AnsiConsole.Ask<int>("Enter task ID:");
                    var task = await _gerenciadorTarefas.Consultar(taskId);
                    AnsiConsole.WriteLine($"Task Details: {task}");
                    break;
                case "3. Cancel Task by ID":
                    int cancelTaskId = AnsiConsole.Ask<int>("Enter task ID to cancel:");
                    await _gerenciadorTarefas.Cancelar(cancelTaskId);
                    break;
                case "4. List Active Tasks":
                    var activeTasks = await _gerenciadorTarefas.ListarAtivas(_gerenciadorTarefas.Repositorio);
                    foreach (var tarefa in activeTasks)
                    {
                        AnsiConsole.WriteLine($"Active Task: {tarefa}");
                    }
                    break;
                case "5. List Inactive Tasks":
                    var inactiveTasks = await _gerenciadorTarefas.ListarInativas(_gerenciadorTarefas.Repositorio);
                    foreach (var tarefa in inactiveTasks)
                    {
                        AnsiConsole.WriteLine($"Inactive Task: {tarefa}");
                    }
                    break;
                case "6. Run All Subtasks":
                    await _gerenciadorTarefas.RodarTodasSubtarefasAsync();
                    break;
                case "7. Cancel All Tasks":
                    await _gerenciadorTarefas.CancelaTodasTarefas();
                    break;
                case "8. Run Subtasks for Task by ID":
                    int runTaskId = AnsiConsole.Ask<int>("Enter task ID to run subtasks:");
                    await _gerenciadorTarefas.RodarSubtarefasPorId(runTaskId);
                    break;
                case "9. Exit":
                    Environment.Exit(0);
                    break;
                default:
                    AnsiConsole.WriteLine($"Error: Invalid option selected.");
                    break;
            }

            await ShowMenu(); // Show menu again after operation completion
        }
    }
}
