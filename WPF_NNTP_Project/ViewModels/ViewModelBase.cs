using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_NNTP_Project.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// The PropertyChanged invokation is encapsualted in this reusable method
        /// in an abstract base class
        /// </summary>
        /// <param name="Property"></param>
        protected void Changed([CallerMemberName] string Property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
