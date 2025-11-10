using Labb3_Quiz.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3_Quiz;
using Labb3_Quiz.Command;

namespace Labb3_Quiz.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();

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

        public PlayerViewModel? PlayerViewModel { get; }
		public ConfigurationViewModel? ConfigurationViewModel { get; }

		private ViewModelBase _currentViewModel;
		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			set
			{
				_currentViewModel = value;
				RaisePropertyChanged();	
			}
		}

		public DelegateCommand ShowPlayerViewCommand { get; }
		public DelegateCommand ShowConfigurationViewCommand {  get; }

		public MainWindowViewModel()
		{

            var pack = new QuestionPack("MyQuestionPack");
            ActivePack = new QuestionPackViewModel(pack);
            ActivePack.Questions.Add(new Question($"Vad är 1+1", "2", "3", "1", "4"));
            ActivePack.Questions.Add(new Question($"Vad heter Sveriges huvudstad?", "Stockholm", "Oslo", "London", "Berlin"));

            ShowPlayerViewCommand = new DelegateCommand(_ => CurrentViewModel = PlayerViewModel);
            ShowConfigurationViewCommand = new DelegateCommand(_ => CurrentViewModel = ConfigurationViewModel);

            PlayerViewModel = new PlayerViewModel(this);
			ConfigurationViewModel = new ConfigurationViewModel(this);

			CurrentViewModel = ConfigurationViewModel;

            

        }


	}
}
