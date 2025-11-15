using Labb3_Quiz.Command;
using Labb3_Quiz.Models;
using System.Windows.Input;

namespace Labb3_Quiz.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {

        private readonly MainWindowViewModel _mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

        private QuestionViewModel? _activeQuestion;
        public QuestionViewModel? ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();

                RemoveQuestionCommand?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand OpenPackOptionsDialogCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel) 
        {
            this._mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(_ => AddQuestion());
            RemoveQuestionCommand = new DelegateCommand(_ => RemoveQuestion(), _ => CanRemoveQuestion());
            OpenPackOptionsDialogCommand = new DelegateCommand(_ => OpenPackoptionsDialog());

        }
        private void AddQuestion()
        {
            if (ActivePack == null) return;

            var newQuestionModel = new Question("New Question", string.Empty, string.Empty, string.Empty, string.Empty);

            var newQuestionViewModel = new QuestionViewModel(newQuestionModel, _mainWindowViewModel.SaveActivePack);
                
            ActivePack.Questions.Add(newQuestionViewModel);
            ActiveQuestion = newQuestionViewModel;
        }
        
        private void RemoveQuestion()
        {
            if (ActivePack == null || ActiveQuestion == null) return;

            ActivePack.Questions.Remove(ActiveQuestion);
            ActiveQuestion = null;
        }
            
        private bool CanRemoveQuestion() => ActiveQuestion != null;
    
        private void OpenPackoptionsDialog()
        {

            if (ActivePack == null) return;

            var dialog = new Dialogs.PackOptionsDialog();
            var viewModel = new PackOptionsDialogViewModel(ActivePack.Model);
            dialog.DataContext = viewModel;

            dialog.ShowDialog();    

            viewModel.ApplyChanges(ActivePack.Model);

            _mainWindowViewModel.SaveActivePack();

            RaisePropertyChanged(nameof(ActivePack));
        }
    }
}
