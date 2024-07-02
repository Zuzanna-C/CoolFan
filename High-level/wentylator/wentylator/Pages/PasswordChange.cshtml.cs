using CoolFan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.IO;

namespace wentylator.Pages
{
    public class PasswordChangeModel : PageModel
    {
        [BindProperty]
        public string CurrentPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (CurrentPassword != credentials.Password)
            {
                ErrorMessage = "Aktualne has�o jest nieprawid�owe.";
                return Page();
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Nowe has�a si� nie zgadzaj�.";
                return Page();
            }

            // Aktualizacja has�a w pliku credentials.cs
            UpdatePasswordInCredentialsFile(NewPassword);

            return RedirectToPage("/Settings");
        }

        private void UpdatePasswordInCredentialsFile(string newPassword)
        {
            // Za�aduj aktualny plik credentials
            var credentialsPath = "credentials.json";
            var credentialsData = new { Username = credentials.Username, Password = newPassword };
            var json = JsonSerializer.Serialize(credentialsData, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(credentialsPath, json);
        }
    }
}

