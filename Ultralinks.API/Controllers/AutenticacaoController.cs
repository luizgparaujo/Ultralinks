using Microsoft.AspNetCore.Mvc;
using Ultralinks.Application.Interfaces;
using HttpResponse = Ultralinks.API.Helpers.HttpResponse;

namespace Ultralinks.API.Controllers
{
    [ApiController]
    [Route("api/v1/autenticacoes")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AutenticacaoController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService ?? throw new ArgumentNullException(nameof(autenticacaoService));
        }

        /// <summary>
        /// Autenticar um usuário.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(string email, string senha)
        {
            var response = new HttpResponse();

            try
            {
                response.Content = await _autenticacaoService.AutenticarAsync(email, senha);
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;

                return BadRequest(response);
            }
        }
    }
}
