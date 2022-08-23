using ApiAuth.Models;

namespace ApiAuth.Repositories
{
    public static class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>
            {
                new() {Id = 1, Username = "guilherme", Password = "1234",  Role = "admin" },
                new() {Id = 1, Username = "ferrari", Password = "4321", Role = "employee" }
            };

            return users.FirstOrDefault(x => string.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase)
            && x.Password.ToLower() == password);

        }
    }
}
