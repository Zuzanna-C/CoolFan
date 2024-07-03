using CoolFan.HelpClasses;
using CoolFan.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace wentylator.Pages.FanControl
{
    [Authorize]
    public class FanControlModel : PageModel
    {
        public bool IsFanOn { get; set; } = false;
        public bool IsAutoOn { get; set; } = false;
        private readonly IFanControlService _fanControlService;



        public FanControlModel(IFanControlService fanControlService)
        {
            _fanControlService = fanControlService;
        }

        public string ErrorMessage { get; private set; }
        public string CommandMessage { get; private set; }

        public async Task OnGetAsync()
        {
            //OnPostTurnFanOffAsync();
        }

        public IActionResult OnPostTurnFanOn()
        {
            try
            {
                _fanControlService.turnON();
                CommandMessage = "Fan turned on.";
                IsFanOn = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }

        public  IActionResult OnPostTurnFanOff()
        {
            try
            {
                _fanControlService.turnOFF();
                CommandMessage = "Fan turned off.";
                IsFanOn = false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }
        
    }
}
