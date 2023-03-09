using System;
using System.Collections.Generic;

namespace DirectorAPI.Models;

public partial class Director
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int Idusuario { get; set; }

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
