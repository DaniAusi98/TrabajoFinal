
using Core.Domain.Validators;

using Domain.VisitasGrupales.Entities;

using FluentValidation;

namespace Domain.Validators.VisitasGrupalesValidators
{
    public class TematicasValidator:EntityValidator<TematicaVisita>
    {
        public TematicasValidator()
        {
            RuleFor(t => t.Nombre)
                .NotEmpty()
                .WithMessage("El nombre de la temática es obligatorio.")
                .MaximumLength(100)
                .WithMessage("El nombre de la temática no puede exceder los 100 caracteres.");

            RuleFor(t => t.Descripcion)
                .NotEmpty()
                .WithMessage("La descripción de la temática es obligatoria.")
                .MaximumLength(500)
                .WithMessage("La descripción de la temática no puede exceder los 500 caracteres.");
        }
    }
}
