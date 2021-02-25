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
        private readonly AuthManager manager = new();

        public Authenticator(AuthenticatorOptions options)
        {
            this.options = options;
        }

        public async Task RunAsync()
        {
            if ((options.LocalLogin || options.Login) && await manager.IsLoggedIn())
            {
                Utils.WriteGreenText("You are already logged in.");
                return;
            }

            if (options.LocalLogin)
                await LocalLoginAsync();

            if (options.Login)
            {
                if (!string.IsNullOrWhiteSpace(options.Password) || !string.IsNullOrWhiteSpace(options.Username))
                {
                    if (string.IsNullOrWhiteSpace(options.Password))
                    {
                        Utils.WriteRedText("Password cannot be empty.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(options.Username))
                    {
                        Utils.WriteRedText("Username cannot be empty.");
                        return;
                    }

                    await LoginWithPasswordAsync(options.Username, options.Password);
                }
                else
                    await LoginAsync();
            }
                

            if (options.Logout)
                await LogoutAsync();
        }

        private async Task LoginAsync()
        {
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
                    await Authorize();
                    break;
                }
                else if (!statusResponse.Success)
                    Utils.WriteRedText($"Could not get device status. Error: {statusResponse.Error.Message}");
            }
        }

        private async Task LoginWithPasswordAsync(string username, string password)
        {
            var response = await manager.Authenticate(username, password);
            if (!response.Success)
            {
                Utils.WriteRedText($"Could not register the device. Error: {response.Error.Message}");
                return;
            }

            await Authorize();
        }

        private async Task LocalLoginAsync()
        {
            if (await manager.LocalAuthenticate())
                Utils.WriteGreenText("You have successfully logged in");
            else
                Utils.WriteRedText("Error getting credentials. Check that you are logged in the Offline Pluralsight App");
        }

        private async Task LogoutAsync()
        {
            if (await manager.Logout())
                Utils.WriteGreenText("Logged out successfully");
            else
                Utils.WriteRedText($"You are not logged in.");
        }

        private async Task Authorize()
        {
            var authorizeResponse = await manager.Authorize();
            if (!authorizeResponse.Success)
                Utils.WriteRedText($"Could not get access token. Error: {authorizeResponse.Error.Message}");
            else
                Utils.WriteGreenText("You have successfully logged in");
        }
    }
}
