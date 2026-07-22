using Domain.ActividadMuseo.Entities;
namespace Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento
{
    public interface IEspecificacionConflictoActividad
    {
        bool AplicaA(Entities.Actividad existente);
        public bool HayConflicto(Entities.Actividad nueva, Entities.Actividad existente);
    }
}
