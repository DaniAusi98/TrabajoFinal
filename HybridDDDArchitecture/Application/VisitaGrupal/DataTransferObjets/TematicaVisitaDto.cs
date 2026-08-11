namespace Application.VisitaGrupal.DataTransferObjets
{
    public class TematicaVisitaDto
    {
        public int Id { get; set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public bool Disponible { get; private set; }
        public IList<int> SalaIds { get; private set; } = [];
    }
}
