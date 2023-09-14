using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;

namespace Pyco.Todo.Areas.Registration.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public IndexModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [ModelBinder] public User UserModel { get; set; } = new();
        [BindProperty] public string PasswordRepeat { get; set; } = string.Empty;

        public string FormResult { get; set; } = string.Empty;

        /// <summary>
        /// OnPost Method of UserRegistration page (executed when registration form is submitted by user).
        /// Validate form inputs, check if username/email already exist in database,
        /// password hashing, insert user account into the database
        /// Give user feedback by setting the FormResult property.
        /// </summary>
        /// <param name="userModel">UserAccountModel of the user that is entered in the form and should be registrated.</param>
        public IActionResult OnPost([FromForm] User userModel)
        {
            //check if username already exists
            User? existingUser = _userRepository.Get(userModel.Username);

            if (existingUser != null)
            {
                FormResult =
                    $"Der eingegebene Benutzername \"{userModel.Username}\" existiert bereits. Bitte wählen Sie einen anderen Benutzernamen.";

                return Page();
            }

            //check if Email already exists
            /*existingUser = _userRepository.Get(userModel.Email);

            if (existingUser != null)
            {
                FormResult = "Für die Email \"" + userModel.Email + "\" wurde bereits ein Account angelegt!";

                return Page();
            }
            */
            //check if password in both input fields are the same
            if (!PasswordRepeat.Equals(userModel.Password))
            {
                FormResult = "Die eingegebenen Passwörter stimmen nicht überein!";

                return Page();
            }

            //Hash Password and insert useraccount in db
            PasswordHasher hasher = new();
            userModel.Password = hasher.HashPassword(userModel.Password);
            _userRepository.Insert(userModel);

            FormResult = $"Ihr Account wurde erflogreich angelegt! Herzlich Willkommen bei PYCO, {userModel.Username}!";

            return Page();
        }
    }
}
