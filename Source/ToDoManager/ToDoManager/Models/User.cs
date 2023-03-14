//need this model?

namespace ToDoManager.Models
{
    public class User 
    {
        string _email;
        public string Email
        {
            get => _email;

            set => _email = value;

        }

        string _password;
        public string Password
        {
            get => _password;

            set => _password = value;
        }


    }
}
