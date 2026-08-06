using Domain.ActividadMuseo.Entities;
using Domain.ActividadMuseo.ValueObjets;

namespace Domain.ActividadMuseo.Others.Factories
{
    public static class CalendarioMuseoFactory
    {
        public static CalendarioMuseo CrearCalendarioPorDefecto()
        {
            var horario = new Horario(
               inicio: new TimeOnly(9, 0),
                fin: new TimeOnly(20, 0)
            );

            var diasLaborales = new DiasLaboralesMuseo(
                new[]
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    
                });

            var diasCierre = new List<DiaCierreMuseo>();

            return new CalendarioMuseo(
                horario,
                diasLaborales,
                diasCierre
            );
        }
    }
}