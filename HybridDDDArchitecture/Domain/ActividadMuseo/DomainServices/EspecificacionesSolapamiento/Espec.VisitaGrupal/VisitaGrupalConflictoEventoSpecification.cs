using Domain.ActividadMuseo.Constants;
using Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento;

using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento.Espec.VisitaGrupal
{
    public class ConflictoVisitaGrupalConEventolSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(Entities.Actividad existente)
         => existente.TipoActividad != TipoActividad.MuestraExposicionTemporal;

        public bool HayConflicto(Entities.Actividad nueva, Entities.Actividad existente)
        {
            if (nueva.TipoActividad != TipoActividad.VisitaGrupalGuiada &&
                nueva.TipoActividad != TipoActividad.VisitaGrupalAutoguiada)
                return false;

            return existente.Salas.Any(s =>
                s.CodigoSala == DomainConstants.CODIGO_HALLINGRESO_PRINCIPAL);
        }
    }
}
