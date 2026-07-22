namespace Domain.ActividadMuseo.DomainServices
{
    public interface IValidadorDisponibilidad
    {
        bool EstaDisponible(Entities.Actividad nueva, IReadOnlyCollection<Entities.Actividad> solapadas);
    }
}
