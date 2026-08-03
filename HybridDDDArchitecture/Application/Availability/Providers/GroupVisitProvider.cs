using System.Collections.Generic;
using System.Linq;
using Application.Availability.Rules;
using Domain.ActividadMuseo.Entities;

namespace Application.Availability.Providers
{
    /// <summary>
    /// Provider que devuelve reglas relacionadas con visitas grupales.
    /// Agrupa reglas que comparten datos y dependencias.
    /// </summary>
    public class GroupVisitProvider : Application.Availability.IRuleProvider
    {
        public bool CanHandle(ActividadMuseo candidate)
        {
            // Aplicable a cualquier visita grupal
            return candidate is Domain.VisitasGrupales.Entities.VisitaGrupalGuiada
                || candidate is Domain.VisitasGrupales.Entities.VisitaGrupalAutoguiada;
        }

        public IEnumerable<Application.Availability.IAvailabilityRule> CreateRules(ActividadMuseo candidate)
        {
            // Devolver instancias de reglas relacionadas. Si las reglas requieren repositorios
            // inyectados, este provider debe recibirlos por constructor y crear las reglas aquí.
            yield return new NoConcurrentGuidedWithAutoguidedRule();
            yield return new NoGroupIfHallEventOrEducationalRule();
            yield return new NoGroupIfExhibitInMountingOrDisassemblyRule();
            yield return new CalendarAndRoomBlockRule();
        }
    }
}
