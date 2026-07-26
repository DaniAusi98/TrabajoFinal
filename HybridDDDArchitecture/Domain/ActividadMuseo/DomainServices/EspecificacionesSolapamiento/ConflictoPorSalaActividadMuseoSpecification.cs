
using Domain.ActividadMuseo.Entities;
namespace Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento
{
    public class ConflictoPorSalaActividadMuseoSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(Entities.ActividadMuseo existente) => true;

        public  bool HayConflicto(Entities.ActividadMuseo nueva, Entities.ActividadMuseo existente)
        {
            return nueva.Salas.Any(ns => existente.Salas.Any(es => es.Id == ns.Id));
        }

        
    }
}
