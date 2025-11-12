using Labb3_Quiz.Command;
using System.Windows.Threading;

namespace Labb3_Quiz.ViewModels
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly DispatcherTimer _timer;
        private int _remainingSeconds;

        public DelegateCommand SetPackNameCommand { get; }
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel) 
        {
            this._mainWindowViewModel = mainWindowViewModel;
                
            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1.0)
            };
           
            _timer.Tick += Timer_Tick;
        }

        public void StartQuiz()
        {
            _timer.Stop();
            _remainingSeconds = ActivePack?.TimeLimitInSeconds ?? 30;
            TimerText = _remainingSeconds.ToString();
            _timer.Start();
        }

        public void StopQuiz()
        {
            _timer.Stop();
            TimerText = _remainingSeconds.ToString();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_remainingSeconds > 0)
            {
                _remainingSeconds--;
                TimerText = _remainingSeconds.ToString();
            }
            else
            {
                _timer.Stop();
                TimerText = "Time's up!";
            }
        }

        private string _timerText;
        public string TimerText
        {
            get => _timerText;
            set
            {
                _timerText = value;
                RaisePropertyChanged();
            }
        }
    }
}
