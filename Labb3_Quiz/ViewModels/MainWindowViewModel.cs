using Labb3_Quiz.Models;    
using System.Collections.ObjectModel;
using Labb3_Quiz.Command;

namespace Labb3_Quiz.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
        public PlayerViewModel PlayerViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }

        public bool IsConfigurationViewVisible => IsEditMode;
        public bool IsPlayerViewVisible => IsPlayMode;


        private bool _isPlayMode;
        public bool IsPlayMode
        {
            get => _isPlayMode;
            set
            {
                _isPlayMode = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEditMode));
                RaisePropertyChanged(nameof(IsConfigurationViewVisible));
                RaisePropertyChanged(nameof(IsPlayerViewVisible));
            }
        }

        public bool IsEditMode => !_isPlayMode;

        public object? CurrentView => IsPlayMode ? PlayerViewModel : ConfigurationViewModel;

        private QuestionPackViewModel _activePack;
        public QuestionPackViewModel ActivePack
		{
			get => _activePack; 
			set {
				_activePack = value;
				RaisePropertyChanged();
				PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
                ConfigurationViewModel?.RaisePropertyChanged(nameof(ConfigurationViewModel.ActivePack));
			}
		}

        private bool _isFullScreen;
        public bool IsFullScreen
        {
            get => _isFullScreen;
            set
            {
                _isFullScreen = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand OpenCreateNewPackDialogCommand { get; }
        public DelegateCommand ShowPlayerViewCommand { get; }
        public DelegateCommand ShowConfigurationViewCommand { get; }
        public DelegateCommand ToggleFullScreenCommand {  get; }

        public MainWindowViewModel()
		{

            var pack = new QuestionPack("MyQuestionPack");
            ActivePack = new QuestionPackViewModel(pack);
            ActivePack.Questions.Add(new Question($"Vad är 1+1", "2", "3", "1", "4"));
            ActivePack.Questions.Add(new Question($"Vad heter Sveriges huvudstad?", "Stockholm", "Oslo", "London", "Berlin"));

            PlayerViewModel = new PlayerViewModel(this);
			ConfigurationViewModel = new ConfigurationViewModel(this);
           
            ShowConfigurationViewCommand = new DelegateCommand(_ => IsPlayMode = false);
            ShowPlayerViewCommand = new DelegateCommand(_ => IsPlayMode = true, _=> ActivePack != null && ActivePack.Questions.Count > 0);
			OpenCreateNewPackDialogCommand = new DelegateCommand(_ => OpenCreateNewPackDialog());
            ToggleFullScreenCommand = new DelegateCommand(_=> IsFullScreen = !IsFullScreen);
           
        }

        private void OpenCreateNewPackDialog()
		{
			var dialog = new Dialogs.CreateNewPackDialog();
			dialog.DataContext = new CreateNewPackDialogViewModel();

			if (dialog.ShowDialog() == true)
			{
				var dialogViewModel = (CreateNewPackDialogViewModel)dialog.DataContext;

				var newPack = new QuestionPack(dialogViewModel.PackName, dialogViewModel.SelectedDifficulty, dialogViewModel.TimeLimit);
				Packs.Add(new  QuestionPackViewModel(newPack));
				ActivePack = Packs.Last();

            }
		}


	}
}
