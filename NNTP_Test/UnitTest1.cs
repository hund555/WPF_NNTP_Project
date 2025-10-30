using WPF_NNTP_Project.Models;
using WPF_NNTP_Project.ViewModels;

namespace NNTP_Test
{
    public class UnitTest1
    {
        [Fact]
        public async void TestConnectToNNTP()
        {
            // Arrange
            NNTPConnection nntpConnection = NNTPConnection.Instance;

            // Act
            string responseConnect = await nntpConnection.ConnectAsync("news.sunsite.dk", 119);
            string responseUser = await nntpConnection.SendCommandAsync("authinfo user allive01@easv365.dk");
            string responsePass = await nntpConnection.SendCommandAsync("authinfo pass 694045");

            // Assert
            Assert.Contains("200", responseConnect);
            Assert.Contains("381", responseUser);
            Assert.Contains("281", responsePass);
        }

        [Fact]
        public void TestLoadProfiles()
        {
            // Arrange
            var viewModel = new MainViewModel();
            // Act
            var profiles = viewModel.Profiles;
            // Assert
            Assert.NotNull(profiles);
            Assert.True(profiles.Count > 0, "Profiles should be loaded and contain at least one profile.");
        }

        [Fact]
        public async void TestGetGroups()
        {
            var mainViewModel = new MainViewModel();
            await mainViewModel.ConnectToNNTPAsync("news.sunsite.dk", 119, new Profile()
            {
                Email = "allive01@easv365.dk",
                Password = "694045"
            });

            var commandViewModel = new CommandViewModel();
            await commandViewModel.GetGroups();

            Assert.True(commandViewModel.Output.Count > 0, "Output should contain groups after GetGroups is called.");
        }

        [Fact]
        public async void TestSelectGroup()
        {
            // arrange
            var mainViewModel = new MainViewModel();
            await mainViewModel.ConnectToNNTPAsync("news.sunsite.dk", 119, new Profile()
            {
                Email = "allive01@easv365.dk",
                Password = "694045"
            });

            var commandViewModel = new CommandViewModel();
            await commandViewModel.GetGroups();

            // act
            await commandViewModel.SelectGroup("dk.test");

            // assert
            Assert.True(commandViewModel.Output.Count > 100, "Output should contain articles after SelectGroup is called.");
        }

        [Fact]
        public async void TestSelectArticle()
        {
            // arrange
            var mainViewModel = new MainViewModel();
            await mainViewModel.ConnectToNNTPAsync("news.sunsite.dk", 119, new Profile()
            {
                Email = "allive01@easv365.dk",
                Password = "694045"
            });

            var commandViewModel = new CommandViewModel();
            await commandViewModel.SelectGroup("dk.test");
            await Task.Delay(500); // Give some time to fetch articles

            // act
            await commandViewModel.SelectArticle("150702");

            // assert
            Assert.True(commandViewModel.Output.Count > 5, "Output should contain data from article after SelectArtical is called.");
        }
    } 
}
