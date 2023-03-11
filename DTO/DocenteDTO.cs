namespace DirectorAPI.DTO
{
    public class DocenteDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string ApellidoPaterno { get; set; } = null!;

        public string ApellidoMaterno { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public int Edad { get; set; }

        public int TipoDocente { get; set; }

        public int IdUsuario { get; set; }
        public int IdGrupo { get; set; }
        public int IdAsignatura { get; set; }

        
    }
}
