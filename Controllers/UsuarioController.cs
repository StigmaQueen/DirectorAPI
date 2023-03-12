using DirectorAPI.Data;
using DirectorAPI.DTO;
using DirectorAPI.Models;
using DirectorAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DirectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        Repository<Usuario> repositories;
        Repository<Director> directorrepository;
        public UsuarioController(Sistem21PrimariaContext conetxt)
        {
            repositories = new(conetxt);
        }
        public IActionResult Get()
        {
            var usuarios = repositories.Get().OrderBy(x => x.Id).ToList();
            if(usuarios.Count== 0)
            {
                return NotFound();
            }
            return Ok(usuarios);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var usuario = repositories.Get().Where(x => x.Id == id).FirstOrDefault();
            if(usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        [HttpPost("login")]
        public IActionResult PostLogin(UsuarioDTO usuario)
        {
            var u = repositories.Get().FirstOrDefault(x => x.Usuario1 == usuario.Usuario1 && x.Contraseña == usuario.Contraseña);
            if (u == null || u.Rol != 1)
            {
                return NotFound("Usuario o Contraseña Incorrectos");
            }
            var d = directorrepository.Get().FirstOrDefault(x => x.Idusuario == u.Id);
            Director director;
            director = new Director
            {
                Id = d.Id,
                Nombre = d.Nombre,
                Telefono = d.Telefono,
                Direccion = d.Direccion,
                Idusuario = d.Idusuario
            };
            return Ok(d);
        }
        [HttpPost]
        public IActionResult Post(UsuarioDTO usuario)
        {
            if (usuario == null)
            {
                return NotFound();
            }
            if (Validar(usuario, out List<string> errors))
            {
                Usuario usuario1 = new Usuario()
                {
                    Usuario1 = usuario.Usuario1,
                    Rol = 2,
                    Contraseña = usuario.Contraseña

                };
                repositories.Insert(usuario1);
                return Ok(usuario1.Id);
            }
            return BadRequest(errors);
        }
        [HttpPut]
        public IActionResult Put(UsuarioDTO usuario)
        {
            var usu = repositories.Get(usuario.Id);

            if (usu == null)
            {
                NotFound();
            }
            if (Validar(usuario, out List<string> errors))
            {
                usu.Usuario1 = usuario.Usuario1;
                usu.Contraseña = usuario.Contraseña;

                repositories.Update(usu);
                return Ok();
            }
            return BadRequest(errors);
        }

        [HttpDelete]
        public IActionResult Delete(UsuarioDTO usu)
        {
            var usuario = repositories.Get(usu.Id);
            
            if (usuario != null)
            {
                repositories.Delete(usuario);
                return Ok();
            }
            else
                return NotFound("El usuario no existe o ya ha sido eliminado");

        }

        private bool Validar(UsuarioDTO usuario, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(usuario.Usuario1))
            {
                errors.Add("El nombre de usuario no puede ir vacio");
            }

            if (repositories.Get().Any(x => x.Usuario1 == usuario.Usuario1 && x.Id != usuario.Id))
            {
                errors.Add("Ya existe un usuario con el mismo nombre, ingresa uno diferente");
            }
            
           
            if (string.IsNullOrWhiteSpace(usuario.Contraseña))
            {
                errors.Add("La contraseña no puede ir vacia. Ingresa una");
            }
            
            return errors.Count == 0;
        }
    }
}
