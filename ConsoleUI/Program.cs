using System;
using System.Threading;
using System.Threading.Tasks;
using ConsoleUI;
using ProcessadorTarefas.Development_Stuff;
using ProcessadorTarefas.Entity.Servicos;
using ProcessadorTarefas.Servicos;
using SOLID_Example.Interfaces;
using Spectre.Console;

internal class Program
{
    private static readonly object _consoleLock = new object();
    public static async Task Main(string[] args)
    {
        try
        {
            var gerenciadorTarefas = new GerenciadorTarefas(new MemoryRepository());

            var taskTable = new UITableTarefas(gerenciadorTarefas);
            taskTable.RenderTable();

            ProcessadorDeTarefas processadorDeTarefas = new ProcessadorDeTarefas(gerenciadorTarefas, gerenciadorTarefas.Repositorio);


            var menu = new UIGerenciadorMenu(gerenciadorTarefas);


            
            // Temporizador de refresh do console
            var refreshTimer = new Timer(async (_) =>
            {
                lock (_consoleLock)
                {
                    AnsiConsole.Clear();
                    taskTable.RenderTable();
                    Task.Delay(1000);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            await menu.ShowMenu();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.ToString());
        }
    }

}