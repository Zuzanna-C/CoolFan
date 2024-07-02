using CoolFan.HelpClasses;
using CoolFan.Interfaces;
using CoolFan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoolFan.Pages
{
    public class IndexModel : PageModel
    {
        private  ISensorDataFetcher _sensorDataFetcher;
        private  IFanControlService _fanControlService;
        //private ConnectArduino _connectArduino;

        public float Temperature;
        public float Humidity;

        public string ArduinoIP;

        public IndexModel(ISensorDataFetcher sensorDataFetcher, IFanControlService fanControlService)
        {
            _fanControlService = fanControlService;
            _sensorDataFetcher = sensorDataFetcher;
        }

        public SensorData SensorData { get; private set; }
        public string ErrorMessage { get; private set; }
        public string CommandMessage { get; private set; }

        public async Task OnGetAsync()
        {
            await OnPostFetchData();
        }

        public async Task<IActionResult> OnPostFetchData()
        {
            try
            {
                SensorData = await _sensorDataFetcher.getSensorDataAsync();
                Temperature = SensorData.Temperature;
                Humidity = SensorData.Humidity;

                return new JsonResult(new
                {
                    temperature = SensorData.Temperature,
                    humidity = SensorData.Humidity
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    error = ex.Message
                });
            }
        }

        public async Task<IActionResult> OnPostTurnFanOnAsync()
        {
            try
            {
                await _fanControlService.turnON();
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
                await _fanControlService.turnOFF();
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
