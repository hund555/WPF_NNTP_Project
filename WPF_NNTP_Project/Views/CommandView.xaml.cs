using System;
using System.Collections.Generic;
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
using WPF_NNTP_Project.ViewModels;

namespace WPF_NNTP_Project.Views
{
    /// <summary>
    /// Interaction logic for CommandView.xaml
    /// </summary>
    public partial class CommandView : Window
    {
        private CommandViewModel _viewModel;
        public CommandView()
        {
            DataContext = _viewModel = new CommandViewModel();
            InitializeComponent();
            nntpListView.ItemsSource = _viewModel.Output;
        }

        private async void LoadGroupsAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.GetGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void search(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.SearchOutput(searchBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void SelectGroupAsync(object sender, RoutedEventArgs e)
        {
            await _viewModel.SelectGroup(groupBox.Text);
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void nntpListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nntpListView.SelectedItem == null)
                return;
            groupBox.Text = ((string?)nntpListView.SelectedItem).Split(' ')[0];
        }
    }
}
