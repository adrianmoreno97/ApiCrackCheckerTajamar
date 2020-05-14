using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCrackChecker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGetCrackChecker.Models;

namespace ApiCrackChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegosController : ControllerBase
    {
        RepositoryProyecto repo;
        public JuegosController(RepositoryProyecto repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        public ActionResult<List<Juego>> Get()
        {
            return this.repo.GetJuegos();
        }

        [HttpGet("{id}")]
        public ActionResult<Juego> Get(String id)
        {
            return this.repo.BuscarJuegoId(id);
        }

        [HttpGet]
        [Route("[action]/{slug}")]
        public ActionResult<Juego> BuscarJuegoSlug(String slug)
        {
            return this.repo.BuscarJuegoSlug(slug);
        }

        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public void Editar(Juego j)
        {
            this.repo.EditarJuego(j.IdJuego, j.FechaCrack, j.FechaLanzamiento);
        }

        [HttpGet]
        [Route("[action]/{busqueda}")]
        public ActionResult<List<Juego>> BuscarJuegos(String busqueda)
        {
            return this.repo.BuscarJuegos(busqueda);
        }

        [HttpGet]
        [Route("[action]/{posicion}")]
        public ActionResult<List<Juego>> PaginarJuegos(int posicion)
        {
            return this.repo.PaginarJuegos(posicion);
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idjuego}/{iduser}")]
        public void InsertarJuegoLista(String idjuego, int iduser)
        {
            this.repo.AnyadirJuegoListaDeseados(idjuego, iduser);
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]/{idjuego}/{iduser}")]
        public void EliminarJuegoLista(String idjuego, int iduser)
        {
            this.repo.EliminarJuegoListaDeseados(idjuego, iduser);
        }
    }
}