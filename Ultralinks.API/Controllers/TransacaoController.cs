using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ultralinks.API.Extensions;
using Ultralinks.Application.Interfaces;
using Ultralinks.Application.ViewModels;
using HttpResponse = Ultralinks.API.Helpers.HttpResponse;

namespace Ultralinks.API.Controllers
{
    [ApiController]
    [Route("api/v1/transacoes")]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoService _transacaoService;

        public TransacaoController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService ?? throw new ArgumentNullException(nameof(transacaoService));
        }

        /// <summary>
        /// Cria um novo depósito.
        /// </summary>
        /// <param name="transacaoVM"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("depositos")]
        public async Task<IActionResult> DepositarAsync([FromBody] CreateTransacaoViewModel transacaoVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = new HttpResponse();

            try
            {
                var usuarioId = User.GetUsuarioId();
                response.Content = await _transacaoService.DepositarAsync(transacaoVM, usuarioId);
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;

                return BadRequest(response);
            }
        }

        /// <summary>
        /// Cria uma nova transferência.
        /// </summary>
        /// <param name="transacaoVM"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("transferencias")]
        public async Task<IActionResult> TransferirAsync([FromBody] CreateTransacaoViewModel transacaoVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = new HttpResponse();

            try
            {
                var usuarioId = User.GetUsuarioId();
                response.Content = await _transacaoService.TransferirAsync(transacaoVM, usuarioId);
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;

                return BadRequest(response);
            }
        }

        /// <summary>
        /// Retorna uma transacao com ID específico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = new HttpResponse();

            try
            {
                response.Content = await _transacaoService.GetByIdAsync(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        /// <summary>
        /// Retorna uma lista de transações.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = new HttpResponse();

            try
            {
                response.Content = await _transacaoService.GetAllAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }
    }
}
