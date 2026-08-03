using System.Collections.Generic;
using Application.Availability.Rules;
using Domain.ActividadMuseo.Entities;
using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.DomainServices;

namespace Application.Availability.Providers
{
    public class GuidedRuleProvider : IRuleProvider
    {
        private readonly IServicioDisponibilidadTurnosVisitasGuiadas _servicioDisponibilidad;
        private readonly IRepositorioGuia _repositorioGuia;
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada;
        private readonly Repositories.IRepositorioDiaCierreMuseo _repositorioDiasCierre;

        public GuidedRuleProvider(
            IServicioDisponibilidadTurnosVisitasGuiadas servicioDisponibilidad,
            IRepositorioGuia repositorioGuia,
            IRepositorioVisitaGuiada repositorioVisitaGuiada,
            Repositories.IRepositorioDiaCierreMuseo repositorioDiasCierre)
        {
            _servicioDisponibilidad = servicioDisponibilidad;
            _repositorioGuia = repositorioGuia;
            _repositorioVisitaGuiada = repositorioVisitaGuiada;
            _repositorioDiasCierre = repositorioDiasCierre;
        }

        public bool CanHandle(Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            return candidate is Domain.VisitasGrupales.Entities.VisitaGrupalGuiada;
        }

        public IEnumerable<IAvailabilityRule> CreateRules(Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            // Create rule instances using injected dependencies
            yield return new GuidedTourRule(_servicioDisponibilidad, _repositorioGuia, _repositorioVisitaGuiada, _repositorioDiasCierre);
        }
    }
}
