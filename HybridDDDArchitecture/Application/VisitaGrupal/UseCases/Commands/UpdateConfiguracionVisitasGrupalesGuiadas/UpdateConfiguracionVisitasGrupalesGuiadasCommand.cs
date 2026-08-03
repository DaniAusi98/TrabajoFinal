using System.ComponentModel.DataAnnotations;

using MediatR;

namespace Application.VisitaGrupal.UseCases.Commands.UpdateConfiguracionVisitasGrupalesGuiadas
{
    public class UpdateConfiguracionVisitasGrupalesGuiadasCommand : IRequest
    {
        [Required]
        public int MinGuiasParaCapacidadCompleta { get; set; }
        [Required]
        public int CapacidadPorGuia { get; set; }
        [Required]
        public int CapacidadMaximaPorTurno { get; set; }
    }
}
