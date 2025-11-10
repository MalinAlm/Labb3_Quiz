using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Labb3_Quiz.Dialogs
{
    public partial class CreateNewPackDialog : Window
    {

        //public ObservableCollection<NewPack> NewPack { get; set; } = new ObservableCollection<NewPack>();

        public CreateNewPackDialog()
        {
            InitializeComponent();

            Loaded += CreateNewPackDialog_Loaded;

            //DataContext = this;
        }

            private void CreateNewPackDialog_Loaded(object sender, RoutedEventArgs e)
            {
                if (DataContext is ViewModels.CreateNewPackDialogViewModel viewModel)
                {
                    viewModel.RequestClose += () =>
                    {
                        DialogResult = true;
                        Close();
                    };
                }
            }


    }
}