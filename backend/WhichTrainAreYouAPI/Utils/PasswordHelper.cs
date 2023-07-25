using BCrypt.Net;
namespace WhichTrainAreYouAPI.Utils
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, out string salt)
        {
            salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool VerifyPassword(string password, string hash, string salt)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hash == hashedPassword;
        }
    }
}
