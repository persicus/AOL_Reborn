using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AOL_Reborn.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;

        public string Username
        {
            get => _username;

            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(); // No need to pass property name manually
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LoginViewModel()
        {
            // Future expansion (e.g., loading stored username)
        }
    
    }
}
