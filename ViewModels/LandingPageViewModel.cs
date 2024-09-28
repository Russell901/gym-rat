using System.Collections.ObjectModel;
using System.Windows.Input;
using gym_rat.Components;
using gym_rat.Helpers;
using gym_rat.Models;
using gym_rat.Services;

namespace gym_rat.ViewModels
{
    public class LandingPageViewModel : BindableObject
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly AuthService _authService;
        public ObservableCollection<FitnessGoal> FitnessGoals { get; set; }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddGoalCommand { get; set; }
        public ICommand GoToDetailsCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public LandingPageViewModel(DatabaseHelper databaseHelper, AuthService authService)
        {
            _databaseHelper = databaseHelper;
            _authService = authService;
            FitnessGoals = new ObservableCollection<FitnessGoal>();
            AddGoalCommand = new Command(async () => await AddGoal());
            GoToDetailsCommand = new Command<FitnessGoal>(async (goal) => await GoToDetails(goal));
            LogoutCommand = new Command(async () => await Logout());
            RefreshCommand = new Command(async () => await LoadUserAndGoals());

            LoadUserAndGoals();
        }

        private async Task LoadUserAndGoals()
        {
            CurrentUser = await _authService.GetCurrentUser();
            if (CurrentUser != null)
            {
                var goals = await _databaseHelper.GetFitnessGoalForUserAsync(CurrentUser.Id);
                FitnessGoals.Clear();
                foreach (var goal in goals)
                {
                    FitnessGoals.Add(goal);
                }
            }
        }

        private async Task AddGoal()
        {
            try
            {
                var addGoalViewModel = new AddGoalViewModel(_databaseHelper, CurrentUser.Id);
                var addGoalModal = new AddGoalModal { BindingContext = addGoalViewModel };
                await Application.Current.MainPage.Navigation.PushModalAsync(addGoalModal);
            }
            catch (Exception ex) 
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task GoToDetails(FitnessGoal goal)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                {"Goal", goal }
            };
            await Shell.Current.GoToAsync($"ActivityDetailsPage?id={goal.Id}");
        }

        private async Task Logout()
        {
            await _authService.LogoutUser();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}