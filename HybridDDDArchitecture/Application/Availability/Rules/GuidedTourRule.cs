using System.Linq;
using System.Threading.Tasks;
using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities;
using Domain.ActividadMuseo.Entities;

namespace Application.Availability.Rules
{
    // Rule that delegates availability calculation to the existing domain service for guided visits
    public class GuidedTourRule : Application.Availability.IAvailabilityRule
    {
        private readonly IServicioDisponibilidadTurnosVisitasGuiadas _servicioDisponibilidad;
        private readonly IRepositorioGuia _repositorioGuia;
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada;
        private readonly Application.Repositories.IRepositorioDiaCierreMuseo _repositorioDiasCierre;

        public GuidedTourRule(
            IServicioDisponibilidadTurnosVisitasGuiadas servicioDisponibilidad,
            IRepositorioGuia repositorioGuia,
            IRepositorioVisitaGuiada repositorioVisitaGuiada,
            Application.Repositories.IRepositorioDiaCierreMuseo repositorioDiasCierre)
        {
            _servicioDisponibilidad = servicioDisponibilidad;
            _repositorioGuia = repositorioGuia;
            _repositorioVisitaGuiada = repositorioVisitaGuiada;
            _repositorioDiasCierre = repositorioDiasCierre;
        }

        public async Task<Application.Availability.AvailabilityResult> CheckAsync(Application.Availability.AvailabilityContext ctx, Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            if (candidate is not VisitaGrupalGuiada guiaCandidate)
                return Application.Availability.AvailabilityResult.Ok();

            // Build date range from candidate timeslots
            var start = guiaCandidate.TimeSlots.Min(ts => ts.Inicio);
            var end = guiaCandidate.TimeSlots.Max(ts => ts.Fin);

            var guias = await _repositorioGuia.ObtenerGuiasConDisponibilidadAsync();
            var visitas = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(start.Date, end.Date);
            var diasCierre = await _repositorioDiasCierre.FindAllAsync();

            var turnos = await _servicioDisponibilidad.CalcularDisponibilidad(start.Date, end.Date, guias, visitas, diasCierre);

            // If there is any turno with capacidad > 0 and estado disponible -> OK
            var anyAvailable = turnos.Any(t => t.EstadoTurno == Domain.VisitasGrupales.Enums.Enums.EstadoTurno.Disponible && t.CapacidadDisponible > 0);

            if (anyAvailable)
                return Application.Availability.AvailabilityResult.Ok();

            return Application.Availability.AvailabilityResult.Fail("No hay turno disponible para la visita guiada en el rango solicitado");
        }
    }
}
