using Restaurant.Web.Models;

namespace Restaurant.Web.Services.Interfaces
{
    public interface IBaseService : IDisposable
    {
        ResponseDto response { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
