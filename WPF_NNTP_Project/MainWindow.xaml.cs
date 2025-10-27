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

namespace WPF_NNTP_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string NntpServer = "news.sunsite.dk";
        const int port = 119;
        List<Profile> profiles = new List<Profile>();
        public MainWindow()
        {
            InitializeComponent();
            profiles.Add(new Profile("Name=Select Profile"));
            foreach (string profileLine in File.ReadAllLines("../../../Files/profiles.txt"))
            {
                profiles.Add(new Profile(profileLine));
            }
            CBProfile.ItemsSource = profiles;
            CBProfile.SelectedIndex = 0;
            CBProfile.DisplayMemberPath = "Name";
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(NntpServer, port);

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.ASCII);
            StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };

            string? response = await reader.ReadLineAsync();
            if (response == null || !response.Contains("200"))
            {
                MessageBox.Show("Failed to connect to NNTP server.");
                return;
            }
            MessageBox.Show(response);



            await writer.WriteLineAsync($"authinfo user {email.Text}");
            response = await reader.ReadLineAsync();
            if (response == null || !response.Contains("381"))
            {
                MessageBox.Show("Username not accepted.");
                return;
            }
            MessageBox.Show(response);



            await writer.WriteLineAsync($"authinfo pass {password.Password}");
            response = await reader.ReadLineAsync();
            if (response == null || !response.Contains("281"))
            {
                MessageBox.Show("Wrong password.\nAuthentication failed.");
                return;
            }
            MessageBox.Show(response);
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