

using DirectorAPI.Data;
using DirectorAPI.DTO;
using DirectorAPI.Models;
using DirectorAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DirectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocenteController : ControllerBase
    {
        Repository<Docente> repositories;
        Repository<Usuario> repositoriesUsuario;
        Repository<DocenteGrupo> repositoriesGrupoP;
        Repository<Asignatura> repositoriasignatra;
        Repository<Grupo> repositorigrupo;
        public DocenteController(Sistem21PrimariaContext conetxt)
        {
            repositories = new(conetxt);
            repositoriesUsuario = new(conetxt);
            repositoriesGrupoP = new(conetxt);
            repositoriasignatra = new(conetxt);
            repositorigrupo = new(conetxt);
        }
        public IActionResult Get()
        {
            var docentes = repositories.Get().OrderBy(x => x.Id).ToList();
            if (docentes.Count == 0)
            {
                return NotFound();
            }
            return Ok(docentes);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var docente = repositories.Get().Where(x => x.Id == id).FirstOrDefault();
            if (docente == null)
            {
                return NotFound();
            }
            return Ok(docente);
        }
        [HttpPost]
        public IActionResult Post(DocenteDTO docente)
        {

            if (Validar(docente, out List<string> errors))
            {
                Docente docent = new Docente()
                {
                    Nombre = docente.Nombre,
                    ApellidoMaterno = docente.ApellidoMaterno,
                    ApellidoPaterno = docente.ApellidoPaterno,
                    Correo = docente.Correo,
                    Telefono = docente.Telefono,
                    Edad = docente.Edad,
                    TipoDocente = docente.TipoDocente,
                    IdUsuario = docente.IdUsuario,
                   

                };
                
                repositories.Insert(docent);
                PostGrupo(docente);
                return Ok();
            }
            return BadRequest(errors);
        }
        public void PostGrupo( DocenteDTO docente)
        {
            var asignatura = repositoriasignatra.Get(docente.IdAsignatura);
            if (asignatura == null)
                throw new ApplicationException("Asignatura no encontrada");

            if (docente.TipoDocente == 1 && repositoriesGrupoP.Get().Include(x => x.IdDocenteNavigation).
                Any(x => x.IdGrupo == docente.IdGrupo && x.IdDocenteNavigation.TipoDocente != 2)&&docente.IdAsignatura==0)
            {

                var status = BadRequest("Este grupo ya tiene un profesor de grupo asignado");
            }
            else
            {
                //var grupo = repositoriesGrupoP.Get().Where(x => x.Id == id).FirstOrDefault();
                //grupo.IdDocente = docente.IdGrupo;

                DocenteGrupo docente_grupo = new DocenteGrupo()
                {
                    IdDocente = docente.Id,
                    IdGrupo = docente.IdGrupo
                };

                repositoriesGrupoP.Insert(docente_grupo);
            }

            if (docente.TipoDocente == 2 && repositoriesGrupoP.Get().Include(x => x.IdDocenteNavigation).Include(x=>x.IdGrupoNavigation).
                Any(x => x.IdGrupo == docente.IdGrupo && x.IdDocenteNavigation.TipoDocente != 1))
            {
                
                var grupos = repositoriesGrupoP.Get().OrderBy(x => x.Id).ToList();
            
                if(grupos.Any(x=>x.IdGrupo==docente.IdGrupo))
                {
                    var status = BadRequest("Este grupo ya tiene un profesor de materia asignado");
                }

                var gruposactuales = repositorigrupo.Get().OrderBy(x => x.Grado);

                foreach (var item in gruposactuales)
                {
                    DocenteGrupo docente_grupo = new DocenteGrupo()
                    {
                        IdDocente = docente.Id,
                        IdGrupo = item.Id
                    };

                    repositoriesGrupoP.Insert(docente_grupo);

                }
               
            }
           

        }

        private bool Validar(DocenteDTO docente, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(docente.Nombre))
            {
                errors.Add("Ingrese el nombre del docente");
            }
            if (string.IsNullOrWhiteSpace(docente.ApellidoPaterno))
            {
                errors.Add("Ingrese el primer apellido del docente");
            }
            if (string.IsNullOrWhiteSpace(docente.ApellidoMaterno))
            {
                errors.Add("Ingrese el segundo apellido del docente");
            }
            if (docente.Edad < 18)
            {
                errors.Add("El docente a agregar debe ser mayor de edad");
            }
            if (string.IsNullOrWhiteSpace(docente.Telefono))
            {
                errors.Add("Ingrse el numero telefonico del docente");
            }
            if (docente.TipoDocente < 1 || docente.TipoDocente > 2)
            {
                errors.Add("Solo puede haber tipo de docente 1 o 2");
            }
            if (repositories.Get().Any(x => x.IdUsuario == docente.IdUsuario))
            {
                errors.Add("Este usuario ya está vinculado con un docente, intente con otro");
            }
            return errors.Count == 0;
        }
    }

}
