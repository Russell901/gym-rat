using gym_rat.Helpers;
using gym_rat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gym_rat.ViewModels
{
    [QueryProperty(nameof(Goal), "Goal")]
    public class GoalDetailsViewModel : BindableObject
    {
        private readonly DatabaseHelper _databaseHelper;

        private FitnessGoal _goal;
        public FitnessGoal Goal
        {
            get => _goal;
            set
            {
                _goal = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteGoalCommand { get; }

        public GoalDetailsViewModel(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            SaveChangesCommand = new Command(async () => await SaveChanges());
            DeleteGoalCommand = new Command(async () => await DeleteGoal());
        }

        private async Task SaveChanges()
        {
            if (Goal.CalorieTarget <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid calorie target.", "OK");
                return;
            }

            await _databaseHelper.SaveFitnessGoalAsync(Goal);
            await Application.Current.MainPage.DisplayAlert("Success", "Goal updated successfully.", "OK");
        }

        private async Task DeleteGoal()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm Delete", "Are you sure you want to delete this goal?", "Yes", "No");
            if (confirm)
            {
                await _databaseHelper.DeleteFitnessGoalAsync(Goal);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
