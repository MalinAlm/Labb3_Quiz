using Labb3_Quiz.Command;
using System.Windows.Threading;
using System.Linq;
using Labb3_Quiz.Models;

namespace Labb3_Quiz.ViewModels
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly DispatcherTimer _timer;
        private int _remainingSeconds;
        private int _currentQuestionIndex;

        private Question? _activeQuestion;
        public Question ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
            }
        }

        private List<string> _answerOptions;
        public List<string> AnswerOptions
        {
            get => _answerOptions;
            set
            {
                _answerOptions = value;
                RaisePropertyChanged(); 
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

        private string _questionProgressText;
        public string QuestionProgressText
        {
            get => _questionProgressText;
            set
            {
                _questionProgressText = value;
                RaisePropertyChanged();
            }
        }

        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel) 
        {
            this._mainWindowViewModel = mainWindowViewModel;

            _remainingSeconds = 30;
            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1.0)
            };
           
            _timer.Tick += Timer_Tick;
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
                LoadNextQuestion();
            }
        }

        public void LoadNextQuestion()
        {
            if (ActivePack == null || !ActivePack.Questions.Any()) return;

            if (_currentQuestionIndex >= ActivePack.Questions.Count)
            {
                _timer.Stop();
                TimerText = "Quiz Complete!";
                ActiveQuestion = null;
                return;
            }

            ActiveQuestion = ActivePack.Questions[_currentQuestionIndex];
            _currentQuestionIndex++;

            var allAnswers = new List<string>(ActiveQuestion.IncorrectAnswers)
            {
                ActiveQuestion.CorrectAnswer
            };

            var Random = new Random();
            AnswerOptions = allAnswers.OrderBy(_ => Random.Next()).ToList();

            _remainingSeconds = ActivePack.TimeLimitInSeconds > 0 ? ActivePack.TimeLimitInSeconds : 30;
            TimerText = _remainingSeconds.ToString();
            _timer.Start();

            QuestionProgressText = $"Question {_currentQuestionIndex} of {ActivePack.Questions.Count}";

        }
        public void StartQuiz()
        {
            _currentQuestionIndex = 0;
            LoadNextQuestion();
        }

        public void StopQuiz()
        {
            _timer.Stop();
            _currentQuestionIndex = 0;
            _remainingSeconds = 0;
            TimerText = string.Empty;
            ActiveQuestion = null;
        }
    }
}
