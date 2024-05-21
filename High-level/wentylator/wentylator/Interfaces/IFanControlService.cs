namespace CoolFan.Interfaces
{
    public interface IFanControlService
    {
        Task SendCommandAsync(string command);
    }
}
