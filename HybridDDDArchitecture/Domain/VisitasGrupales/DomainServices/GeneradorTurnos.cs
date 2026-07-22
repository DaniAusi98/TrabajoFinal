/*
using Domain.CommonDomain.ValueObjets;
using Domain.VisitasMuseo.DomainServices;
using Domain.VisitasMuseo.Entities.Guia;

namespace Domain.VisitasMuseo.DomainServices
{
    public class GeneradorDeTurnos : IGeneradorDeTurnos
    {
        public IEnumerable<Turno> GenerarParaFecha(
            DateOnly fecha,
            IEnumerable<Guia> guias
           )
        {
            var turnosDelDia = new List<Turno>();


            foreach (var (inicio, fin, franjaTurno) in TurnosVisitasGuiadas.HorarioTurnos) // mañana / tarde
            {
                var horario = new TimeSlot(
                    fecha,          // DateOnly
                    inicio,  // TimeOnly
                    fin      // TimeOnly
                );



                // Verificar solapamiento con turnos ya generados
                if (turnosDelDia.Any(t => t.Horario.SeSolapaCon(horario)))
                    continue;
                // busco guias en esa franja 

               


                if (guiasAsignados.Count == 0)
                    continue; // si no hay guías para esa franja, no creo turno


                var guiasDisponibles = new List<Guia>();

                foreach (var guia in guiasAsignados)
                {
                    bool tieneSolapamiento = false;

                    foreach (var ausencia in guia.Ausenciasprogramadas)
                    {
                        // si la ausencia no afecta la fecha → sigue
                        if (horario.Fecha < ausencia.FechaInicio || horario.Fecha > ausencia.FechaFin)
                            continue;


                        bool seSolapa =
                            horario.Inicio < ausencia.HoraFin &&
                            horario.Fin > ausencia.HoraInicio;

                        if (seSolapa)
                        {
                            tieneSolapamiento = true;
                            break;
                        }
                    }

                    if (!tieneSolapamiento)
                        guiasDisponibles.Add(guia);
                }
                if (guiasDisponibles.Count == 0)
                {
                    // saltás al próximo horario
                    continue;
                    // O devolvés error, depende del caso
                }

                var turno = new Turno(horario, guiasDisponibles);
                turnosDelDia.Add(turno);
                yield return turno;
            }
        }

        // 🔹 Método flexible para rango de fechas
        public IEnumerable<Turno> GenerarMes(
            DateOnly fechaDesde,
            DateOnly fechaHasta,
            IEnumerable<Guia> guias
            )
        {

            for (var dia = fechaDesde; dia <= fechaHasta; dia = dia.AddDays(1))
            {
                if (dia.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                    continue; // saltar fines de semana

                foreach (var turno in GenerarParaFecha(dia, guias))
                    yield return turno;
            }
        }


    }
}
*/
