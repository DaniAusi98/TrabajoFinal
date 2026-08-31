using Core.Domain.Entities;
using Domain.Common.Exceptions;
using static Domain.ActividadMuseo.Enums.Enums;
using Domain.RecursoMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Domain.ActividadMuseo.Entities
{
    public class ActividadMuseo : DomainEntity<string>
    {
        public CategoriaActividad CategoriaActividad { get; private set; }
        public TipoActividad TipoActividad { get; private set; }
        public TimeSlot Horario { get;  private set; } 
        public EstadoActividad Estado { get; private set; }
        public int? CantidadPersonas { get; private set; }

        public List<Sala> Salas = [];

        public  List<RecursoAsignado> Recursos = [];

        public RecurrenceRule? Recurrence { get; private set; }

        private readonly List<ActividadException> _exceptions = [];

        public IReadOnlyCollection<ActividadException> Exceptions
            => _exceptions.AsReadOnly();

        

        public ActividadMuseo(
            CategoriaActividad categoria,
            TipoActividad tipo,
            int? cantidadAsistentes,
            TimeSlot horario,
            IEnumerable<Sala> salas = null,
            IEnumerable<RecursoAsignado> recursos = null)
        {

            if (cantidadAsistentes is not null && cantidadAsistentes <= 0)
                throw new DomainException("La cantidad de personas debe ser mayor a cero.");

            if (horario is null)
                throw new DomainException("El horario no puede ser nulo.");

            if (salas is not null)
            {
                if (salas.Any(s => s is null))
                    throw new DomainException("No puede haber salas nulas.");

                if (salas.GroupBy(s => s.Id).Any(g => g.Count() > 1))
                    throw new DomainException("No puede haber salas repetidas.");
            }

            if (recursos is not null)
            {
                if (recursos.Any(r => r is null))
                    throw new DomainException("No puede haber recursos nulos.");

                if (recursos.GroupBy(r => r.Id).Any(g => g.Count() > 1))
                    throw new DomainException("No puede haber recursos repetidos.");
            }

            Id = Guid.NewGuid().ToString();

            // Recién acá modificás el estado
            CategoriaActividad = categoria;

            TipoActividad = tipo;

            CantidadPersonas = cantidadAsistentes;
            Estado = EstadoActividad.Activa;

            Horario = horario;

            if (salas is not null)
                Salas.AddRange(salas);

            if (recursos is not null)
                Recursos.AddRange(recursos);
        }

        // EF Core
        protected ActividadMuseo() { }
        public void CambiarCategoria(CategoriaActividad nuevaCategoria)
        {
            ArgumentNullException.ThrowIfNull(nuevaCategoria);
            CategoriaActividad = nuevaCategoria;
        }
        public void CambiarTipoActividad(TipoActividad nuevoTipo)
        {
            ArgumentNullException.ThrowIfNull(nuevoTipo);
            TipoActividad = nuevoTipo;
        }

        public void AgregarSala(Sala sala)
        {
            ArgumentNullException.ThrowIfNull(sala);
            if (Salas.Any(s => s.Id == sala.Id))
                return;
            Salas.Add(sala);
        }

        public void AgregarRecurso(RecursoAsignado recurso)
        {
            ArgumentNullException.ThrowIfNull(recurso);
            if (Recursos.Any(r => r.Id == recurso.Id))
                return;
            Recursos.Add(recurso);
        }

        public void CancelarActividad()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException("La actividad ya está cancelada.");

            Estado = EstadoActividad.Cancelada;
        }

        public void MarcarComoActividadReprogramada()
        {
            if (Estado == EstadoActividad.Cancelada || Estado == EstadoActividad.Reprogramada)
                throw new DomainException("No se puede reprogramar una actividad cancelada o ya reprogramada.");

            Estado = EstadoActividad.Reprogramada;
        }

        public void Activar()
        {
            if (Estado == EstadoActividad.Cancelada || Estado == EstadoActividad.Reprogramada)
                throw new DomainException("No se puede activar una actividad cancelada o reprogramada.");

            Estado = EstadoActividad.Activa;
        }

        public void CambiarCantidadAsistentes(int? nuevaCantidad)
        {
            if (nuevaCantidad is not null && nuevaCantidad <= 0)
                throw new DomainException("La cantidad de personas debe ser mayor a cero.");
            CantidadPersonas = nuevaCantidad;
        }

        public void AsignarSalas(IEnumerable<Sala> salas)
        {
            ArgumentNullException.ThrowIfNull(salas);

            if (salas.Any(s => s is null))
                throw new DomainException("No puede haber salas nulas.");

            if (salas.GroupBy(s => s.Id).Any(g => g.Count() > 1))
                throw new DomainException("No puede haber salas repetidas.");

            Salas.Clear();
            Salas.AddRange(salas);
        }

        public void AsignarRecursos(IEnumerable<RecursoAsignado> recursos)
        {
            ArgumentNullException.ThrowIfNull(recursos);

            if (recursos.Any(r => r is null))
                throw new DomainException("No puede haber recursos nulos.");

            if (recursos.GroupBy(r => r.Id).Any(g => g.Count() > 1))
                throw new DomainException("No puede haber recursos repetidos.");

            Recursos.Clear();
            Recursos.AddRange(recursos);
        }

        public void AsignarTimeSlots(TimeSlot horario)
        {
            ArgumentNullException.ThrowIfNull(horario);

            if (horario is null)
                throw new DomainException("El horario no puede ser nulo .");
         
            Horario = horario;
        }
        public void AgregarRecurrencia(RecurrenceRule recurrence)
        {
            if (recurrence == null)
                throw new DomainException(
                    "La recurrencia no puede ser nula.");

            if (Recurrence != null)
                throw new DomainException(
                    "La actividad ya tiene una recurrencia.");

            Recurrence = recurrence;
        }

        public void QuitarRecurrencia()
        {
            Recurrence = null;
            _exceptions.Clear();
        }

        public void CambiarRecurrencia(RecurrenceRule recurrence)
        {
            if (recurrence == null)
                throw new DomainException(
                    "La recurrencia no puede ser nula.");

            Recurrence = recurrence;
            _exceptions.Clear();
        }

        public void AgregarExcepcion(DateOnly date)
        {
            if (Recurrence == null)
                throw new DomainException(
                    "No se puede agregar una excepción a una actividad sin recurrencia.");

            if (date < Recurrence.StartDate)
                throw new DomainException(
                    "La fecha de la excepción no puede ser anterior al inicio de la recurrencia.");

            if (Recurrence.EndDate.HasValue &&
                date > Recurrence.EndDate.Value)
                throw new DomainException(
                    "La fecha de la excepción no puede ser posterior al fin de la recurrencia.");

            if (_exceptions.Any(x => x.Date == date))
                throw new DomainException(
                    "Ya existe una excepción para esa fecha.");

            _exceptions.Add(
                new ActividadException(Id, date)
            );
        }
    }
}
