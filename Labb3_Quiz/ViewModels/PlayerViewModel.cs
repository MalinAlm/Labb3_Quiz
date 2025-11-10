using Labb3_Quiz.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            SetPackNameCommand = new DelegateCommand(SetPackName, CanSetPackName);
            DemoText = string.Empty;


            _remainingSeconds = 30;
            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1.0)
            };
            //var timer = new DispatcherTimer(); //spara inte i variabel, spara i field. 
            //timer.Interval = TimeSpan.FromSeconds(1.0); //hur ofta den ska köras, 1.0 är en sekunden.
            _timer.Tick += Timer_Tick;
            _timer.Start();
            //Lägg till stoppmetod här
            //Spara i field och räkna sekunder.
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            //ActivePack.Name += "+";
            if (_remainingSeconds > 0)
            {
                _remainingSeconds--;
                TimerText = _remainingSeconds.ToString();
            }
            else
            {
                _timer.Stop();

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

        private string _demoText;

        public string DemoText
        {
            get { return _demoText; }
            set 
            { 
                _demoText = value;
                RaisePropertyChanged();
                SetPackNameCommand.RaiseCanExecuteChanged();
            }
        }


        private bool CanSetPackName(object? arg)
        {
            return DemoText.Length > 0;
        }

        private void SetPackName(object? obj)
        {
            ActivePack.Name = DemoText;
        }
    }
}
