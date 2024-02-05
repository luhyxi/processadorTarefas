namespace ProcessadorTarefas.Entidades
{
    public struct Subtarefa
    {
        public Subtarefa()
        {
            _startTime = DateTime.UtcNow;
            Duracao = TimeSpan.FromSeconds(Random.Shared.Next(3, 60));
        }
        private DateTime _startTime;
        public TimeSpan Duracao { get; set; } 
        public bool DuracaoZerada => Duracao.TotalSeconds <= 0;
        
        // Método async para update do tempo, tal como um timer
        public async Task UpdateDurationAsync()
        {
            TimeSpan elapsed = DateTime.UtcNow - _startTime;
            Duracao = Duracao.Subtract(elapsed);

            // Fazer questão que a duração nunca seja menor que zero
            if (Duracao.TotalSeconds < 0) Duracao = TimeSpan.Zero;

            _startTime = DateTime.UtcNow; // Update do tempo

            // Simula um timer async
            await Task.Delay(1000); // 1000 milliseconds (1 second) delay
        }

    }

}
