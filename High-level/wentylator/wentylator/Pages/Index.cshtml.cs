using CoolFan.HelpClasses;
using CoolFan.Interfaces;
using CoolFan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace CoolFan.Pages
{
    public class IndexModel : PageModel
    {
        private  ISensorDataFetcher _sensorDataFetcher;
        private  IFanControlService _fanControlService;

        public float Temperature;
        public float Humidity;

        public string ArduinoIP;
        public async Task InitializeAsync()
        {
            ArduinoDiscoverer discoverer = new ArduinoDiscoverer(4567);

            try
            {
                Console.WriteLine("Discovering Arduino...");
                IPAddress arduinoIp = await discoverer.DiscoverArduinoAsync();

                if (arduinoIp != null)
                {
                    ArduinoIP = arduinoIp.ToString();
                }
                else
                {
                    ArduinoIP = "Failed to discover Arduino.";
                }
            }
            catch (Exception ex)
            {
                ArduinoIP = ex.Message;
            }
            finally
            {
                discoverer.StopDiscovery();
            }

            _sensorDataFetcher = new SensorDataFetcher(ArduinoIP);
        }

        // Konstruktor
        public IndexModel(ISensorDataFetcher sensorDataFetcher, IFanControlService fanControlService)
        {
            _fanControlService = fanControlService;
            InitializeAsync().Wait();
        }

        public SensorData SensorData { get; private set; }
        public string ErrorMessage { get; private set; }
        public string CommandMessage { get; private set; }

        public async Task OnGetAsync()
        {
            await OnPostFetchDataAsync();
        }

        public async Task<IActionResult> OnPostFetchDataAsync()
        {
            try
            {
                SensorData = await _sensorDataFetcher.FetchSensorDataAsync();
                Temperature = SensorData.Temperature;
                Humidity = SensorData.Humidity;
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

        public async Task<JsonResult> OnGetSensorDataAsync()
        {
            try
            {
                SensorData = await _sensorDataFetcher.FetchSensorDataAsync();
                return new JsonResult(SensorData);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
