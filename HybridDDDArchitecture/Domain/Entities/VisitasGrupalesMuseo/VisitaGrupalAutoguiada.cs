using Core.Domain.Entities;

using Domain.Entities.DisponibilidadMuseo;
using Domain.Exceptions;
using Domain.ValueObjets;

using static Domain.Enums.DisponibilidadMuseo.Enums;
using static Domain.Enums.VisitasGrupalesEnums.Enums;

namespace Domain.Entities.VisitasGrupalesMuseo
{
    public class VisitaGrupalAutoguiada:DomainEntity
    {
        public int UsuarioVisitanteId { get; private set; }
        public string Institucion { get; private set; }
        public Email EmailInstitucion { get; private set; }
        public string ProvinciaInstitucion { get; private set; }
        public string DepartamentoInstitucion { get; private set; }
        public string CiudadInstitucion { get; private set; }
        public string DiversidadFuncional { get; private set; }
        public string Observaciones { get; private set; }
        public EstadoConfirmacionVisita EstadoConfirmacion { get; private set; }

        public ActividadMuseo ReservaAgendaMuseo { get; private set; }


        protected VisitaGrupalAutoguiada()
        {
                
        }
        public VisitaGrupalAutoguiada(
            int usuarioVisitanteId,
            int cantidadPersonas,
            string institucion,
            Email emailInstitucion,
            string provinciaInstitucion,
            string departamamentoInstitucion,
            string ciudadInstitucion,
            string descripcionDiversidad,
            string observaciones,
            IEnumerable<TimeSlot> timeSlots
            


            )
        {
            UsuarioVisitanteId = usuarioVisitanteId;
            Institucion = institucion;
            EmailInstitucion = emailInstitucion;
            ProvinciaInstitucion = provinciaInstitucion;
            DepartamentoInstitucion = departamamentoInstitucion;
            CiudadInstitucion = ciudadInstitucion;
            DiversidadFuncional = descripcionDiversidad ?? string.Empty;
            Observaciones = observaciones ?? string.Empty;
            EstadoConfirmacion = EstadoConfirmacionVisita.PendienteConfirmar;
            ReservaAgendaMuseo = new ActividadMuseo(
            tipo: TipoActividad.VisitaGrupalAutoguiada,
            cantidadAsistentes: cantidadPersonas,
            timeSlots: timeSlots);




        }

        public void ActualizarInstitucion(string nuevaInstitucion)
        {
            Institucion = nuevaInstitucion ?? throw new ArgumentNullException(nameof(nuevaInstitucion), "La institución no puede ser nula.");
        }

        public void ActualizarEmailInstitucion(Email nuevoEmail)
        {
            EmailInstitucion = nuevoEmail;
        }

 
        public void ActualizarProvincia(string nuevaProvincia)
        {
            ProvinciaInstitucion = nuevaProvincia ?? throw new ArgumentNullException(nameof(nuevaProvincia), "La provincia de la institución no puede ser nula.");
        }

        public void ActualizarDepartamento(string nuevoDepartamento)
        {
            DepartamentoInstitucion = nuevoDepartamento ?? throw new ArgumentNullException(nameof(nuevoDepartamento), "El departamento de la institución no puede ser nulo.");
        }
        public void ActualizarCiudad(string nuevaCiudad)
        {
            CiudadInstitucion = nuevaCiudad ?? throw new ArgumentNullException(nameof(nuevaCiudad), "La ciudad de la institución no puede ser nula.");
        }


        public void ActualizarDiversidadFuncional(string diversidad)
        {
            DiversidadFuncional = diversidad ?? string.Empty;

        }
        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            Observaciones = nuevasObservaciones ?? string.Empty;
        }
        public void Confirmar()
        {
            if (ReservaAgendaMuseo.Estado == EstadoActividad.Cancelada)
                throw new DomainException("No se puede confirmar una visita cancelada.");

            if (EstadoConfirmacion != EstadoConfirmacionVisita.PendienteConfirmar)
                throw new DomainException("Solo se pueden confirmar visitas pendientes de confirmación.");

            EstadoConfirmacion = EstadoConfirmacionVisita.Confirmada;
        }




        public void Rechazar()
        {
            if (ReservaAgendaMuseo.Estado == EstadoActividad.Cancelada)
                throw new DomainException("La visita ya ha sido cancelada.");

            ReservaAgendaMuseo.CancelarActividad();
        }
    }
}
