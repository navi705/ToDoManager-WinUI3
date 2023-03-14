using System.ComponentModel.DataAnnotations;
using Windows.ApplicationModel.Resources;
namespace ToDoManager.HelpClasses.Verify
{
    public static class Verfy
    {
        public static ResourceLoader textStatus = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse();
        // to get rid of "if" or change something in logic to get status for register page (maybe hanler in server bacause get around for api)
        public static string FieldsIsValid(string email, string password, string confirmPassword)
        {
            if (!IsEmail(email))
                return textStatus.GetString("EmailIsNotValid");

            
            if (!IsPasswordValid(password))
                return textStatus.GetString("PasswordIsNotValid");

            if (!IsPasswordsMatch(password, confirmPassword))
                return textStatus.GetString("PasswordsIsNotMatch");

            return "";
        }
        
        public static bool IsEmail(string email)
        {
            if (new EmailAddressAttribute().IsValid(email))
                return true;
            return false;
        }

        public static bool IsPasswordsMatch(string password, string confirmPassword)
        {
            if (password == confirmPassword)
                return true;
            return false;
        }

        public static bool IsPasswordValid(string password)
        {
            if (password.Length >= 6)
                return true;
            return false;
        }

        public static string FieldsIsValidSignIn(string email, string password)
        {
            if (!IsEmail(email))
                return textStatus.GetString("EmailIsNotValid");


            if (!IsPasswordValid(password))
                return textStatus.GetString("PasswordIsNotValid");

            return "";
        }
    }
}
