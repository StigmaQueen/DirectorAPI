using System;
using System.Collections.Generic;

namespace DirectorAPI.Models;

public partial class DocenteGrupo
{
    public int Id { get; set; }

    public int IdDocente { get; set; }

    public int IdGrupo { get; set; }

    public int IdPeriodo { get; set; }

    public virtual Docente IdDocenteNavigation { get; set; } = null!;

    public virtual Grupo IdGrupoNavigation { get; set; } = null!;

    public virtual Periodo IdPeriodoNavigation { get; set; } = null!;
}
