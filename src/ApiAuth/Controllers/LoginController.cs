using ApiAuth.Models;
using ApiAuth.Repositories;
using ApiAuth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiAuth.Controllers
{
    [ApiController]
    [Route("V1")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public ActionResult<dynamic> Authenticate([FromBody] User model)
        {
            // Recupera o user
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera token
            var token = TokenService.GenerateToken(user);

            // Gera RefreshToken (Se implementado)
            var refreshToken = TokenService.GenerateRefreshToken();

            // Salva o refresh token (Poderia salvar em um banco de dados)
            TokenService.SaveRefreshToken(user.Username, refreshToken);

            // Oculta senha
            user.Password = "";

            // Retorna os dados
            return new
            {
                User = user,
                token = token,
                refreshToken = refreshToken
            };
        }

        [HttpPost]
        [Route("refresh")]
        public ActionResult<dynamic> Refresh(string token, string refreshToken)
        {
            var principal = TokenService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = TokenService.GetRefreshToken(username);

            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh");

            var newJwtToken = TokenService.GenerateToken(principal.Claims);
            var newRefreshToken = TokenService.GenerateRefreshToken();
            TokenService.DeleteRefreshToken(username, refreshToken);
            TokenService.SaveRefreshToken(username, newRefreshToken);

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }
    }
}
