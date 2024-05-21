using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wentylator.Models;

namespace wentylator.Pages
{
    public class SensorDataModel : PageModel
    {
        private readonly ISensorDataFetcher _sensorDataFetcher;
        private readonly IFanControlService _fanControlService;

        public float Temperature;
        public float Humidity;

        public SensorDataModel(ISensorDataFetcher sensorDataFetcher, IFanControlService fanControlService)
        {
            _sensorDataFetcher = sensorDataFetcher;
            _fanControlService = fanControlService;
        }

        public SensorData SensorData { get; private set; }
        public string ErrorMessage { get; private set; }
        public string CommandMessage { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostFetchDataAsync()
        {
            try
            {
                SensorData = await _sensorDataFetcher.FetchSensorDataAsync();
                CommandMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostTurnFanOnAsync()
        {
            try
            {
                await _fanControlService.SendCommandAsync("on");
                CommandMessage = "Fan turned on.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostTurnFanOffAsync()
        {
            try
            {
                await _fanControlService.SendCommandAsync("off");
                CommandMessage = "Fan turned off.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}
