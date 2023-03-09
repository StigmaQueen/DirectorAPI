namespace DirectorAPI.DTO
{
    public class UsuarioDTO
    {

        public int Id { get; set; }

        public string Usuario1 { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public int Rol { get; set; }
    }
}
