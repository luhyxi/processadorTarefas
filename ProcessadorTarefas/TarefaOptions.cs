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
    }

    public interface ITarefaOptionsProvider
    {
        TarefaOptions GetTarefaOptions();
    }

    public class TarefaOptionsProvider : ITarefaOptionsProvider
    {
        private readonly IConfiguration _configuration;

        public TarefaOptionsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TarefaOptions GetTarefaOptions()
        {
            var tarefaOptions = new TarefaOptions();
            _configuration.GetSection("TarefaOptions").Bind(tarefaOptions);
            return tarefaOptions;
        }
    }
}