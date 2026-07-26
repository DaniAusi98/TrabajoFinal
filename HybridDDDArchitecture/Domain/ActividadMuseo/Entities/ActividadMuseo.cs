using Core.Domain.Entities;
using Domain.Common.Exceptions;
using static Domain.ActividadMuseo.Enums.Enums;
using Domain.RecursoMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Domain.ActividadMuseo.Entities
{
    public class ActividadMuseo : DomainEntity<int>
    {
        public TipoActividad TipoActividad { get; private set; }
        public EstadoActividad Estado { get; private set; }
        public int? CantidadPersonas { get; private set; }

        public List<Sala> Salas = [];

        public  List<RecursoAsignado> Recursos = [];

        public List<TimeSlot> TimeSlots = [];

        public ActividadMuseo(
            TipoActividad tipo,
            int? cantidadAsistentes,
            IEnumerable<TimeSlot> timeSlots,
            IEnumerable<Sala> salas = null,
            IEnumerable<RecursoAsignado> recursos = null)
        {
            if (cantidadAsistentes is not null && cantidadAsistentes <= 0)
                throw new DomainException("La cantidad de personas debe ser mayor a cero.");

            ArgumentNullException.ThrowIfNull(timeSlots);

            if (!timeSlots.Any())
                throw new DomainException("Debe haber al menos un TimeSlot.");

            if (timeSlots.Any(t => t is null))
                throw new DomainException("No puede haber TimeSlots nulos.");

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


            // Recién acá modificás el estado

            TipoActividad = tipo;
            CantidadPersonas = cantidadAsistentes;
            Estado = EstadoActividad.Activa;

            TimeSlots.AddRange(timeSlots);

            if (salas is not null)
                Salas.AddRange(salas);

            if (recursos is not null)
                Recursos.AddRange(recursos);
        }

        // EF Core
        protected ActividadMuseo() { }

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

        public void AsignarTimeSlots(IEnumerable<TimeSlot> timeSlots)
        {
            ArgumentNullException.ThrowIfNull(timeSlots);

            if (!timeSlots.Any())
                throw new DomainException("Debe haber al menos un TimeSlot.");
            if (timeSlots.Any(t => t is null))
                throw new DomainException("No puede haber TimeSlots nulos.");

            TimeSlots.Clear();
            TimeSlots.AddRange(timeSlots);
        }
    }
}
