using System;
using System.Threading;
using System.Threading.Tasks;
using ConsoleUI;
using ProcessadorTarefas.Development_Stuff;
using ProcessadorTarefas.Entity.Servicos;
using ProcessadorTarefas.Servicos;
using SOLID_Example.Interfaces;

internal class Program
{
    private static readonly object _consoleLock = new object();
    
    public static async Task Main(string[] args)
    {
        try
        {
            var gerenciadorTarefas = new GerenciadorTarefas(new MemoryRepository());
            gerenciadorTarefas.Criar();

            var taskTable = new TableTarefas(gerenciadorTarefas);
            taskTable.RenderTable();

            var processadorTarefas = ProcessadorDeTarefas.Criar(gerenciadorTarefas, gerenciadorTarefas.Repositorio);

            Console.WriteLine("Welcome to the Console!");

            var menu = new GerenciadorMenu(gerenciadorTarefas);

            // Refresh the table periodically
            var refreshTimer = new Timer(async (_) =>
            {
                lock (_consoleLock)
                {
                    Console.Clear();
                    taskTable.RenderTable();
                    Task.Delay(1000); // Wait for 1 second
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            await menu.ShowMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}