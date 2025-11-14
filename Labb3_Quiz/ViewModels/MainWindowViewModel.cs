using Labb3_Quiz.Models;    
using System.Collections.ObjectModel;
using Labb3_Quiz.Command;
using System.Windows;
using Labb3_Quiz.Services;
using System.Threading.Tasks;

namespace Labb3_Quiz.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DataService _dataService;
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
        public DelegateCommand ExitProgramCommand { get; }
        public DelegateCommand SelectPackCommand { get; }

        public MainWindowViewModel()
		{

            _dataService = new DataService();

            LoadPacks();

            PlayerViewModel = new PlayerViewModel(this);
			ConfigurationViewModel = new ConfigurationViewModel(this);
           
            ShowConfigurationViewCommand = new DelegateCommand(_ =>
            {
                IsPlayMode = false;
                PlayerViewModel.StopQuiz();
            });

            ShowPlayerViewCommand = new DelegateCommand(_ => 
            {
                IsPlayMode = true;
                PlayerViewModel.StartQuiz();
            }, _ => ActivePack != null && ActivePack.Questions.Count > 0 );

            SelectPackCommand = new DelegateCommand(selectedPack => 
            { 
                if (selectedPack is QuestionPackViewModel pack)
                {
                    ActivePack = pack;
                }
            });

			OpenCreateNewPackDialogCommand = new DelegateCommand(_ => OpenCreateNewPackDialog());
            ToggleFullScreenCommand = new DelegateCommand(_ => IsFullScreen = !IsFullScreen);
            ExitProgramCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        }

        private void OpenCreateNewPackDialog()
		{
			var dialog = new Dialogs.CreateNewPackDialog();
			dialog.DataContext = new CreateNewPackDialogViewModel();

			if (dialog.ShowDialog() == true)
			{
				var dialogViewModel = (CreateNewPackDialogViewModel)dialog.DataContext;

				var newPack = new QuestionPack(
                    dialogViewModel.Name, 
                    dialogViewModel.Difficulty, 
                    dialogViewModel.TimeLimitInSeconds);

				Packs.Add(new QuestionPackViewModel(newPack));
				ActivePack = Packs.Last();

                ActivePack.SyncToModel();

                var allPacks = Packs.Select(p => p.Model).ToList();
                _dataService.SavePacks(allPacks);
            }
		}

        private void LoadPacks()
        {
            var packs = _dataService.LoadPacks();

            if (packs.Any())
            {
                foreach (var pack in packs)
                {
                    Packs.Add(new QuestionPackViewModel(pack));
                }

                ActivePack = Packs.First();
            }
            else
            {
                var newPack = new QuestionPack("Default Pack");
                Packs.Add(new QuestionPackViewModel(newPack));
                ActivePack = Packs.First();
            }
        }

        public void SaveActivePack()
        {
            if (ActivePack == null) return;

            ActivePack?.SyncToModel();

            var allPacks = Packs.Select(p => p.Model).ToList();
            _dataService.SavePacks(allPacks);   
        }

	}
}
