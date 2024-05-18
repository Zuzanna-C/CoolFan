using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using wentylator.ExtraClasses;

namespace wentylator.Pages
{
    public class IndexModel : PageModel
    {
        public string Temperature; // przyk³adowe dane
        public float Humidity { get; set; } = 60.5f; // przyk³adowe dane

        public void OnGet()
        {
            ArduinoIPFinder arduinoIPFinder = new ArduinoIPFinder();
            arduinoIPFinder.SendBroadcast("FIND_ARDUINO");
            IPAddress arduinoIp = arduinoIPFinder.ReceiveResponse(5000);
            arduinoIPFinder.Close();
            Temperature = arduinoIp.ToString();
        }

    }

        public class SensorData
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
    }
}
