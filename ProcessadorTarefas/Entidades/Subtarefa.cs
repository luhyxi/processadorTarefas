using System;
using System.Timers;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace ProcessadorTarefas.Entidades
{
    public class Subtarefa
    {
        private Timer _timer;

        public int DuracaoSeconds { get; private set; }
        public event EventHandler CountdownFinished;
        public bool IsFinished { get; set; }

        public Subtarefa()
        {
            DuracaoSeconds = new Random().Next(1, 7);
        }

        public void RunCountdown()
        {
            _timer = new Timer(1000); // Timer ticks every second
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            DuracaoSeconds--;

            if (DuracaoSeconds <= 0)
            {
                _timer.Stop();
                OnCountdownFinished(EventArgs.Empty);
            }
        }

        protected virtual void OnCountdownFinished(EventArgs e)
        {
            if (!IsFinished) // Check if the event hasn't been fired already
            {
                IsFinished = true;
                CountdownFinished?.Invoke(this, e);
            }
        }
    }

}