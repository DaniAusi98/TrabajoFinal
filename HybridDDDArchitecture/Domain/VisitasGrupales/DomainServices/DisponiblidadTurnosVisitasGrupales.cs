using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.DomainServices
{
    public class ServicioDisponibilidadTurnosVisitasGuiadas : IServicioDisponibilidadTurnosVisitasGuiadas
    {
        /// <summary>
        /// LIMITACIÓN DEL SISTEMA: Actualmente solo se soporta 1 visita guiada
        /// simultánea por turno.
        /// 
        /// JUSTIFICACIÓN: Permitir múltiples visitas simultáneas requeriría:
        /// - Gestión de disponibilidad de guías individuales
        /// - Validación de capacidad acumulada del museo
        /// - Coordinación de recursos compartidos
        /// 
        /// Si el museo requiere esta funcionalidad en el futuro, será necesario:
        /// - Crear agregado/entidad Guia
        /// - Implementar servicio de asignación de recursos
        /// - Refactorizar validaciones de capacidad
        /// </summary>
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
                // 1. Verificar si el día está disponible según configuración (día de la semana)
                if (!configuracion.EsDiaDisponible(fecha.DayOfWeek))
                {
                    continue; // Salta este día completo
                }

                // 2. Verificar si la fecha está bloqueada (validación de bloqueos)
                if (!configuracion.EstaDisponibleEnFecha(fecha))
                {
                    // La fecha está bloqueada, marcar todos los turnos como no disponibles
                    foreach (var turno in configuracion.Turnos)
                    {
                        var inicioTurno = fecha.Date.Add(turno.HoraInicio.ToTimeSpan());
                        var finTurno = fecha.Date.Add(turno.HoraFin.ToTimeSpan());
                        var horario = new TimeSlot(inicioTurno, finTurno);

                        resultado.Add(new TurnoDisponible(
                            horario,
                            0,
                            EstadoTurno.NoDisponible));
                    }
                    continue; // Salta al siguiente día
                }

                // 3. Procesar cada turno del día (solo si no está bloqueado)
                foreach (var turno in configuracion.Turnos)
                {
                    var inicioTurno = fecha.Date.Add(turno.HoraInicio.ToTimeSpan());
                    var finTurno = fecha.Date.Add(turno.HoraFin.ToTimeSpan());
                    var horario = new TimeSlot(inicioTurno, finTurno);

                    // 3.1. Verificar si el museo está abierto en ese horario
                    if (!calendario.EstaAbierto(horario.Inicio, horario.Fin))
                    {
                        resultado.Add(new TurnoDisponible(
                            horario,
                            0,
                            EstadoTurno.NoDisponible));

                        continue; // Siguiente turno
                    }

                    // 3.2. Calcular guías disponibles para este turno
                    int cantidadGuias = guias.Count(g => DisponibilidadGuiaFecha.GuiaPuedeCubrirTurno(g, horario));

                    // 3.3. Calcular capacidad máxima según guías disponibles
                    int capacidadMaxima = configuracion.CalcularCapacidadDisponible(cantidadGuias);

                    // 3.4. Verificar si ya hay una reserva en este turno
                    bool tieneReserva = visitasguiadas
                        .Any(v => v.TimeSlots.Any(ts => ts.SeSolapaCon(horario)));

                    // 3.5. Calcular estado del turno
                    var estado = CalculoEstadoTurno.Calcular(
                        tieneReserva,
                        cantidadGuias);

                    // 3.6. Agregar turno al resultado
                    var turnoDisponible = new TurnoDisponible(horario, capacidadMaxima, estado);
                    resultado.Add(turnoDisponible);
                }
            }

            return Task.FromResult(resultado);
        }
    }
}