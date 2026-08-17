using System.Collections.Generic;
using Application.Availability.Rules;
using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.Availability.Providers
{
    /// <summary>
    /// Provider que devuelve reglas relacionadas con visitas grupales.
    /// Agrupa reglas que comparten datos y dependencias.
    /// </summary>
    public class GroupVisitProvider : IRuleProvider
    {
        private readonly NoConcurrentGuidedWithAutoguidedRule _noConcurrentRule;
        //private readonly NoGroupIfHallEventOrEducationalRule _hallEventRule;
       // private readonly NoGroupIfExhibitInMountingOrDisassemblyRule _exhibitRule;

        public GroupVisitProvider(
            NoConcurrentGuidedWithAutoguidedRule noConcurrentRule
           // NoGroupIfHallEventOrEducationalRule hallEventRule,
           // NoGroupIfExhibitInMountingOrDisassemblyRule exhibitRule
           )
        {
            _noConcurrentRule = noConcurrentRule;
           // _hallEventRule = hallEventRule;
           // _exhibitRule = exhibitRule;
        }

        public bool CanHandle(Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            // Aplicable a cualquier visita grupal
            return candidate is VisitaGrupalGuiada
                || candidate is Domain.VisitasGrupales.Entities.VisitaGrupalAutoguiada;
        }

        public IEnumerable<IAvailabilityRule> CreateRules(
            Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            yield return _noConcurrentRule;
           // yield return _hallEventRule;
           // yield return _exhibitRule;
        }
    }
}