using System.IO;
using System.Text.Json;

namespace CoolFan.Models
{
    public static class credentials
    {
        static credentials()
        {
            var credentialsPath = "credentials.json";
            var json = File.ReadAllText(credentialsPath);
            var data = JsonSerializer.Deserialize<CredentialsData>(json);
            Username = data.Username;
            Password = data.Password;
        }

        public static string Username { get; private set; }
        public static string Password { get; set; }

        private class CredentialsData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
