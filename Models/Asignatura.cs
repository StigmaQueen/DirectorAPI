using System;
using System.Collections.Generic;

namespace DirectorAPI.Models;

public partial class Asignatura
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int TipoAsignatura { get; set; }

    public virtual ICollection<Calificacion> Calificacion { get; } = new List<Calificacion>();

    public virtual ICollection<DocenteAsignatura> DocenteAsignatura { get; } = new List<DocenteAsignatura>();
}
