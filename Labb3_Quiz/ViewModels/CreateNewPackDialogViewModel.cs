using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3_Quiz.Command;
using Labb3_Quiz.Models;


namespace Labb3_Quiz.ViewModels
{
    class CreateNewPackDialogViewModel :ViewModelBase
    {

        private string _packName = "New Pack";
        public string PackName
        {
            get => _packName;
            set
            {
                if (_packName != value)
                {
                    _packName = value;
                    RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<Difficulty> Difficulties { get; } = new(Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>());
        public Difficulty SelectedDifficulty { get; set; } = Difficulty.Medium;

        private int _timeLimit = 30;
        public int TimeLimit 
        { 
            get => _timeLimit;
            set
            {
                if (_timeLimit != value)
                {
                    _timeLimit = value;
                    RaisePropertyChanged();
                }
            }
        
        }

        public DelegateCommand ConfirmCommand  { get; }

        public event Action? RequestClose;

        public CreateNewPackDialogViewModel()
        {
            ConfirmCommand = new DelegateCommand(_ => Confirm());
        }

        private void Confirm()
        {
            RequestClose?.Invoke();
        }

    }
}
