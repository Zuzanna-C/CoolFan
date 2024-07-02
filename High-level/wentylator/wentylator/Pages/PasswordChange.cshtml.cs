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
                ErrorMessage = "Aktualne has³o jest nieprawid³owe.";
                return Page();
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Nowe has³a siê nie zgadzaj¹.";
                return Page();
            }

            // Aktualizacja has³a w pliku credentials.cs
            UpdatePasswordInCredentialsFile(NewPassword);

            return RedirectToPage("/Settings");
        }

        private void UpdatePasswordInCredentialsFile(string newPassword)
        {
            // Za³aduj aktualny plik credentials
            var credentialsPath = "credentials.json";
            var credentialsData = new { Username = credentials.Username, Password = newPassword };
            var json = JsonSerializer.Serialize(credentialsData, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(credentialsPath, json);
        }
    }
}

