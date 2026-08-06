/*using System.Collections.Generic;
using Application.Availability.Rules;
using Domain.ActividadMuseo.Entities;

namespace Application.Availability.Providers
{
    public class GroupRuleProvider : IRuleProvider
    {
        public bool CanHandle(Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            // Apply when candidate is either guided or autoguided group visit
            return candidate is Domain.VisitasGrupales.Entities.VisitaGrupalGuiada
                || candidate is Domain.VisitasGrupales.Entities.VisitaGrupalAutoguiada;
        }

        public IEnumerable<IAvailabilityRule> CreateRules(Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            yield return new NoConcurrentGuidedIfGroupRule();
        }
    }
}*/
