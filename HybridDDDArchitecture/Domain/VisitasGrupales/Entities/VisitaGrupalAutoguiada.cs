using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;

using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.Entities
{
    public class VisitaGrupalAutoguiada : ActividadMuseo.Entities.ActividadMuseo
    {
        public int UsuarioVisitanteId { get; private set; }

        public string Institucion { get; private set; }

        public Email EmailInstitucion { get; private set; }

        public string ProvinciaInstitucion { get; private set; }

        public string DepartamentoInstitucion { get; private set; }

        public string CiudadInstitucion { get; private set; }

        public string DiversidadFuncional { get; private set; } = string.Empty;

        public string Observaciones { get; private set; } = string.Empty;

        public EstadoConfirmacionVisita EstadoConfirmacion { get; private set; }


        protected VisitaGrupalAutoguiada()
        {
        }


        public VisitaGrupalAutoguiada(
            int usuarioVisitanteId,
            int cantidadPersonas,
            string institucion,
            Email emailInstitucion,
            string provinciaInstitucion,
            string departamentoInstitucion,
            string ciudadInstitucion,
            string descripcionDiversidad,
            string observaciones,
            IEnumerable<TimeSlot> timeSlots,
            IEnumerable<Sala> salas
        )
        : base(
            TipoActividad.VisitaGrupalAutoguiada,
            cantidadPersonas,
            timeSlots,
            salas)
        {

            if (usuarioVisitanteId <= 0)
                throw new DomainException(
                    "El id del usuario visitante debe ser válido.");


            if (string.IsNullOrWhiteSpace(institucion))
                throw new DomainException(
                    "La institución es obligatoria.");


            ArgumentNullException.ThrowIfNull(emailInstitucion);


            if (string.IsNullOrWhiteSpace(provinciaInstitucion))
                throw new DomainException(
                    "La provincia de la institución es obligatoria.");


            if (string.IsNullOrWhiteSpace(departamentoInstitucion))
                throw new DomainException(
                    "El departamento de la institución es obligatorio.");


            if (string.IsNullOrWhiteSpace(ciudadInstitucion))
                throw new DomainException(
                    "La ciudad de la institución es obligatoria.");


            UsuarioVisitanteId = usuarioVisitanteId;

            Institucion = institucion.Trim();

            EmailInstitucion = emailInstitucion;

            ProvinciaInstitucion = provinciaInstitucion.Trim();

            DepartamentoInstitucion = departamentoInstitucion.Trim();

            CiudadInstitucion = ciudadInstitucion.Trim();


            DiversidadFuncional =
                string.IsNullOrWhiteSpace(descripcionDiversidad)
                ? string.Empty
                : descripcionDiversidad.Trim();


            Observaciones =
                string.IsNullOrWhiteSpace(observaciones)
                ? string.Empty
                : observaciones.Trim();


            EstadoConfirmacion = EstadoConfirmacionVisita.PendienteConfirmar;
        }



        public void ActualizarInstitucion(string nuevaInstitucion)
        {
            if (string.IsNullOrWhiteSpace(nuevaInstitucion))
                throw new DomainException(
                    "La institución es obligatoria.");

            Institucion = nuevaInstitucion.Trim();
        }


        public void ActualizarEmailInstitucion(Email nuevoEmail)
        {
            ArgumentNullException.ThrowIfNull(nuevoEmail);

            EmailInstitucion = nuevoEmail;
        }


        public void ActualizarProvincia(string nuevaProvincia)
        {
            if (string.IsNullOrWhiteSpace(nuevaProvincia))
                throw new DomainException(
                    "La provincia de la institución es obligatoria.");

            ProvinciaInstitucion = nuevaProvincia.Trim();
        }


        public void ActualizarDepartamento(string nuevoDepartamento)
        {
            if (string.IsNullOrWhiteSpace(nuevoDepartamento))
                throw new DomainException(
                    "El departamento de la institución es obligatorio.");

            DepartamentoInstitucion = nuevoDepartamento.Trim();
        }


        public void ActualizarCiudad(string nuevaCiudad)
        {
            if (string.IsNullOrWhiteSpace(nuevaCiudad))
                throw new DomainException(
                    "La ciudad de la institución es obligatoria.");

            CiudadInstitucion = nuevaCiudad.Trim();
        }


        public void ActualizarDiversidadFuncional(string diversidad)
        {
            DiversidadFuncional =
                string.IsNullOrWhiteSpace(diversidad)
                ? string.Empty
                : diversidad.Trim();
        }


        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            Observaciones =
                string.IsNullOrWhiteSpace(nuevasObservaciones)
                ? string.Empty
                : nuevasObservaciones.Trim();
        }



        public void Confirmar()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "No se puede confirmar una visita cancelada.");

            if (EstadoConfirmacion != EstadoConfirmacionVisita.PendienteConfirmar)
                throw new DomainException(
                    "Solo se pueden confirmar visitas pendientes.");

            EstadoConfirmacion = EstadoConfirmacionVisita.Confirmada;
        }



        public void Rechazar()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "La visita ya está cancelada.");

            CancelarActividad();
        }
    }
}
