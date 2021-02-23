using PluralVideos.Data.Persistence;
using PluralVideos.Download.Clients;
using PluralVideos.Download.Resources;
using System.Threading.Tasks;

namespace PluralVideos.Download
{
    public class AuthenticationManager
    {
        private readonly PluralsightApi api;
        private readonly UnitOfWork<PluralVideosContext> uow = new(new PluralVideosContext());
        private DeviceInfoResource deviceInfo;

        public AuthenticationManager()
        {
            var user = uow.Users.GetUserAsync().Result;
            api = new(15, user);
            deviceInfo = user == null ? null : new(user.DeviceId, user.RefreshToken);
        }

        public async Task<bool> IsLoggedIn() 
            => await uow.Users.GetUserAsync() != null;

        public async Task<bool> LocalAuthenticate()
        {
            var work = new UnitOfWork<PluralsightContext>(new PluralsightContext());
            var user = await work.Users.GetUserAsync();
            if (user == null)
                return false;

            uow.Users.Add(user);
            await uow.CompleteAsync();

            return true;
        }

        public async Task<ApiResponse<RegisterResource>> Authenticate()
        {
            var response = await api.Auth.AutheticateAsync();
            if (response.Success)
                deviceInfo = new(response.Data.DeviceId, response.Data.RefreshToken);

            return response;
        }

        public async Task<ApiResponse<DeviceStatusResource>> DeviceStatus()
            => await api.Auth.GetStatusAsync(deviceInfo.DeviceId);

        public async Task<ApiResponse> Authorize()
        {
            var response = await api.Auth.AuthorizeAsync(deviceInfo);
            if (response.Success)
            {
                var user = response.Data;
                user.DeviceInfo = deviceInfo;
                uow.Users.Add(user.ToUser());
                await uow.CompleteAsync();
            }
            return response;
        }

        public async Task<ApiResponse> Logout()
        {
            if (deviceInfo == null)
            {
                return new ApiResponse
                {
                    Error = new ApiError { Message = "You are not logged in." }
                };
            }
                
            var response = await api.Auth.LogoutAsync(deviceInfo.DeviceId);
            if (response.Success)
            {
                await uow.Users.DeleteUserAsync();
                await uow.CompleteAsync();
            }
            return response;
        }
    }
}
