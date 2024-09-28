using gym_rat.Models;
using SQLite;

namespace gym_rat.Helpers
{
    public class DatabaseHelper
    {
        readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<FitnessGoal>().Wait();
            //_database.CreateTableAsync<ActivityModel>().Wait();
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<User> GetUserAsync(string username)
        {
            return _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public Task<int> UpdateUserAsync(User user)
        {
            return _database.UpdateAsync(user);
        }

        //FitnessGoal operations
        public Task<List<FitnessGoal>> GetFitnessGoalForUserAsync(int userId)
        {
            return _database.Table<FitnessGoal>()
                            .Where(g => g.UserId == userId)
                            .ToListAsync();
        }

        public Task<int> SaveFitnessGoalAsync(FitnessGoal goal)
        {          
            return _database.InsertAsync(goal); 
        }

        public Task<FitnessGoal> GetFitnessGoalAsync(int goalId)
        {
            return _database.Table<FitnessGoal>().Where(g => g.Id == goalId).FirstOrDefaultAsync();
        }

        public Task<int> UpdateFitnessGoalAsync(FitnessGoal goal)
        {
            return _database.UpdateAsync(goal);
        }

        public Task<int> DeleteFitnessGoalAsync(FitnessGoal goal)
        {
            return _database.DeleteAsync(goal);
        }

    }
}
