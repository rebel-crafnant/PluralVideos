using System;
using System.Threading;
using System.Threading.Tasks;
using PluralVideos.Download;
using PluralVideos.Helpers;
using PluralVideos.Options;

namespace PluralVideos
{
    public class Authenticator
    {
        private readonly AuthenticatorOptions options;
        private readonly AuthenticationManager manager = new();

        public Authenticator(AuthenticatorOptions options)
        {
            this.options = options;
        }

        public async Task RunAsync()
        {
            if (options.Login)
                await LoginAsync(options.Application);

            if (options.Logout)
            {
                var response = await manager.Logout();
                if (response.Success)
                    Utils.WriteGreenText("Logged out successfully");
                else
                    Utils.WriteRedText($"Could not log out . Error: {response.Error.Message}");
            }
        }

        private async Task LoginAsync(bool local)
        {
            if (await manager.IsLoggedIn())
            {
                Utils.WriteGreenText("You are already logged in.");
                return;
            }

            if (local)
            {
                if (await manager.LocalAuthenticate())
                    Utils.WriteGreenText("You have successfully logged in");
                else
                    Utils.WriteRedText("Error getting credentials. Check that you are logged in the Offline Pluralsight App");
                return;
            }

            var authResponse = await manager.Authenticate();
            if (!authResponse.Success)
            {
                Utils.WriteRedText($"Could not register the device. Error: {authResponse.Error.Message}");
                return;
            }

            var register = authResponse.Data;
            Utils.WriteYellowText($"Visit {register.AuthDeviceUrl}");
            Utils.WriteYellowText($"Enter Pin {register.Pin}");
            Utils.WriteYellowText($"Expires at: {register.ValidUntil:HH:mm} UTC");

            while (true)
            {
                Thread.Sleep(15000);

                if (register.ValidUntil <= DateTimeOffset.UtcNow)
                {
                    Utils.WriteRedText("Pin is no longer valid.");
                    break;
                }

                var statusResponse = await manager.DeviceStatus();
                if (statusResponse.Success && statusResponse.Data.Status == "Valid")
                {
                    var authorizeResponse = await manager.Authorize();
                    if (!authorizeResponse.Success)
                        Utils.WriteRedText($"Could not get access token. Error: {authorizeResponse.Error.Message}");
                    else
                        Utils.WriteGreenText("You have successfully logged in");

                    break;
                }
                else if (!statusResponse.Success)
                {
                    Utils.WriteRedText($"Could not get device status. Error: {statusResponse.Error.Message}");
                }
            }
        }
    }
}
