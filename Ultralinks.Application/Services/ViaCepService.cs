using Newtonsoft.Json;
using Ultralinks.Application.Interfaces;

namespace Ultralinks.Application.Services
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ViaCepResponse> GetAddressByCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"{cep}/json/");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar endereço pelo CEP '{cep}'.");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ViaCepResponse>(result);
        }
    }

    public class ViaCepResponse
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Ibge { get; set; }
        public string Gia { get; set; }
        public string Ddd { get; set; }
        public string Siafi { get; set; }
    }

}
