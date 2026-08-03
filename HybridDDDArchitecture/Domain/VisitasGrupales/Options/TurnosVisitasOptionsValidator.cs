using Microsoft.Extensions.Options;

namespace Domain.VisitasGrupales.Options
{
    public class TurnosVisitasOptionsValidator : IValidateOptions<TurnosVisitasOptions>
    {
        public ValidateOptionsResult Validate(string name, TurnosVisitasOptions options)
        {
            if (options == null)
                return ValidateOptionsResult.Fail("TurnosVisitasOptions no puede ser null.");

            if (options.MinGuiasParaCapacidadCompleta < 1)
                return ValidateOptionsResult.Fail("MinGuiasParaCapacidadCompleta debe ser al menos 1.");

            if (options.CapacidadPorGuia <= 0)
                return ValidateOptionsResult.Fail("CapacidadPorGuia debe ser mayor a 0.");

            if (options.CapacidadMaximaPorTurno <= 0)
                return ValidateOptionsResult.Fail("CapacidadMaximaPorTurno debe ser mayor a 0.");

            if (options.CapacidadMaximaPorTurno < options.CapacidadPorGuia)
                return ValidateOptionsResult.Fail("CapacidadMaximaPorTurno no puede ser menor que CapacidadPorGuia.");

            return ValidateOptionsResult.Success;
        }
    }
}
