using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_rat.Models
{
    public class FitnessGoal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ActivityName { get; set; }
        public int CalorieTarget { get; set; }
        public int CaloriesBurned { get; set; }
        public string CreatedAt { get; set; }
        public string Description
        {
            get; set;
        }
    }
}
