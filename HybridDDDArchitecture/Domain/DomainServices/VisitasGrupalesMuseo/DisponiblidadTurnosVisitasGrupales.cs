using Domain.Entities.DisponibilidadMuseo;
using Domain.Entities.VisitasGrupalesMuseo;
using Domain.Entities.VisitasGrupalesMuseo.Guia;
using Domain.ValueObjets;
using Domain.ValueObjets.VisitaGrupalMuseo;

using static Domain.Enums.VisitasGrupalesEnums.Enums;

namespace Domain.DomainServices.VisitasGrupalesMuseo
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
                                v.ActividadMuseo.TimeSlots.Any(ts =>
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
                            var personas = reservasGuiadas[0].ActividadMuseo.CantidadPersonas;

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

