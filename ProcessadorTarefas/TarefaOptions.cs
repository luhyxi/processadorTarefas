using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace ProcessadorTarefas.Development_Stuff
{
    public sealed class TarefaOptions
    {
        public TimeSpan SubTarefaMaxDuracao { get; set; }
        public int TarefaMaxSubtarefa { get; set; }
        
        public static TarefaOptions CreateFromConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var tarefaOptions = new TarefaOptions();
            configuration.GetSection("TarefaOptions").Bind(tarefaOptions);

            return tarefaOptions;
        }
    }
}