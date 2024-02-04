namespace ProcessadorTarefas.Entidades
{
    public struct Subtarefa
    {
        public Subtarefa()
        {
            Duracao = TimeSpan.FromSeconds(Random.Shared.Next(3, 60));
        }
        public TimeSpan Duracao { get; set; } 
        public bool DuracaoZerada => Duracao.TotalSeconds <= 0;
    }

}
