using Core.Domain.Entities;
using Domain.Common.Exceptions;
using static Domain.ActividadMuseo.Enums.Enums;
using Domain.Validators.DisponibilidadMuseo;
using Domain.RecursoMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Domain.ActividadMuseo.Entities
{
    public class Actividad : DomainEntity<int,ActividadMuseoValidator>
    {
        public TipoActividad TipoActividad { get; private set; }
        public EstadoActividad Estado { get; private set; }
        public int? CantidadPersonas { get; private set; }

        public List<Sala> Salas = [];

        public  List<RecursoAsignado> Recursos = [];

        public List<TimeSlot> TimeSlots = [];

        public Actividad(
            TipoActividad tipo,
            int? cantidadAsistentes,
            IEnumerable<TimeSlot> timeSlots,
            IEnumerable<Sala> salas = null,
            IEnumerable<RecursoAsignado> recursos = null)
        {
            TipoActividad = tipo;
            CantidadPersonas = cantidadAsistentes;
            Estado = EstadoActividad.Activa;

            ArgumentNullException.ThrowIfNull(timeSlots);

            if (!timeSlots.Any())
                throw new DomainException("Debe haber al menos un TimeSlot.");
            TimeSlots.AddRange(timeSlots);

            if (salas is not null)
            {
                var nuevasSalas = salas
                    .Where(s => !Salas.Any(x => x.Id == s.Id));

                Salas.AddRange(nuevasSalas);
            }

            if (recursos is not null)
                Recursos.AddRange(recursos);
        }

        // EF Core
        protected Actividad() { }

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
            
            Salas.Clear();
            Salas.AddRange(salas);
        }

        public void AsignarRecursos(IEnumerable<RecursoAsignado> recursos)
        {
            ArgumentNullException.ThrowIfNull(recursos);
            Recursos.Clear();
            Recursos.AddRange(recursos);
        }

        public void AsignarTimeSlots(IEnumerable<TimeSlot> timeSlots)
        {
            ArgumentNullException.ThrowIfNull(timeSlots);

            if (!timeSlots.Any())
                throw new DomainException("Debe haber al menos un TimeSlot.");

            TimeSlots.Clear();
            TimeSlots.AddRange(timeSlots);
        }
    }
}
