using PluralVideos.Download.Entities;
using PluralVideos.Download.Helpers;
using PluralVideos.Download.Options;
using PluralVideos.Download.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PluralVideos.Download
{
    public class Authenticator
    {
        private readonly AuthenticatorOptions options;

        private readonly PluralsightService services;

        public Authenticator(AuthenticatorOptions options)
        {
            this.options = options;
            services = new PluralsightService();
        }

        public async Task Run()
        {
            if (options.Login && options.Logout)
                throw new Exception("Cannot use both --login and --logout flags");

            if (options.Login)
                await Login();
        }

        public async Task Login()
        {
            var user = FileHelper.ReadUser();
            if (user != null)
            {
                Utils.WriteGreenText("You are already logged in", false);
                if (user.Expiration <= DateTimeOffset.UtcNow.AddDays(1.0))
                {
                    await services.AuthorizeAsync(user);
                    Utils.WriteGreenText("  ... Refreshed.");
                }
                Utils.WriteGreenText(".");
                return;
            }

            var register = await services.AuthenticateAsync();
            if (register == null)
            {
                Utils.WriteRedText("Could not authenticate this device.Please try again.");
                return;
            }

            Utils.WriteYellowText($"Visit {register.AuthDeviceUrl}");
            Utils.WriteYellowText($"Enter Pin {register.Pin}");
            Utils.WriteYellowText($"Expires at: {register.ValidUntil:HH:mm} UTC");

            var info = new DeviceInfo { DeviceId = register.DeviceId, RefreshToken = register.RefreshToken };
            while (true)
            {
                Thread.Sleep(15000);

                if (register.ValidUntil <= DateTimeOffset.UtcNow)
                {
                    Utils.WriteRedText("Pin is no longer valid.");
                    break;
                }

                if (await services.DeviceStatus(info.DeviceId))
                {
                    await services.AuthorizeAsync(new User { DeviceInfo = info });
                    Utils.WriteGreenText("You have successfully logged in");

                    break;
                }


            }
        }
    }
}
