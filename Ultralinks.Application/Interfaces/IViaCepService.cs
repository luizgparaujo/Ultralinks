using Ultralinks.Application.Services;

namespace Ultralinks.Application.Interfaces
{
    public interface IViaCepService
    {
        Task<ViaCepResponse> GetAddressByCepAsync(string cep);
    }
}
