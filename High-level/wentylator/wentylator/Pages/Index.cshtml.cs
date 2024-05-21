using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wentylator.Pages
{
    public class IndexModel : PageModel
    {
        public float Temperature { get; set; } = 25.3f; // przyk�adowe dane
        public float Humidity { get; set; } = 60.5f; // przyk�adowe dane

        public void OnGet()
        {
            // Logika pobierania rzeczywistych danych z czujnik�w
        }
    }

}
