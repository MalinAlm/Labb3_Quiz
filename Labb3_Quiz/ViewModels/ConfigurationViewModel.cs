using Labb3_Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_Quiz.ViewModels
{
    internal class ConfigurationViewModel : ViewModelBase
    {

        private readonly MainWindowViewModel? _mainWindowViewModel;
        private Question _activeQuestion;

        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        public Question ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
            }
        }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel) 
        {
            this._mainWindowViewModel = mainWindowViewModel;

        }
    }
}
