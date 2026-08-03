using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Options;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.DomainServices
{
    public class ServicioDisponibilidadTurnosVisitasGuiadas : IServicioDisponibilidadTurnosVisitasGuiadas
    {
        private readonly IConfiguracionVisitasOptionsProvider _optionsProvider;
        private readonly ICalendarioMuseo _calendario;

        public ServicioDisponibilidadTurnosVisitasGuiadas(IConfiguracionVisitasOptionsProvider optionsProvider, ICalendarioMuseo calendario)
        {
            _optionsProvider = optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider));
            _calendario = calendario ?? throw new ArgumentNullException(nameof(calendario));
        }

        public async System.Threading.Tasks.Task<List<TurnoDisponible>> CalcularDisponibilidad(
            DateTime fechaDesde, DateTime fechaHasta,
            IReadOnlyCollection<Guia> guias,
            IReadOnlyCollection<VisitaGrupalGuiada> visitasguiadas
            )
        {
            var resultado = new List<TurnoDisponible>();

            var options = await _optionsProvider.GetOptionsAsync() ?? new TurnosVisitasOptions();

            for (DateTime fecha = fechaDesde; fecha <= fechaHasta; fecha = fecha.AddDays(1))
            {
                foreach (var (horaInicio, horaFin) in TurnosVisitasGuiadas.HorarioTurnos)
                {
                    var inicioTurno = fecha.Date.Add(horaInicio.ToTimeSpan());
                    var finTurno = fecha.Date.Add(horaFin.ToTimeSpan());
                    var horario = new TimeSlot(inicioTurno, finTurno);

                    // Si el museo está cerrado para ese turno, marcar como no disponible
                    if (!_calendario.EstaAbierto(horario.Inicio, horario.Fin))
                    {
                        resultado.Add(new TurnoDisponible(
                            horario,
                            0,
                            0,
                            EstadoTurno.NoDisponible));

                        continue;
                    }

                    var guiasDisponibles = guias
                        .Where(g => DisponibilidadGuiaFecha.GuiaPuedeCubrirTurno(g, horario))
                        .ToList();

                    int cantidadGuias = guiasDisponibles.Count;

                    int capacidadMaxima = CalcularCapacidadMaxima(cantidadGuias, options);

                    int capacidadDisponible = 0;
                    EstadoTurno estado;

                    var reservasGuiadas = visitasguiadas
                            .Where(v => v.TimeSlots.Any(ts => ts.SeSolapaCon(horario)))
                            .ToList();

                    if (cantidadGuias == 0)
                    {
                        capacidadDisponible = 0;
                        estado = EstadoTurno.NoDisponible;
                    }
                    else if (cantidadGuias == 1)
                    {
                        if (reservasGuiadas.Count > 0)
                        {
                            capacidadDisponible = 0;
                            estado = EstadoTurno.Completo;
                        }
                        else
                        {
                            capacidadDisponible = options.CapacidadPorGuia;
                            estado = EstadoTurno.Disponible;
                        }
                    }
                    else
                    {
                        if (reservasGuiadas.Count == 0)
                        {
                            capacidadDisponible = capacidadMaxima;
                            estado = EstadoTurno.Disponible;
                        }
                        else if (reservasGuiadas.Count == 1)
                        {
                            var personas = reservasGuiadas[0].CantidadPersonas ?? 0;
                            if (personas <= options.CapacidadPorGuia)
                            {
                                capacidadDisponible = options.CapacidadPorGuia;
                                estado = EstadoTurno.Disponible;
                            }
                            else
                            {
                                capacidadDisponible = 0;
                                estado = EstadoTurno.Completo;
                            }
                        }
                        else
                        {
                            capacidadDisponible = 0;
                            estado = EstadoTurno.Completo;
                        }
                    }

                    var turnoDisponible = new TurnoDisponible(horario, capacidadMaxima, capacidadDisponible, estado);
                    resultado.Add(turnoDisponible);
                }
            }

            return resultado;
        }

        private int CalcularCapacidadMaxima(int cantidadGuias, TurnosVisitasOptions options)
        {
            if (cantidadGuias >= options.MinGuiasParaCapacidadCompleta)
                return options.CapacidadMaximaPorTurno;

            if (cantidadGuias == 1)
                return options.CapacidadPorGuia;

            return 0;
        }
    }
}

/*using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.VisitasGrupales.Entities;

using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.DomainServices
{
    public class ServicioDisponibilidadTurnosVisitasGuiadas
    {
        public static List<TurnoDisponible> DisponiblidadTurnosVisitasGuiadas

            (DateTime fechaDesde, DateTime fechaHasta,
            IReadOnlyCollection<Guia> guias,
            IReadOnlyCollection<VisitaGrupalGuiada> visitasguiadas,
            IReadOnlyCollection<DiaCierreMuseo> diascerrado,
            ICalendarioMuseo calendarioMuseo)

        {
            var resultado = new List<TurnoDisponible>();

            for (DateTime fecha = fechaDesde; fecha <= fechaHasta; fecha = fecha.AddDays(1))
            {

                foreach (var (horaInicio, horaFin) in TurnosVisitasGuiadas.HorarioTurnos)

                {
                    var inicioTurno = fecha.Date.Add(horaInicio.ToTimeSpan());
                    var finTurno = fecha.Date.Add(horaFin.ToTimeSpan());
                    var horario = new TimeSlot(inicioTurno, finTurno);

                    



                    var guiasDisponibles = guias
                        .Where(g => DisponibilidadGuiaFecha.GuiaPuedeCubrirTurno(g, horario))
                        .ToList();

                    int cantidadGuias = guiasDisponibles.Count;
                    //int capacidadMaximaVisitas = 50;

                    int capacidadMaxima = cantidadGuias switch
                    {
                        >= 2 => 50,
                        1 => 25,
                        _ => 0
                    };

                    int capacidadDisponible = 0;

                    EstadoTurno estado;


                    /* var reservasAutoguiadas = visitasgrupalesAutoguiadas.Where(v =>

                         v.TimeSlots.Any(ts =>
                             ts.Fecha == horario.Fecha &&
                             ts.Inicio == horario.Inicio &&
                             ts.Fin == horario.Fin))
                         .ToList();*/

// int? personasAutoguiadas = reservasAutoguiadas.Sum(r => r.CantidadPersonas);*/
