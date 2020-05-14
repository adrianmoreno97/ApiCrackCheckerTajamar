using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCrackChecker.Helpers;
using ApiCrackChecker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCrackChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageController : ControllerBase
    {
        HelperCorreo helper;
        RepositoryProyecto repo;
        public ManageController(HelperCorreo helper, RepositoryProyecto repo)
        {
            this.helper = helper;
            this.repo = repo;
        }
        [HttpGet]
        [Route("[action]/{id}/{email}")]
        public async Task<ActionResult<String>> EnviarEmailActivacion(int id, String email)
        {
            String msj = await helper.EnviarEmail(email, id, "activar");
            return msj;
        }

        [HttpGet]
        [Route("[action]/{email}")]
        public async Task<ActionResult<String>> EnviarEmailRecuperacion(String email)
        {
            String msj = await helper.EnviarEmail(email, 0, "recuperar");
            return msj;
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public void Activar(int id)
        {
            this.repo.ActivarCuenta(id);
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]/{id}")]
        public void EliminarUsuario(int id)
        {
            this.repo.EliminarUsuario(id);
        }
    }
}