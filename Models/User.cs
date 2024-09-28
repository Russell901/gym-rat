using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_rat.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }        
        public string PasswordHash { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool IsLocked { get; set; }
    }
}
