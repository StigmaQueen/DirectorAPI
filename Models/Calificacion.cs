using System;
using System.Collections.Generic;

namespace DirectorAPI.Models;

public partial class Calificacion
{
    public int Id { get; set; }

    public int IdAsignatura { get; set; }

    public int IdAlumno { get; set; }

    public int IdDocente { get; set; }

    public int IdPeriodo { get; set; }

    public double Calificacion1 { get; set; }

    public int Unidad { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual Asignatura IdAsignaturaNavigation { get; set; } = null!;

    public virtual Docente IdDocenteNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;
}
