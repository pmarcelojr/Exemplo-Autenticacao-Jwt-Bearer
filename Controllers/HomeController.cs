using System.Threading.Tasks;
using ExemploAutenticacao.Models;
using ExemploAutenticacao.Repositories;
using ExemploAutenticacao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExemploAutenticacao.Controllers
{
    [Route("v1/account")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            // Recupera o usuário
            var user = UserRepository.GetUser(model.Username, model.Password);

            // verifica se o usuário existe
            if (user == null)
                return NotFound(new
                {
                    message = "Usuário ou senha inválidos"
                });

            // gera o token
            var token = TokenService.GenerateToken(user);
            // oculta a senha
            user.Password = "";
            // retorna os dados
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonumous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => string.Format($"Auntenticado - {User.Identity.Name}");

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";

    }
}