namespace Common
{
    public class User
    {
        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public string Email { get; private set; }
        public string Password { get; private set; }
    }
}