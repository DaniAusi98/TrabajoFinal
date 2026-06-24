using Domain.Constants.DisponibilidadMuseo;
using Domain.DomainServices.DisponibilidadMuseo.EspecificacionesSolapamiento;
using Domain.Entities.DisponibilidadMuseo;

using static Domain.Enums.DisponibilidadMuseo.Enums;

namespace Domain.DomainServices.DisponibilidadMuseo.EspecificacionesSolapamiento.Espec.VisitaGrupal
{
    public class ConflictoVisitaGrupalConEventolSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(ActividadMuseo existente)
         => existente.TipoActividad != TipoActividad.MuestraExposicionTemporal;

        public bool HayConflicto(ActividadMuseo nueva, ActividadMuseo existente)
        {
            if (nueva.TipoActividad != TipoActividad.VisitaGrupalGuiada &&
                nueva.TipoActividad != TipoActividad.VisitaGrupalAutoguiada)
                return false;

            return existente.Salas.Any(s =>
                s.CodigoSala == DomainConstants.CODIGO_HALLINGRESO_PRINCIPAL);
        }
    }
}