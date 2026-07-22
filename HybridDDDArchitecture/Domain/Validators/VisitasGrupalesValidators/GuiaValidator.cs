using Core.Domain.Validators;

using Domain.VisitasGrupales.Entities.Guia;

using FluentValidation;

namespace Domain.Validators.VisitasGrupalesValidators
{
    public class GuiaValidator : EntityValidator<Guia>
    {
        public GuiaValidator()
        {
            RuleFor(g => g.HorariosGuia)
                .NotNull()
                .WithMessage("La lista de horarios del guía no puede ser nula.")
                .NotEmpty()
                .WithMessage("El horario del guía es obligatorio.");

            RuleFor(g => g.PersonalInternoId)
                .GreaterThan(0)
                .WithMessage("El usuario interno asociado al guía es obligatorio.");

          
            
        }
    }
}
