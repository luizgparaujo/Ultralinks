using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ultralinks.Application.Interfaces;
using Ultralinks.Application.ViewModels;
using HttpResponse = Ultralinks.API.Helpers.HttpResponse;

namespace Ultralinks.API.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="usuarioVM"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateUsuarioViewModel usuarioVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = new HttpResponse();

            try
            {
                response.Content = await _usuarioService.CreateAsync(usuarioVM);
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
        /// Atualiza o usuário com ID específico.
        /// </summary>
        /// <param name="usuarioVM"></param>
        /// <returns></returns>        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateUsuarioViewModel usuarioVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = new HttpResponse();

            try
            {
                response.Content = await _usuarioService.UpdateAsync(usuarioVM);
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
        /// Retorna um usuário com ID específico.
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
                response.Content = await _usuarioService.GetByIdAsync(id);

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
        /// Atualiza a senha de um usuário com ID específico.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="senha"></param>
        /// <returns></returns>        
        [Authorize]
        [HttpPut]
        [Route("senha")]
        public async Task<IActionResult> PutSenhaAsync(int idUsuario, string senha)
        {
            var response = new HttpResponse();

            try
            {
                await _usuarioService.PutSenhaAsync(idUsuario, senha);
                response.Content = "Senha atualizada com sucesso!";

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
        /// Retorna a lista de usuários.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = new HttpResponse();

            try
            {
                response.Content = await _usuarioService.GetAllAsync();

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
        /// Retorna um usuário com ID específico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = new HttpResponse();

            try
            {
                await _usuarioService.DeleteAsync(id);
                response.Content = $"Usuário com id '{id}' excluído com sucesso.";

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
