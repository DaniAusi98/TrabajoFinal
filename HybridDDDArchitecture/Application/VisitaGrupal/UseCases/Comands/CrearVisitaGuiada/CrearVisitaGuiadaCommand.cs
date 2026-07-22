using System.ComponentModel.DataAnnotations;

using Core.Application;

using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.VisitaGrupal.UseCases.Comands.CrearVisitaGuiada
{
    public class CrearVisitaGuiadaCommand:IRequestCommand<string>
    {
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
        public DateTime  Inicio { get; set; }
        [Required]
        public DateTime Fin { get; set; }
        public CrearVisitaGuiadaCommand()
        {
        }
    }
}


/*using Core.Application;
using System.ComponentModel.DataAnnotations;
using static Domain.CommonDomain.Enums.Enums;

namespace Application.ApplicationMuseo.UseCases.DummyEntity.Commands.CreateDummyEntity
{
    /// <summary>
    /// Ejemplo de comando para crear una entidad de dominio Dummy
    /// Todo comando debe implementar la interfaz <see cref="IRequestCommand{TResponse}"/> 
    /// si espera una respuesta donde <c TResponse> puede ser cualquier tipo de dato, 
    /// o bien <see cref="IRequestCommand"/> si no espera un valor devuelto
    /// </summary>
    public class CreateDummyEntityCommand : IRequestCommand<string>
    {
        [Required]
        public string dummyPropertyOne { get; set; }
        public DummyValues dummyPropertyTwo { get; set; }

        public CreateDummyEntityCommand()
        {
        }
    }
}
*/
