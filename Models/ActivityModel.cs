using SQLite;

namespace gym_rat.Models
{
    public class ActivityModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int CalorieTarget { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
