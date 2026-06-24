using Domain.Entities.DisponibilidadMuseo;
namespace Domain.DomainServices.VisitasGrupalesMuseo
{
    public class CalendarioMuseo : ICalendarioMuseo
    {
        public bool DiaOperativoMuseo(
            DateTime fecha,
            DateTime fechaHasta,
            IReadOnlyCollection<DiaCierreMuseo> diasCierre)
        {
            if (fecha.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                return false;

            return !diasCierre.Any(d => d.SolapaConFechas(fecha,fechaHasta));
        }
    }    /////TENGO QUE MODIFICAR LOS CONFIG DE AUSENCIAS HORARIOS GUIAS QUE VA A SER UN VO Y NO UNA CLASE TENGO Y SEGUIR MODIFICANDO LO DE LOS TIMESLOT 
}
