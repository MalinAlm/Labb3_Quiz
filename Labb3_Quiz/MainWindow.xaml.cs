using Labb3_Quiz.ViewModels;
using System.Windows;


namespace Labb3_Quiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public MainWindow()
        {
           
            InitializeComponent();

            mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;

            mainWindowViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(mainWindowViewModel.IsFullScreen))
                {
                    ToggleFullScreen(mainWindowViewModel.IsFullScreen);
                }
            };
        }

        private void ToggleFullScreen(bool isFullscreen)
        {
            if (isFullscreen)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
        }

    }
}