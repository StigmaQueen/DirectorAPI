using System;
using System.Collections.Generic;

namespace DirectorAPI.Models;

public partial class AlumnoTutor
{
    public int Id { get; set; }

    public int IdTutor { get; set; }

    public int IdAlumno { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual Tutor IdTutorNavigation { get; set; } = null!;
}
