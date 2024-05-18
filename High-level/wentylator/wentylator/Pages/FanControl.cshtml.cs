using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wentylator.Pages.FanControl
{
    [Authorize]
    public class FanControlIndexModel : PageModel
    {
        public bool IsFanOn { get; set; } = false; // przyk�adowe dane

        public void OnGet()
        {
            // Pobierz aktualny stan wentylatora
        }

        public IActionResult OnPost(string action)
        {
            if (action == "toggle")
            {
                IsFanOn = !IsFanOn;
                // Wy�lij odpowiedni� komend� do Arduino
            }
            return RedirectToPage();
        }
    }
}
