using Refit;
using System;
using System.Threading.Tasks;

namespace PluralVideos.Download.Services
{
    public class BaseService
    {
        protected async Task<ServiceResponse<T>> Process<T>(Func<Task<T>> fun) where T : class, new()
        {
            var response = new ServiceResponse<T>();
            try
            {
                response.Data = await fun();
            }
            catch (ValidationApiException ex)
            {
                response.Error = await ex.GetContentAsAsync<object>();
            }
            catch (ApiException ex)
            {
                response.Error = await ex.GetContentAsAsync<object>();
            }

            return response;
        }
    }
}
