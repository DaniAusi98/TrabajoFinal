using Core.Domain.Validators;


using Domain.Entities.VisitasGrupalesMuseo;
using Domain.Validators.DisponibilidadMuseo;

using FluentValidation;

namespace Domain.Validators.VisitasGrupalesValidators
{
    public class VisitaGuiadaValidator : EntityValidator<VisitaGrupalGuiada>
    {
        public VisitaGuiadaValidator()
        {
            RuleFor(i=> i.UsuarioVisitanteId)
                .NotNull()
                .NotEmpty().WithMessage("El ID del usuario visitante es obligatorio.");




            RuleFor(i => i.AnioGrado)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Debe especificar el año o grado.")
                .InclusiveBetween(1, 8).WithMessage("El año/grado debe estar entre 1 y 8.")
                .When(i => i.NivelEducativo != null);

   

            RuleFor(x => x.EmailInstitucion)
                .NotNull().WithMessage("El email es obligatorio.");

            RuleFor(x => x.TelefonoInstitucion)
                .NotNull().WithMessage("El teléfono es obligatorio.");

            RuleFor(v => v.Tematicas)
                .NotEmpty().WithMessage("Debe seleccionar al menos una temática.");

            RuleFor(v => v.Tematicas)
                .Must(t => t.Select(x => x.Id).Distinct().Count() == t.Count)
                .WithMessage("No se pueden repetir temáticas.");



            RuleFor(v => v.Institucion)
                .NotEmpty().WithMessage("El nombre de la institución es obligatorio.");
           
            RuleFor(x => x.TelefonoInstitucion)
            .NotEmpty().WithMessage("El teléfono es obligatorio.");
           
            RuleFor(x => x.EmailInstitucion)
                .NotEmpty().WithMessage("El email es obligatorio.");

            RuleFor(v => v.ProvinciaInstitucion)
                .NotEmpty().WithMessage("La provincia de la institución es obligatoria.");

            RuleFor(v => v.DepartamentoInstitucion)
                 .NotEmpty().WithMessage("El departamento de la institución es obligatorio.");

            RuleFor(v => v.LocalidadInstitucion)
                .NotEmpty().WithMessage("La ciudad de la institución es obligatoria.");

            RuleFor(x => x.DiversidadFuncionalDescripcion)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.DiversidadFuncionalDescripcion));


            RuleFor(x => x.MotivoRelacionVisita)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.MotivoRelacionVisita));

            RuleFor(x => x.Observaciones)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Observaciones));

            RuleFor(x => x.NivelEducativo)
                .IsInEnum()
                .WithMessage("El nivel educativo no es válido.");
            RuleFor(x => x.Tematicas)
                .NotNull()
                .WithMessage("La lista de temáticas no puede ser nula.")
                .NotEmpty()
                .WithMessage("Debe haber al menos una temática seleccionada.");



            RuleFor(x => x.ActividadMuseo)
           .NotNull()
           .SetValidator(new ActividadMuseoValidator());
            

        }
    }
}
