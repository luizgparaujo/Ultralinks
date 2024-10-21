using System.Security.Claims;

namespace Ultralinks.API.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetUsuarioId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("usuarioId");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Token inválido ou expirado.");

            if (!int.TryParse(userIdClaim.Value, out var usuarioId))
                throw new UnauthorizedAccessException("ID do usuário no token está inválido.");

            return usuarioId;
        }
    }
}
