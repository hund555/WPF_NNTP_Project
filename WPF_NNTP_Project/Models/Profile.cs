using WPF_NNTP_Project.ViewModels;

namespace WPF_NNTP_Project.Models
{
    public class Profile : ViewModelBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email { get; set; }
        public string Password { get; set; }

        public Profile(string profileValues)
        {
            foreach(string value in profileValues.Split('|'))
            {
                string[] keyValue = value.Split('=');
                switch (keyValue[0])
                {
                    case "Name":
                        Name = keyValue[1];
                        break;
                    case "Email":
                        Email = keyValue[1];
                        break;
                    case "Password":
                        Password = keyValue[1];
                        break;
                }
            }
        }
        public Profile()
        {
            
        }
    }
}
