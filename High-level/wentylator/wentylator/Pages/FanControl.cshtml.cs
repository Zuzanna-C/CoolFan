using CoolFan.HelpClasses;
using CoolFan.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    }
}
