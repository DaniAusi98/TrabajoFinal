using Domain.ActividadMuseo.Entities;
namespace Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento
{
    public interface IEspecificacionConflictoActividad
    {
        bool AplicaA(Entities.ActividadMuseo existente);
        public bool HayConflicto(Entities.ActividadMuseo nueva, Entities.ActividadMuseo existente);
    }
}
