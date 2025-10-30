using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_NNTP_Project.Models;

namespace WPF_NNTP_Project.ViewModels
{
    public class CommandViewModel : ViewModelBase
    {
        private NNTPConnection _nntpConnection;
        private List<string> groups = new List<string>();
        private ObservableCollection<string> output;
		public ObservableCollection<string> Output
        {
			get { return output; }
			set { output = value; this.Changed(); }
		}

		public CommandViewModel()
		{
            _nntpConnection = NNTPConnection.Instance;

            Output = new ObservableCollection<string>();
        }

        public async Task<string> GetGroups()
        {
            string? response = await _nntpConnection.SendCommandAsync("list");
            if (response == null || !response.Contains("215"))
            {
                throw new Exception("Failed to retrieve groups");
            }
            if (groups.Count > 0)
                groups.Clear();

            output.Add(response);
            while (true)
            {
                string? line = await _nntpConnection.Reader.ReadLineAsync();
                if (line == null || line == ".")
                    break;
                output.Add(line);
                groups.Add(line);
            }
            return response;
        }

        public void ClearOutput()
        {
            Output.Clear();
        }

        public void Disconnect()
        {
            _nntpConnection.Disconnect();
        }

        public void SearchOutput(string searchTerm)
        {
            ClearOutput();
            var filteredGroups = groups.Where(g => g.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            foreach (var group in filteredGroups)
            {
                Output.Add(group);
            }
        }

        public async Task SelectGroup(string groupName)
        {
            string? response = await _nntpConnection.SendCommandAsync($"group {groupName}");
            if (response == null || !response.Contains("211"))
            {
                throw new Exception("Failed to select group");
            }

            
            output.Clear();
            output.Add(response);
            string[] startEnd = response.Split(' ');
            response = await _nntpConnection.SendCommandAsync($"xover {startEnd[2]}-{startEnd[3]}");

            output.Add(response);

            while (true)
            {
                string? line = await _nntpConnection.Reader.ReadLineAsync();
                if (line == null || line == ".")
                    break;
                output.Add(line);
            }

            output.Add(response);

        }
    }
}
