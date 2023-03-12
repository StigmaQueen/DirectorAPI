

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
        Repository<Periodo> repositoriesPeriodo;
        Repository<DocenteAsignatura> repositoridocenteasignatura;
        public DocenteController(Sistem21PrimariaContext conetxt)
        {
            repositories = new(conetxt);
            repositoriesUsuario = new(conetxt);
            repositoriesGrupoP = new(conetxt);
            repositoriasignatra = new(conetxt);
            repositorigrupo = new(conetxt);
            repositoriesPeriodo = new(conetxt);
            repositoridocenteasignatura = new(conetxt);
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
            try
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

                    return Ok();
                }
                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public IActionResult Put(DocenteDTO docente)
        {

            var docent = repositories.Get(docente.Id);
            if (Validar(docente, out List<string> errors))
            {
                docent.Nombre = docente.Nombre;
                docent.ApellidoMaterno = docente.ApellidoMaterno;
                docent.ApellidoPaterno = docente.ApellidoPaterno;
                docent.Correo = docente.Correo;
                docent.Telefono = docente.Telefono;
                docent.Edad = docente.Edad;
                docent.TipoDocente = docente.TipoDocente;
                docent.IdUsuario = docente.IdUsuario;


                repositories.Update(docent);

                return Ok();
            }
            return BadRequest(errors);

        }
        [HttpPost("AsignarGrupoMateria")]
        public IActionResult PostG(DocenteAsigDTO docente)
        {
            var usu = repositories.Get(docente.Id);
            int maxperiodo = repositoriesPeriodo.Get().Max(x => x.Id);


            if (docente.TipoDocente == 1)
            {
                docente.IdAsignatura = 0;

                if (repositoriesGrupoP.Get().Include(x => x.IdDocenteNavigation).
                    Any(x => x.IdGrupo == docente.IdGrupo && x.IdDocenteNavigation.TipoDocente != 2) && docente.IdAsignatura == 0)
                {

                    return BadRequest("Este grupo ya tiene un profesor de grupo asignado");
                }
                else
                {
                    //var grupo = repositoriesGrupoP.Get().Where(x => x.Id == id).FirstOrDefault();
                    //grupo.IdDocente = docente.IdGrupo;

                    DocenteGrupo docente_grupo = new DocenteGrupo()
                    {
                        IdDocente = docente.Id,
                        IdGrupo = docente.IdGrupo,
                        IdPeriodo = maxperiodo
                    };

                    repositoriesGrupoP.Insert(docente_grupo);


                    var asignaturasordinarias = repositoriasignatra.Get().Where(x => x.TipoAsignatura == 1);

                    foreach (var asignatura in asignaturasordinarias.ToList())
                    {
                        DocenteAsignatura docenteAsignatura = new()
                        {
                            IdAsignatura = asignatura.Id,
                            IdDocente = docente_grupo.IdDocente
                        };

                        repositoridocenteasignatura.Insert(docenteAsignatura);
                    }
                    return Ok();
                }

            }
            else
            {
                docente.IdGrupo = 0;

                //if (docente.TipoDocente == 2 && repositoriesGrupoP.Get().Include(x => x.IdDocenteNavigation).Include(x => x.IdGrupoNavigation).
                //    Any(x => x.IdGrupo == docente.IdGrupo && x.IdDocenteNavigation.TipoDocente != 1))
                //{
                //    throw new ApplicationException("Este grupo ya tiene un profesor de asignatura asignado");
                //}
                var asignatura = repositoriasignatra.Get(docente.IdAsignatura);

                if (asignatura == null)
                    return BadRequest("Asignatura no encontrada");

                if (repositoridocenteasignatura.Get().Any(x => x.IdAsignatura == docente.IdAsignatura &&x.IdDocente!=docente.Id))
               
                   
                          return BadRequest("Ya hay un profesor con esa asignatura asignado a esos grupos");
                   



                //var grupos = repositoriesGrupoP.Get().OrderBy(x => x.Id).ToList();

                //if (grupos.Any(x => x.IdGrupo == docente.IdGrupo))
                //{
                //    throw new ApplicationException("Este grupo ya tiene un profesor de materia asignado");
                //}
                //var gruposSe = docente.IdGrupos.ToList();


                var gruposSeleccionados = repositorigrupo.Get().OrderBy(x => x.Grado).ToList();
                

                foreach (var item in docente.IdGrupos.ToList())
                {
                    DocenteGrupo docente_grupo = new DocenteGrupo()
                    {
                        IdDocente = docente.Id,
                        IdGrupo = item.Id,
                        IdPeriodo = maxperiodo
                    };

                    repositoriesGrupoP.Insert(docente_grupo);

                }
                DocenteAsignatura docenteAsignatura = new()
                {
                    IdAsignatura = asignatura.Id,
                    IdDocente = docente.Id
                };

                repositoridocenteasignatura.Insert(docenteAsignatura);
                return Ok();
            }
        }

        //public void PostGrupo(DocenteAsigDTO docente)
        //{
        //    int maxperiodo = repositoriesPeriodo.Get().Max(x => x.Id);


        //    if (docente.TipoDocente == 1)
        //    {
        //        docente.IdAsignatura = 0;

        //        if (repositoriesGrupoP.Get().Include(x => x.IdDocenteNavigation).
        //            Any(x => x.IdGrupo == docente.IdGrupo && x.IdDocenteNavigation.TipoDocente != 2) && docente.IdAsignatura == 0)
        //        {

        //            throw new ApplicationException("Este grupo ya tiene un profesor de grupo asignado");
        //        }
        //        else
        //        {
        //            var grupo = repositoriesGrupoP.Get().Where(x => x.Id == id).FirstOrDefault();
        //            grupo.IdDocente = docente.IdGrupo;

        //            DocenteGrupo docente_grupo = new DocenteGrupo()
        //            {
        //                IdDocente = docente.Id,
        //                IdGrupo = docente.IdGrupo,
        //                IdPeriodo = maxperiodo
        //            };

        //            repositoriesGrupoP.Insert(docente_grupo);


        //            var asignaturasordinarias = repositoriasignatra.Get().Where(x => x.TipoAsignatura == 1);

        //            foreach (var asignatura in asignaturasordinarias.ToList())
        //            {
        //                DocenteAsignatura docenteAsignatura = new()
        //                {
        //                    IdAsignatura = asignatura.Id,
        //                    IdDocente = docente_grupo.IdDocente
        //                };

        //                repositoridocenteasignatura.Insert(docenteAsignatura);
        //            }

        //        }
        //    }
        //    else
        //    {
        //        docente.IdGrupo = 0;

        //        if (docente.TipoDocente == 2 && repositoriesGrupoP.Get().Include(x => x.IdDocenteNavigation).Include(x => x.IdGrupoNavigation).
        //            Any(x => x.IdGrupo == docente.IdGrupo && x.IdDocenteNavigation.TipoDocente != 1))
        //        {
        //            throw new ApplicationException("Este grupo ya tiene un profesor de asignatura asignado");
        //        }
        //        var asignatura = repositoriasignatra.Get(docente.IdAsignatura);

        //        if (asignatura == null)
        //            throw new ApplicationException("Asignatura no encontrada");

        //        if (repositoridocenteasignatura.Get().Any(x => x.IdAsignatura == docente.IdAsignatura))
        //            throw new ApplicationException("Ya hay un profesor con esa asignatura");


        //        var grupos = repositoriesGrupoP.Get().OrderBy(x => x.Id).ToList();

        //        if (grupos.Any(x => x.IdGrupo == docente.IdGrupo))
        //        {
        //            throw new ApplicationException("Este grupo ya tiene un profesor de materia asignado");
        //        }

        //        var gruposactuales = repositorigrupo.Get().OrderBy(x => x.Grado).ToList();

        //        foreach (var item in gruposactuales.ToList())
        //        {
        //            DocenteGrupo docente_grupo = new DocenteGrupo()
        //            {
        //                IdDocente = docente.Id,
        //                IdGrupo = item.Id,
        //                IdPeriodo = maxperiodo
        //            };

        //            repositoriesGrupoP.Insert(docente_grupo);

        //        }
        //        DocenteAsignatura docenteAsignatura = new()
        //        {
        //            IdAsignatura = asignatura.Id,
        //            IdDocente = docente.Id
        //        };

        //        repositoridocenteasignatura.Insert(docenteAsignatura);
        //    }


        //}
        [HttpDelete]
        public IActionResult Delete(DocenteDTO docnt)
        {

            var objeto = repositories.Get().Find(docnt.Id);

            if (objeto == null)
            {
                return NotFound();
            }

            // Buscar los registros asociados en la tabla "TablaSecundaria1"
            var registrosTablaSecundaria1 = repositoriesGrupoP.Get().Where(r => r.IdDocente ==docnt.Id);

            // Buscar los registros asociados en la tabla "TablaSecundaria2"
            var registrosTablaSecundaria2 = repositoridocenteasignatura.Get().Where(r => r.IdDocente ==docnt.Id);

            // Eliminar los registros asociados en la tabla "TablaSecundaria1"
            repositoriesGrupoP.Get().RemoveRange(registrosTablaSecundaria1);

            // Eliminar los registros asociados en la tabla "TablaSecundaria2"
            repositoridocenteasignatura.Get().RemoveRange(registrosTablaSecundaria2);

            // Guardar los cambios en la base de datos

            repositories.Delete(objeto);
            return Ok();

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
