using gym_rat.Helpers;
using gym_rat.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace gym_rat.Services
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class AuthService
    {
        private readonly DatabaseHelper _dbHelper;
        private User _currentUser;

        public AuthService(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<AuthResult> RegisterUser(string username, string password)
        {
            if (!IsValidUsername(username))
                return new AuthResult { Success = false, Message = "Invalid username. Use only letters and numbers." };

            if (!IsValidPassword(password))
                return new AuthResult { Success = false, Message = "Invalid password. It should be at least 12 characters long with one uppercase and one lowercase letter." };

            var existingUser = await _dbHelper.GetUserAsync(username);
            if (existingUser != null)
                return new AuthResult { Success = false, Message = "Username already exists." };

            var newUser = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                FailedLoginAttempts = 0,
                IsLocked = false
            };

            await _dbHelper.SaveUserAsync(newUser);
            return new AuthResult { Success = true, Message = "Registration successful." };
        }


        public async Task<AuthResult> LoginUser(string username, string password)
        {
            var user = await _dbHelper.GetUserAsync(username);
            if (user == null)
                return new AuthResult { Success = false, Message = "User not found." };
            if (user.IsLocked)
                return new AuthResult { Success = false, Message = "Account is locked. Please contact support." };
            if (VerifyPassword(password, user.PasswordHash))
            {
                user.FailedLoginAttempts = 0;
                await _dbHelper.UpdateUserAsync(user);
                _currentUser = user;  // Set the current user
                return new AuthResult { Success = true, Message = "Login successful." };
            }

            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 3)
            {
                user.IsLocked = true;
                await _dbHelper.UpdateUserAsync(user);
                return new AuthResult { Success = false, Message = "Account locked due to too many failed attempts." };
            }

            await _dbHelper.UpdateUserAsync(user);
            return new AuthResult { Success = false, Message = "Incorrect password." };
        }

        public Task<User> GetCurrentUser()
        {
            return Task.FromResult(_currentUser);
        }

        private bool IsValidUsername(string username)
        {
            return Regex.IsMatch(username, @"^[a-zA-Z0-9]+$");
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 12 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return HashPassword(inputPassword) == storedHash;
        }        

        public async Task LogoutUser()
        {
            _currentUser = null;
        }
    }


}
