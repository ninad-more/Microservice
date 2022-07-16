using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Services.Interfaces;
using System.Text;

namespace Restaurant.Web.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto response { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.response = new ResponseDto();
            this.httpClient = httpClient;
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("TestAPI");
                HttpRequestMessage msg = new HttpRequestMessage();
                msg.Headers.Add("Accept", "application/json");
                msg.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                if(apiRequest.Data != null)
                {
                    msg.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage apiResponse = null;

                switch(apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.GET:
                        msg.Method = HttpMethod.Get;
                        break;
                    case StaticDetails.ApiType.POST:
                        msg.Method = HttpMethod.Post;
                        break;
                    case StaticDetails.ApiType.PUT:
                        msg.Method = HttpMethod.Put;
                        break;
                    case StaticDetails.ApiType.DELETE:
                        msg.Method = HttpMethod.Delete;
                        break;
                }

                apiResponse = await client.SendAsync(msg);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;
            }
            catch(Exception ex)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }            
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
