
using Domain.ActividadMuseo.Entities;
namespace Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento
{
    public class ConflictoPorSalaActividadMuseoSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(Entities.Actividad existente) => true;

        public  bool HayConflicto(Entities.Actividad nueva, Entities.Actividad existente)
        {
            return nueva.Salas.Any(ns => existente.Salas.Any(es => es.Id == ns.Id));
        }

        
    }
}
