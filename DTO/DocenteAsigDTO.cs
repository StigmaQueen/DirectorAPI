namespace DirectorAPI.DTO
{
    public class DocenteAsigDTO
    {

        public int Id { get; set; }
        public int TipoDocente { get; set; }
        public int IdGrupo { get; set; }
        public int IdAsignatura { get; set; }
        public List<GrupoDTO>? IdGrupos { get; set; } = new List<GrupoDTO>();
    }
}
