using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiCrackChecker.Models;
using ApiCrackChecker.Repositories;
using ApiCrackChecker.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGetCrackChecker.Models;

namespace ApiCrackChecker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        RepositoryProyecto repo;
        HelperToken helper;
        public AuthController(RepositoryProyecto repo, IConfiguration configuration)
        {
            this.repo = repo;
            this.helper = new HelperToken(configuration);
        }

        // Necesitamos un punto de entrada (endpoint) para que el usuario nos envie los datos de su validación
        // Los endpoint auth son POST
        // Lo que recibiremos será Username y Password, que nosotros lo hemos incluido con LoginModel
        [HttpPost]
        [Route("[action]")]
        public IActionResult Login(LoginModel model)
        {
            Usuario u = this.repo.BuscarUsuario(model.Username, model.Password);
            if (u != null)
            {

                // Necesitamos crearnos un Token. El token llevará información de tipo issuer, tiempo de duración, 
                // credenciales del usuario y podemos almacenar info extra dentro del token. 
                // Vamos a almacenar a nuestro empleado
                Claim[] claims = new[]
                {
                    new Claim("UserData", JsonConvert.SerializeObject(u))
                };
                JwtSecurityToken token = new JwtSecurityToken
            (
            issuer: helper.Issuer,
            audience: helper.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256)
            );
                // Devolvemos una respuesta afirmativa con su token
                return Ok(
                    new
                    {
                        response = new JwtSecurityTokenHandler().WriteToken(token)
                    }
                    );
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}