namespace Application.VisitaGrupal.DataTransferObjets
{
    public class GuiaDto
    {
        public string NombreCompleto { get; private set; } = string.Empty;
        public int PersonalInternoId { get; private set; }
        public bool Activo { get; private set; }
        public IList<GuiaHorarioDto> Horarios { get; private set; } = new List<GuiaHorarioDto>();
    }
}
