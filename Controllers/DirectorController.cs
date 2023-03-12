using DirectorAPI.Data;
using DirectorAPI.DTO;
using DirectorAPI.Models;
using DirectorAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DirectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        Repository<Director> repository;
        public DirectorController(Sistem21PrimariaContext context)
        {
            repository = new Repository<Director>(context);
        }
        [HttpGet("{Id}")]
        public ActionResult GetDirector(int d)
        {
            var director = repository.Get(d);
            if (director == null)
                return NotFound();

            return Ok(director);
        }

        [HttpPut]
        public IActionResult Update(DirectorDTO directordto)
        {
            var director = repository.Get(directordto);
            if (director == null) return NotFound();
            if (string.IsNullOrWhiteSpace(directordto.Nombre))
            {
                return BadRequest("El nombre no puede ir vacio");
            }
            if (string.IsNullOrWhiteSpace(directordto.Telefono))
            {
                return BadRequest("El telefono no pude ir vacio");
            }
            if (string.IsNullOrWhiteSpace(directordto.Direccion))
            {
                return BadRequest("La direccion no puede ir vacia");
            }

            director.Nombre = directordto.Nombre;
            director.Telefono = directordto.Telefono;
            director.Direccion = directordto.Direccion;
            repository.Update(director);
            return Ok(director);

        }
    }
}
