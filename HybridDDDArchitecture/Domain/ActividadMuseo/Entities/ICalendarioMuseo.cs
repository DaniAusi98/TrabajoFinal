using Domain.Common.ValueObjets;

namespace Domain.ActividadMuseo.Entities
{
    public interface ICalendarioMuseo
    {
        bool EsDiaOperativo(DateTime fecha);

        bool EstaAbierto(DateTime inicio, DateTime fin);

        IEnumerable<TimeSlot> ObtenerFranjasOperativas(DateOnly fecha);
    }
}