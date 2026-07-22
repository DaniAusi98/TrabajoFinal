using Domain.ActividadMuseo.Constants;
using Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento;

using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento.Espec.VisitaGrupal
{
    public class ConflictoVisitaGrupalConEventolSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(Entities.ActividadMuseo existente)
         => existente.TipoActividad != TipoActividad.MuestraExposicionTemporal;

        public bool HayConflicto(Entities.ActividadMuseo nueva, Entities.ActividadMuseo existente)
        {
            if (nueva.TipoActividad != TipoActividad.VisitaGrupalGuiada &&
                nueva.TipoActividad != TipoActividad.VisitaGrupalAutoguiada)
                return false;

            return existente.Salas.Any(s =>
                s.CodigoSala == DomainConstants.CODIGO_HALLINGRESO_PRINCIPAL);
        }
    }
}
