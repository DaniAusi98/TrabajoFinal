using Core.Domain.Validators;

using Domain.Entities.DisponibilidadMuseo;

using FluentValidation;
namespace Domain.Validators.DisponibilidadMuseo
{
    public class DiaCierreMuseoValidator:EntityValidator<DiaCierreMuseo>
    {
        public DiaCierreMuseoValidator()
        {
            RuleFor(x => x.Fecha)
                 .NotEmpty().NotNull().WithMessage("La fecha de inicio es obligatoria.");

            RuleFor(x => x.FechaHasta)
                .GreaterThanOrEqualTo(x => x.Fecha)
                .WithMessage("La fecha de fin debe ser mayor o igual a la fecha de inicio.");

            RuleFor(x => x.Motivo)
                .IsInEnum().WithMessage("El motivo de cierre debe ser un valor válido del enum MotivoCierreMuseo.");

            RuleFor(x => x.Observaciones)
                .MaximumLength(500).WithMessage("Las observaciones no pueden exceder los 500 caracteres.");

        }    
    }
}
