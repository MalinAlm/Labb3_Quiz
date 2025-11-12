using Labb3_Quiz.Command;
using System.Windows.Threading;
using Labb3_Quiz.Models;
using System.Windows.Media;



namespace Labb3_Quiz.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly DispatcherTimer _timer;
        private int _remainingSeconds;
        private int _currentQuestionIndex;

        private Question? _activeQuestion;
        public Question? ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
            }
        }

        private bool _canAnswer = true;
        public bool CanAnswer
        {
            get => _canAnswer;
            set
            {
                _canAnswer = value;
                RaisePropertyChanged();
                AnswerCommand.RaiseCanExecuteChanged();
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

        private string _selectedAnswer;
        public string SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                RaisePropertyChanged();
            }
        }

        private bool _quizFinished;
        public bool QuizFinished
        {
            get => _quizFinished;
            set
            {
                _quizFinished = value;
                RaisePropertyChanged();
            }
        }

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
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

        private string _feedbackText;
        public string FeedbackText
        {
            get => _feedbackText;
            set
            {
                _feedbackText = value;
                RaisePropertyChanged();
            }
        }

        private Brush _feedbackColor = Brushes.Black;
        public Brush FeedbackColor
        {
            get => _feedbackColor;
            set
            {
                _feedbackColor = value;
                RaisePropertyChanged(); 
            }
        }

        public string ResultText => $"You got {Score} out of {ActivePack?.Questions.Count ?? 0} Correct!";

        public DelegateCommand AnswerCommand { get; }
        public DelegateCommand RestartCommand { get; }
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

            AnswerCommand = new DelegateCommand(SelectAnswer, _=> CanAnswer);
            RestartCommand = new DelegateCommand(_ => RestartQuiz());
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

        private async void SelectAnswer(object? selected)
        {
            if (ActiveQuestion == null || selected is not string answerText) return;

            CanAnswer = false;
            SelectedAnswer = answerText;
            _timer.Stop();

            bool isCorrect = answerText == ActiveQuestion.CorrectAnswer;
            if (isCorrect) Score++;

            FeedbackText = isCorrect ? "Correct answer!" : "Incorrect Answer!";
            FeedbackColor = isCorrect ? Brushes.Green : Brushes.Red;


            await Task.Delay(2000);

            SelectedAnswer = null;
            FeedbackText = string.Empty;
            LoadNextQuestion();
            CanAnswer = true;
        }

        public void LoadNextQuestion()
        {
            if (ActivePack == null || !ActivePack.Questions.Any()) return;

            if (_currentQuestionIndex >= ActivePack.Questions.Count)
            {
                _timer.Stop();
                TimerText = "Quiz Complete!";
                ActiveQuestion = null;
                QuizFinished = true;
                RaisePropertyChanged(nameof(ResultText));
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

        public void RestartQuiz()
        {
            QuizFinished = false;
            Score = 0;
            _currentQuestionIndex = 0;
            StartQuiz();
        }
    }
}
