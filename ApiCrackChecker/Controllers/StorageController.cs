using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiCrackChecker.Helpers;
using ApiCrackChecker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCrackChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        StorageHelper helper;
        public StorageController(StorageHelper helper)
        {
            this.helper = helper;
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<String>> SubirImagen([FromBody] FileData archivo)
        {
            String mensaje = await this.helper.SubirImagen(archivo.Archivo, archivo.Nombre);
            return mensaje;
        }
    }
}