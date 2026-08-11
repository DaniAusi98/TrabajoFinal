using Core.Application;

using System.ComponentModel.DataAnnotations;


namespace Application.VisitaGrupal.UseCases.Comands.CrearVisitaAutoguiada
{
    public class CrearVisitaAutoguiadaCommand : IRequestCommand<string>
    {
        [Required]
        public string UsuarioVisitanteId { get; set; }
        [Required]
        public string Institucion { get; set; }
        [Required]
        public string EmailInstitucion { get; set; }
        [Required]
        public string ProvinciaInstitucion { get; set; }
        [Required]
        public string DepartamentoInstitucion { get; set; }
        [Required]
        public string LocalidadInstitucion { get; set; }
        public string DiversidadFuncionalDescripcion { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;

        [Required]
        public List<int> TematicasIds { get; set; } = [];

        [Required]
        public int CantidadPersonas { get; set; }
        public List<int> SalasIds { get; set; } = [];
        [Required]
        public DateTime Inicio { get; set; }
        [Required]
        public DateTime Fin { get; set; }
        public CrearVisitaAutoguiadaCommand()
        {
        }
    }
}
