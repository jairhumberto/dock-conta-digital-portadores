using System.Text;
using System.Text.Json;
using PortadoresService.Dtos;

namespace PortadoresService.SyncDataServices.Http
{
    public class HttpContaDataClient : IContaDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpContaDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPortadorToConta(PortadorReadDto portadorReadDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(portadorReadDto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["ContasService"]}", httpContent);

            if(!response.IsSuccessStatusCode)
            {
                throw new HttpSyncException();
            }
        }
    }
}