using Domain.Entities.DisponibilidadMuseo;

namespace Domain.DomainServices.DisponibilidadMuseo
{
    public interface IValidadorDisponibilidad
    {
        bool EstaDisponible(ActividadMuseo nueva, IReadOnlyCollection<ActividadMuseo> solapadas);
    }
}
