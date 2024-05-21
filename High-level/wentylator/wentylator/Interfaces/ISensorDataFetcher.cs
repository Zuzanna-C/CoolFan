using CoolFan.Models;

namespace CoolFan.Interfaces
{
    public interface ISensorDataFetcher
    {
        Task<SensorData> FetchSensorDataAsync();
    }
}
