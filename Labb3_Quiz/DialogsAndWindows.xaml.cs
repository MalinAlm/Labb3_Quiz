using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Labb3_Quiz
{
    /// <summary>
    /// Interaction logic for DialogsAndWindows.xaml
    /// </summary>
    public partial class DialogsAndWindows : Window
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public DialogsAndWindows()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is a message", "Message to user:");
        }

        private void AddCharacterButton_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult result = MessageBox.Show("Do you want to add a character to the button text?", "Update button text?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                AddCharacterButton.Content += "?";
            }
        }

        private void DisableButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to disable this button?", "Disable button?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                DisableButton.IsEnabled = false;
            }
        }

        private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.ShowHiddenItems = true;
            dialog.FileName = "Malin.txt";
            // dialog.InitialDirectory = @"C:\Windows";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //Komma åt mappen för den användare som kör programmet.
            
            if (dialog.ShowDialog() == true)
            {
                OpenFileDialogButton.Content = dialog.FileName;
            }
        }

        private void SaveFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();

            dialog.ShowHiddenItems = true;
            dialog.FileName = "SaveFileCSharpDemo.txt";
            // dialog.InitialDirectory = @"C:\Windows";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //Komma åt mappen för den användare som kör programmet.
            dialog.OverwritePrompt = true; // detta ger varningen för att inte råka skriva över en fil, inget man behöver skriva själv då det sker automatiskt
           
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, AddCharacterButton.Content.ToString()); //Skriver över filen, man får dock en varning
            }
        }

        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            var dialog =  new AddUserDialog();

            //dialog.FirstName = "Malin"; //Detta sätter ett defaultvärde på textboxen

            if (dialog.ShowDialog() == true)
            {
                Users.Add(new User() { FirstName = dialog.FirstName, LastName = dialog.LastName});
            }

        }
    }
}
