using System.Text;
using System.Text.Json;
using PortadoresService.Dtos;

namespace PortadoresService.SyncDataServices.Http
{
    public class HttpContasServiceClient : IContasServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpContasServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task CreatePortador(PortadorReadDto portadorReadDto)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(portadorReadDto), Encoding.UTF8,
                    "application/json");
            var response = await _httpClient.PostAsync($"{_configuration["ContasService"]}", httpContent);

            if(!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }
        }

        public async Task DeletePortadorByCpf(string cpf)
        {
            var response = await _httpClient.DeleteAsync($"{_configuration["ContasService"]}/{cpf}");

            if(!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }
        }
    }
}