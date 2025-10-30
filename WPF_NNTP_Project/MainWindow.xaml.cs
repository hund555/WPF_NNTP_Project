using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_NNTP_Project.Models;
using WPF_NNTP_Project.ViewModels;
using WPF_NNTP_Project.Views;

namespace WPF_NNTP_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        const string NntpServer = "news.sunsite.dk";
        const int port = 119;
        public MainWindow()
        {
            DataContext = _viewModel = new MainViewModel();
            InitializeComponent();
            CBProfile.ItemsSource = _viewModel.Profiles;
            CBProfile.SelectedIndex = 0;
            CBProfile.DisplayMemberPath = "Name";
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                string response = await _viewModel.ConnectToNNTPAsync(NntpServer, port, new Profile() { Email = email.Text, Password = password.Password });
                if (response.Contains("281"))
                {
                    CommandView commandView = new CommandView();
                    commandView.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CBProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                Profile selectedProfile = (Profile)comboBox.SelectedItem;
                email.Text = selectedProfile.Email;
                password.Password = selectedProfile.Password;
            }
        }
    }
}