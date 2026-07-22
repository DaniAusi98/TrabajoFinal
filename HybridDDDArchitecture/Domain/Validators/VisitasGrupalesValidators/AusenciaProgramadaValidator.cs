using Core.Domain.Validators;

using Domain.ActividadMuseo.Others;
using Domain.VisitasGrupales.Entities.Guia;

using FluentValidation;
namespace Domain.Validators.VisitasGrupalesValidators
{
    public class AusenciaProgramadaValidator : EntityValidator<AusenciaGuia>
    {
        public AusenciaProgramadaValidator()
        {
            RuleFor(a => a.GuiaId)
                .GreaterThan(0)
                .WithMessage("El ID del guía debe ser un número válido.");
            RuleFor(a => a.FechaDesde)
                .LessThanOrEqualTo(a => a.FechaHasta)
                .WithMessage("La fecha de inicio no puede ser posterior a la fecha de fin.");
  
            RuleFor(a => a.Motivo)
                .NotEmpty()
                .WithMessage("El motivo de la ausencia es obligatorio.")
                .MaximumLength(500)
                .WithMessage("El motivo de la ausencia no puede exceder los 500 caracteres.");
            RuleFor(a => a.FechaDesde)
                .Must(fecha => fecha.Date >= HoraMuseo.Hoy())
                .WithMessage("La fecha de inicio de la ausencia no puede ser en el pasado.");
            RuleFor(a => a.FechaHasta)
                .Must(fecha => fecha.Date >= HoraMuseo.Hoy())
                .WithMessage("La fecha de fin de la ausencia no puede ser en el pasado.");
        
            
        }
    }
}
