using CoolFan.HelpClasses;
using CoolFan.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace wentylator.Pages.FanControl
{
    [Authorize]
    public class FanControlIndexModel : PageModel
    {
        public bool IsFanOn { get; set; } = false;
        public bool IsAutoOn { get; set; } = false;
        private readonly IFanControlService _fanControlService;

        public FanControlIndexModel(IFanControlService fanControlService)
        {
            _fanControlService = fanControlService;
        }

        public string ErrorMessage { get; private set; }
        public string CommandMessage { get; private set; }

        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostTurnFanOnAsync()
        {
            try
            {
                await _fanControlService.SendCommandAsync("on");
                CommandMessage = "Fan turned on.";
                IsFanOn = true;
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
                IsFanOn = false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }

        public string? arduinoIP = "0";
        public async Task<IActionResult> OnPostReturnIP()
        {
            ArduinoDiscoverer discoverer = new ArduinoDiscoverer(4567);

            try
            {
                Console.WriteLine("Discovering Arduino...");
                IPAddress arduinoIp = await discoverer.DiscoverArduinoAsync();

                if (arduinoIp != null)
                {
                    Console.WriteLine($"Arduino discovered at IP address: {arduinoIp}");
                    arduinoIP = arduinoIp.ToString();
                }
                else
                {
                    arduinoIP = "Failed to discover Arduino.";
                }
            }
            catch (Exception ex)
            {
                arduinoIP = ex.Message;
            }
            finally
            {
                discoverer.StopDiscovery();
            }

            return Page();
        }

        private static void OnIpAddressDiscovered(string ipAddress)
        {
            Console.WriteLine($"Discovered Arduino IP Address: {ipAddress}");
        }
    }
}
