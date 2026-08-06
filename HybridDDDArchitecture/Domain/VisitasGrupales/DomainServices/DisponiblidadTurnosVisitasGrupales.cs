using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.VisitasGrupales.Entities;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.DomainServices
{
    public class ServicioDisponibilidadTurnosVisitasGuiadas : IServicioDisponibilidadTurnosVisitasGuiadas
    {
        public Task<List<TurnoDisponible>> CalcularDisponibilidad(
            DateTime fechaDesde, 
            DateTime fechaHasta,
            IReadOnlyCollection<Guia> guias,
            IReadOnlyCollection<VisitaGrupalGuiada> visitasguiadas,
            ConfiguracionVisitasGrupalesGuiadas configuracion,
            CalendarioMuseo calendario)
        {
            if (configuracion == null)
                throw new ArgumentNullException(nameof(configuracion));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            var resultado = new List<TurnoDisponible>();

            for (DateTime fecha = fechaDesde; fecha <= fechaHasta; fecha = fecha.AddDays(1))
            {
                if (!configuracion.EsDiaDisponible(fecha.DayOfWeek))
                {
                    continue;
                }

                foreach (var turno in configuracion.Turnos)
                {
                    var inicioTurno = fecha.Date.Add(turno.HoraInicio.ToTimeSpan());
                    var finTurno = fecha.Date.Add(turno.HoraFin.ToTimeSpan());
                    var horario = new TimeSlot(inicioTurno, finTurno);

                    if (!calendario.EstaAbierto(horario.Inicio, horario.Fin))
                    {
                        resultado.Add(new TurnoDisponible(
                            horario,
                            0,
                            EstadoTurno.NoDisponible));

                        continue;
                    }

                    int cantidadGuias = guias.Count(g =>DisponibilidadGuiaFecha.GuiaPuedeCubrirTurno(g, horario));

                    int capacidadMaxima = configuracion.CalcularCapacidadDisponible(cantidadGuias);


                    bool tieneReserva = visitasguiadas
                        .Any(v => v.TimeSlots.Any(ts => ts.SeSolapaCon(horario)));

                    var estado = CalculoEstadoTurno.Calcular(
                        tieneReserva,
                        cantidadGuias);

                    var turnoDisponible = new TurnoDisponible(horario, capacidadMaxima, estado);
                    resultado.Add(turnoDisponible);
                }
            }

            return Task.FromResult(resultado);
        }
    }
}

 