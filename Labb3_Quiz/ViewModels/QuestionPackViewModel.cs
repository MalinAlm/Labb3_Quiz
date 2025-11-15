
using Labb3_Quiz.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Labb3_Quiz.ViewModels
{

    public class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPack _model;
        private readonly Action _saveAction;

        public string Name
        {
            get => _model.Name;
            set
            {
                _model.Name = value;
                RaisePropertyChanged();
                _saveAction();
            }
        }

        public Difficulty Difficulty
        {
            get => _model.Difficulty;
            set
            {
                _model.Difficulty = value;
                RaisePropertyChanged();
                _saveAction();
            }
        }

        public int TimeLimitInSeconds
        {
            get => _model.TimeLimitInSeconds;
            set
            {
                _model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
                _saveAction();
            }
        }

        public ObservableCollection<QuestionViewModel> Questions { get; }
        public QuestionPackViewModel(QuestionPack model, Action saveAction)
        {
            _model = model;
            _saveAction = saveAction;

            Questions = new ObservableCollection<QuestionViewModel>(
            _model.Questions.Select(q => new QuestionViewModel(q, saveAction)));

            Questions.CollectionChanged += Questions_CollectionChanged;
        }


        private void Questions_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (QuestionViewModel questionVm in e.NewItems) _model.Questions.Add(questionVm.Model);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (QuestionViewModel questionVm in e.OldItems) _model.Questions.Remove(questionVm.Model);
            }

            _saveAction();
        }

        public void SyncToModel()
        {
            _model.Questions = Questions.Select(qvm => qvm.Model).ToList();
        }
        public QuestionPack Model => _model;

    }

}