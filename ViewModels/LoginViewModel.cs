using gym_rat.Helpers;
using gym_rat.Interfaces;
using gym_rat.Services;
using gym_rat.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace gym_rat.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly DatabaseHelper _databaseHelper;
        private string _username;
        private string _password;
        private string _message;
        private bool _rememberMe;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                if (_rememberMe != value)
                {
                    _rememberMe = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Please enter both username and password.";
                return;
            }

            var result = await _authService.LoginUser(Username, Password);
            Message = result.Message;

            if (result.Success)
            {
                if (RememberMe)
                {
                    await SecureStorage.SetAsync("username", Username);
                    await SecureStorage.SetAsync("password", Password);
                }
                else
                {
                    SecureStorage.Remove("username");
                    SecureStorage.Remove("password");
                }

                // Set the user's name if it's not already set
                var currentUser = await _authService.GetCurrentUser();
                if (string.IsNullOrEmpty(currentUser.Username))
                {
                    currentUser.Username = currentUser.Username; // Or prompt the user to enter their name
                    await _databaseHelper.UpdateUserAsync(currentUser);
                }

                // Navigate to the main page of your app
                await _navigationService.NavigateToAsync("/LandingPage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
