namespace Domain.ActividadMuseo.Entities
{
    public interface ICalendarioMuseo
    {
       public bool DiaOperativoMuseo(DateTime fechaDesde, DateTime fechaHasta, IReadOnlyCollection<DiaCierreMuseo> diasCierre);
    }
}
