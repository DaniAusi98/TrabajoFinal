namespace Domain.ActividadMuseo.DomainServices
{
    public interface IValidadorDisponibilidad
    {
        bool EstaDisponible(Entities.ActividadMuseo nueva, IReadOnlyCollection<Entities.ActividadMuseo> solapadas);
    }
}
