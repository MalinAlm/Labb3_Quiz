using Labb3_Quiz.Command;
using Labb3_Quiz.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Labb3_Quiz.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {

        private readonly MainWindowViewModel? _mainWindowViewModel;
        private Question? _activeQuestion;

        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        public Question? ActiveQuestion
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
        private async Task AddQuestion()
        {
            if (ActivePack == null) return;

            var newQuestion = new Question("New Question", string.Empty, string.Empty, string.Empty, string.Empty);
                
            ActivePack.Questions.Add(newQuestion);
            ActiveQuestion = newQuestion;

            ActivePack.SyncToModel();
            await _mainWindowViewModel.SaveActivePackAsync();
        }
        
        private async void RemoveQuestion()
        {
            if (ActivePack == null || ActiveQuestion == null) return;

            //var index = ActivePack.Questions.IndexOf(ActiveQuestion);
            ActivePack.Questions.Remove(ActiveQuestion);
            ActiveQuestion = null;

            ActivePack.SyncToModel();
            await _mainWindowViewModel.SaveActivePackAsync();
        }

        private bool CanRemoveQuestion()
        {
            return ActiveQuestion != null;
        }

        private void OpenPackoptionsDialog()
        {

            if (ActivePack == null) return;

            var dialog = new Dialogs.PackOptionsDialog();
            var viewModel = new PackOptionsDialogViewModel(ActivePack.Model);
            dialog.DataContext = viewModel;

            dialog.ShowDialog();

            viewModel.ApplyChanges(ActivePack.Model);
            RaisePropertyChanged(nameof(ActivePack));
        }
    }
}
