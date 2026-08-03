using System.ComponentModel.DataAnnotations;
using Core.Application;
using Application.VisitaGrupal.DataTransferObjets;

namespace Application.VisitaGrupal.UseCases.Comands.UpdateGuia
{
    public class UpdateGuiaCommand : IRequestCommand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public int PersonalInternoId { get; set; }
        [Required]
        public List<GuiaHorarioDto> Horarios { get; set; } = new();
        public bool Activo { get; set; } = true;
    }
}
