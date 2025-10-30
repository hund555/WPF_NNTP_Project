using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_NNTP_Project.Models;

namespace WPF_NNTP_Project.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Profile> profiles;

        public ObservableCollection<Profile> Profiles
        {
            get { return profiles; }
            set { profiles = value; this.Changed(); }
        }

        NNTPConnection _nntpConnection;

        public MainViewModel()
        {
            profiles = new ObservableCollection<Profile>();
            LoadProfiles();
        }

        public async Task<string> ConnectToNNTPAsync(string server, int port, Profile profile)
        {
            _nntpConnection = NNTPConnection.Instance;
            string? response = await _nntpConnection.ConnectAsync(server, port);
            

            if (response == null || !response.Contains("200"))
            {
                MessageBox.Show("Failed to connect to NNTP server.");
                throw new Exception("Connection failed");
            }
            MessageBox.Show(response);



            response = await _nntpConnection.SendCommandAsync($"authinfo user {profile.Email}");
            if (response == null || !response.Contains("381"))
            {
                MessageBox.Show("Username not accepted.");
                throw new Exception("Username not accepted");
            }
            MessageBox.Show(response);



            response = await _nntpConnection.SendCommandAsync($"authinfo pass {profile.Password}");
            if (response == null || !response.Contains("281"))
            {
                MessageBox.Show("Wrong password.\nAuthentication failed.");
                throw new Exception("Authentication failed");
            }
            MessageBox.Show(response);
            return response;
        }

        private void LoadProfiles()
        {
            profiles.Add(new Profile("Name=Select Profile"));
            foreach (string profileLine in File.ReadAllLines("../../../Files/profiles.txt"))
            {
                profiles.Add(new Profile(profileLine));
            }
        }
    }
}
