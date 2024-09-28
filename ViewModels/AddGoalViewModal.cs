using System.Windows.Input;
using gym_rat.Helpers;
using gym_rat.Models;

namespace gym_rat.ViewModels
{
    public class AddGoalViewModel : BindableObject
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly int _userId;

        private string _activityName;
        public string ActivityName
        {
            get => _activityName;
            set
            {
                _activityName = value;
                OnPropertyChanged();
            }
        }

        private int _calorieTarget;
        public int CalorieTarget
        {
            get => _calorieTarget;
            set
            {
                _calorieTarget = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveGoalCommand { get; }
        public ICommand CancelCommand { get; }

        public AddGoalViewModel(DatabaseHelper databaseHelper, int userId)
        {
            _databaseHelper = databaseHelper;
            _userId = userId;
            SaveGoalCommand = new Command(async () => await SaveGoal());
            CancelCommand = new Command(async () => await Cancel());
        }

        private async Task SaveGoal()
        {
            if (string.IsNullOrWhiteSpace(ActivityName) || CalorieTarget <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid activity name and calorie target.", "OK");
                return;
            }

            var newGoal = new FitnessGoal
            {
                UserId = _userId,
                ActivityName = ActivityName,
                CalorieTarget = CalorieTarget,
                CaloriesBurned = 0,
                Description = Description,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await _databaseHelper.SaveFitnessGoalAsync(newGoal);
            await Application.Current.MainPage.Navigation.PopModalAsync();
            MessagingCenter.Send(this, "RefreshGoals");
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}