using Domain.Entities.VisitasGrupalesMuseo.Guia;
using Domain.ValueObjets;

namespace Domain.DomainServices.VisitasGrupalesMuseo
{
    public class DisponibilidadGuiaFecha
    {

        public static bool GuiaPuedeCubrirTurno(Guia guia, TimeSlot horario)
        {
            DayOfWeek diaRequerido = horario.Inicio.DayOfWeek;

            // 2. Convertir los DateTime a TimeOnly para comparar solo horas
            TimeOnly horaInicioRequerida = TimeOnly.FromDateTime(horario.Inicio);
            TimeOnly horaFinRequerida = TimeOnly.FromDateTime(horario.Fin);
            // 3. Buscar si tiene un horario que cubra el día y las horas exactas
            bool cumpleHorario = guia.HorariosGuia.Any(h =>
                h.DiasLaborales.Contains(diaRequerido) &&
                horaInicioRequerida >= h.HoraInicio &&
                horaFinRequerida <= h.HoraFin
            );
            if (!cumpleHorario)
                return false;
            bool tieneAusencia = guia.AusenciasProgramadas.Any(a =>
              horario.Inicio < a.FechaHasta && horario.Fin > a.FechaDesde
             );

            // Puede cubrir el turno solo si cumple el horario Y NO tiene una ausencia que lo afecte
            return !tieneAusencia;
        }
    }
}
// de guia que tengo tengo 
