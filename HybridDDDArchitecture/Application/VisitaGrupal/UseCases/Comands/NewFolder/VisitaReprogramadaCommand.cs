using System.ComponentModel.DataAnnotations;

using Core.Application;

using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.VisitaGrupal.UseCases.Comands.NewFolder
{
    public class ReprogramarCommand : IRequestCommand<string>
    {
        [Required]
        public int VisitaReprogramadaId { get; set; }
        [Required]
        public string UsuarioVisitanteId { get; set; }

        [Required]
        public string Institucion { get; set; }

        public NivelEducativo? NivelEducativo { get; set; }

        public int? AnioGrado { get; set; }

        [Required]
        public string EmailInstitucion { get; set; }

        [Required]
        public string TelefonoInstitucion { get; set; }

        [Required]
        public string ProvinciaInstitucion { get; set; }

        [Required]
        public string DepartamentoInstitucion { get; set; }

        [Required]
        public string LocalidadInstitucion { get; set; }

        public string DiversidadFuncionalDescripcion { get; set; } = string.Empty;

        public string MotivoRelacionVisita { get; set; } = string.Empty;

        public string Observaciones { get; set; } = string.Empty;

        [Required]
        public int CantidadPersonas { get; set; }

        public List<int> TematicasIds { get; set; } = [];
        [Required]
        public DateTime Inicio { get; set; }
        [Required]
        public DateTime Fin { get; set; }
       
        public ReprogramarCommand() { } 

    }
    
}
