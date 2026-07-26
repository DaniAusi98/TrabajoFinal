using Domain.ActividadMuseo.Entities;
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

                    // int? personasAutoguiadas = reservasAutoguiadas.Sum(r => r.CantidadPersonas);

                    var reservasGuiadas = visitasguiadas
                            .Where(v =>
                                v.TimeSlots.Any(ts =>
                                    ts.Inicio == horario.Inicio &&
                                    ts.Fin == horario.Fin))
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
                        /*else if (personasAutoguiadas > 25)
                        {
                            capacidadDisponible = 0;
                            estado = EstadoTurno.NoDisponible;
                        }*/
                        else
                        {
                            capacidadDisponible = 25;
                            estado = EstadoTurno.Disponible;
                        }

                        
                    }
                    else
                    {

                        if (reservasGuiadas.Count == 0 /*&& personasAutoguiadas == 0*/)
                        {
                            capacidadDisponible = 50;
                            estado = EstadoTurno.Disponible;
                        }
                        /*else if (reservasGuiadas.Count == 0 && personasAutoguiadas > 0)
                        {
                            if (personasAutoguiadas <= 20)
                            {
                                capacidadDisponible = 25;
                                estado = EstadoTurno.Disponible;
                            }
                            else
                            {
                                capacidadDisponible = 0;
                                estado = EstadoTurno.NoDisponible;
                            }

                           
                        }*/
                        else if (reservasGuiadas.Count == 1)
                        {
                            var personas = reservasGuiadas[0].CantidadPersonas;

                           /* if (personasAutoguiadas > 0)
                            {
                                capacidadDisponible = 0;
                                estado = EstadoTurno.NoDisponible;
                            }*/
                             if (personas <= 25)
                            {
                                capacidadDisponible = 25;
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

                    var turnoDisponible = new TurnoDisponible(
                        horario,
                        capacidadMaxima,
                        capacidadDisponible,
                        estado
                        
                    );

                    resultado.Add(turnoDisponible);
                }



            }
            return resultado;

        }

    }
}

