using Labb3_Quiz.Command;
using Labb3_Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                RemoveQuestionCommand?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel) 
        {
            this._mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(_ => AddQuestion());
            RemoveQuestionCommand = new DelegateCommand(_ => RemoveQuestion(), _ => CanRemoveQuestion());

        }
        private void AddQuestion()
        {
            if (ActivePack == null) return;

            var newQuestion = new Question("New Question", "", "", "", "");
                
            ActivePack.Questions.Add(newQuestion);
            ActiveQuestion = newQuestion;

        }

        private void RemoveQuestion()
        {
            if (ActivePack == null && ActiveQuestion == null) return;

            //var index = ActivePack.Questions.IndexOf(ActiveQuestion);
            ActivePack.Questions.Remove(ActiveQuestion);

            ActiveQuestion = null;
        }

        private bool CanRemoveQuestion()
        {
            return ActiveQuestion != null;
        }
    }
}
