using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Labb3_Quiz.Models;

namespace Labb3_Quiz.ViewModels
{
    public class PackOptionsDialogViewModel : ViewModelBase
    {
        private string _packName;
        public string PackName
        {
            get => _packName;
            set 
            { 
                _packName = value; 
                RaisePropertyChanged();
            }

        }

        private Difficulty _selectedDifficulty;
        public Difficulty SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                _selectedDifficulty = value;
                RaisePropertyChanged();
            }
        }

        private int _timeLimit;
        public int TimeLimit
        {
            get => _timeLimit;
            set
            {
                _timeLimit = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Difficulty> Difficulties { get; } =
            new(Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>());

        public PackOptionsDialogViewModel(QuestionPack pack)
        {
            _packName = pack.Name;
            _selectedDifficulty = pack.Difficulty;
            _timeLimit = pack.TimeLimitInSeconds;
        }

        public void ApplyChanges(QuestionPack pack)
        {
            pack.Name = _packName;
            pack.Difficulty = _selectedDifficulty;
            pack.TimeLimitInSeconds = _timeLimit;
        }
    }
}
