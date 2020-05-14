using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiCrackChecker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGetCrackChecker.Models;

namespace ApiCrackChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        RepositoryProyecto repo;
        public UsuariosController(RepositoryProyecto repo)
        {
            this.repo = repo;
        }
        [Authorize]
        [HttpGet]
        public ActionResult<List<Usuario>> Get() 
        {
            return this.repo.GetUsuarios();
        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> Get(int id)
        {
            return this.repo.BuscarUsuario(id);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<Usuario> Buscar(String email, String password)
        {
            return this.repo.BuscarUsuario(email, password);
        }

        [HttpGet]
        [Route("[action]/{email}")]
        public ActionResult<Usuario> BuscarUsuarioEmail(String email)
        {
            return this.repo.BuscarUsuario(email);
        }

        [HttpGet]
        [Route("[action]/{role}")]
        public ActionResult<List<Usuario>> BuscarPorRole(String role)
        {
            return this.repo.BuscarUsuariosPorRole(role);
        }

        [HttpPut]
        [Route("[action]")]
        public void Editar(Usuario u)
        {
            this.repo.EditarUsuario(u.IdUsuario, u.Email, u.Role, u.Foto, u.Nombre, u.Apellidos, u.FechaNacimiento, u.Telefono, u.Username);
        }

        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public void EditarUsuarioAdmin(Usuario user)
        {
            this.repo.EditarUsuario(user.IdUsuario, user.Role, user.Activo);
        }

        [Authorize]
        [HttpPut]
        [Route("[action]/{id}")]
        public void DarBaja(int id)
        {
            this.repo.DarBajaCuenta(id);
        }

        [HttpGet]
        [Route("[action]/{email}/{username}")]
        public ActionResult<String> ComprobarDisponibilidad(String email, String username)
        {
            String jsonobj = this.repo.ComprobarDisponibilidad(email, username);
            return jsonobj;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<Usuario> Perfil()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String json = claims.SingleOrDefault(z => z.Type == "UserData").Value;
            Usuario u = JsonConvert.DeserializeObject<Usuario>(json);
            return u;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{id}")]
        public ActionResult<List<String>> ListaDeseados(int id)
        {
            List<String> deseados = this.repo.GetIdJuegoLista(id);
            return deseados;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<int> Registrar(Usuario u)
        {
            return this.repo.RegistrarUsuario(u.Email, u.Password, u.Salt, u.Username);
        }

        [HttpPut]
        [Route("[action]")]
        public void CambiarPass(Usuario u)
        {
            this.repo.CambiarPassword(u);
        }
    }
}