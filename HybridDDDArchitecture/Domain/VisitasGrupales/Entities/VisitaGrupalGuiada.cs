using Core.Domain.Entities;
using Domain.Common.Exceptions;

using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;
using Domain.Validators.VisitasGrupalesValidators;
using Domain.Common.ValueObjets;


namespace Domain.VisitasGrupales.Entities
{
   public class VisitaGrupalGuiada: DomainEntity<int, VisitaGuiadaValidator>
    {
        public string UsuarioVisitanteId { get; private set; } //Id del usuario visitante que reserva la visita grupal guiada
        public string Institucion { get; private set; }
        public NivelEducativo? NivelEducativo { get; private set; }//Enum
        public int? AnioGrado { get; private set; }
        public Email EmailInstitucion { get; private set; } //Value Objets
        public Telefono TelefonoInstitucion { get; private set; }// ValueObjets
        public string ProvinciaInstitucion { get; private set; }
        public string DepartamentoInstitucion { get; private set; }
        public string LocalidadInstitucion { get; private set; }
        public string DiversidadFuncionalDescripcion { get; private set; } = string.Empty;
        public string MotivoRelacionVisita { get; private set; } = string.Empty;
        public string Observaciones { get; private set; } = string.Empty;
        public DateTime FechaCreacion { get; private set; } = DateTime.UtcNow;
        public List<TematicaVisita> Tematicas { get; private set; } = [];            // clase TematicaVisita con Id y Nombre
        public EstadoConfirmacionVisita EstadoConfirmacion { get; private set; }
        public int ActividadMuseoId { get; private set; }
        public Domain.ActividadMuseo.Entities.Actividad ActividadMuseo { get; private set; }   // composicion con la clase ActividadMuseo, que tiene el tipo de actividad, cantidad de personas, horarios, etc.


        protected VisitaGrupalGuiada()
        {
        }

        public VisitaGrupalGuiada(
            string usuarioVisitanteId,
            NivelEducativo? nivelEducativo,
            int? anioGrado,
            int cantidadPersonas,
            string institucion,
            Email emailInstitucion,
            Telefono telefonoInstitucion,
            string provinciaInstitucion,
            string departamamentoInstitucion,
            string ciudadInstitucion,
            string descripcionDiversidad,
            string motivoVisita,
            string observaciones,
            IEnumerable<TimeSlot> timeSlots,
            IEnumerable<TematicaVisita> tematicas
             

            )
        {

            UsuarioVisitanteId = usuarioVisitanteId;
            NivelEducativo = nivelEducativo;
            AnioGrado = anioGrado;
            Institucion = institucion;
            EmailInstitucion = emailInstitucion;
            TelefonoInstitucion = telefonoInstitucion;
            ProvinciaInstitucion = provinciaInstitucion;
            DepartamentoInstitucion = departamamentoInstitucion;
            LocalidadInstitucion = ciudadInstitucion;
            AsignarTematicas(tematicas);

            DiversidadFuncionalDescripcion = string.IsNullOrWhiteSpace(descripcionDiversidad)
                ? string.Empty
                : descripcionDiversidad.Trim();

            MotivoRelacionVisita = string.IsNullOrWhiteSpace(motivoVisita)
                ? string.Empty
                : motivoVisita.Trim();

            Observaciones = string.IsNullOrWhiteSpace(observaciones)
                ? string.Empty
                : observaciones.Trim();
            EstadoConfirmacion = EstadoConfirmacionVisita.PendienteConfirmar;

            ActividadMuseo = new Domain.ActividadMuseo.Entities.Actividad(
            tipo: TipoActividad.VisitaGrupalGuiada,
            cantidadAsistentes: cantidadPersonas,
            timeSlots: timeSlots
        );



        }

        public void ActualizarInstitucion(string nuevaInstitucion)
        {
            Institucion = nuevaInstitucion ?? throw new ArgumentNullException(nameof(nuevaInstitucion), "La institución no puede ser nula.");
        }

        public void ActualizarEmailInstitucion(Email nuevoEmail)
        {
            EmailInstitucion = nuevoEmail;
        }

        public void ActualizarTelefonoInstitucion(Telefono nuevoTelefono)
        {
            TelefonoInstitucion = nuevoTelefono;
        }

        public void ActualizarProvincia(string nuevaProvincia)
        {
            ProvinciaInstitucion = nuevaProvincia ?? throw new ArgumentNullException(nameof(nuevaProvincia), "La provincia de la institución no puede ser nula.");
        }

        public void ActualizarDepartamento(string nuevoDepartamento)
        {
            DepartamentoInstitucion = nuevoDepartamento ?? throw new ArgumentNullException(nameof(nuevoDepartamento), "El departamento de la institución no puede ser nulo.");
        }
        public void ActualizarLocalidad(string nuevaLocalidad)
        {
            LocalidadInstitucion = nuevaLocalidad ?? throw new ArgumentNullException(nameof(nuevaLocalidad), "La localidad de la institución no puede ser nula.");
        }

        public void ActualizarNivelEducativo(int? anioGrado, NivelEducativo? nivelEducativo)
        {
            if (nivelEducativo is not null && anioGrado is null)
                throw new DomainException("Debe indicar el año/grado cuando se informa nivel educativo.");

            NivelEducativo = nivelEducativo;
            AnioGrado = anioGrado;
        }
        public void ActualizarDiversidadFuncional(string diversidad)
        {
            DiversidadFuncionalDescripcion = diversidad ?? string.Empty;

        }


        public void ActualizarMotivoRelacion(string motivorelacion)
        {
            MotivoRelacionVisita = motivorelacion ?? string.Empty;
        }

        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            Observaciones = nuevasObservaciones ?? string.Empty;
        }
        public void Confirmar()
        {
            if (ActividadMuseo.Estado == EstadoActividad.Cancelada)
                throw new DomainException("No se puede confirmar una visita cancelada.");

            if (EstadoConfirmacion != EstadoConfirmacionVisita.PendienteConfirmar)
                throw new DomainException("Solo se pueden confirmar visitas pendientes de confirmación.");

            EstadoConfirmacion = EstadoConfirmacionVisita.Confirmada;
        }




        public void CancelarVisitaGuiada()
        {
            if (ActividadMuseo.Estado == EstadoActividad.Cancelada)
                throw new DomainException("La visita ya ha sido cancelada.");

            ActividadMuseo.CancelarActividad();
        }
        public void AgregarTematica(TematicaVisita tematica)
        {
            ArgumentNullException.ThrowIfNull(tematica);

            if (Tematicas.Any(t => t.Id == tematica.Id))
                throw new DomainException("La temática ya fue agregada a la visita.");

            Tematicas.Add(tematica);
        }

        public void AsignarTematicas(IEnumerable<TematicaVisita> tematicas)
        {
            ArgumentNullException.ThrowIfNull(tematicas);

            var lista = tematicas.ToList();

            if (lista.Count == 0)
                throw new DomainException("Debe seleccionar al menos una temática.");

            if (lista.Select(t => t.Id).Distinct().Count() != lista.Count)
                throw new DomainException("No se pueden repetir temáticas.");

            Tematicas = lista;
        }
        public void CambiarEstadoAReprogramada()
        {
            if (ActividadMuseo.Estado == EstadoActividad.Cancelada)
                throw new DomainException("No se puede reprogramar una visita cancelada.");
            ActividadMuseo.MarcarComoActividadReprogramada();
        }
    }
}
