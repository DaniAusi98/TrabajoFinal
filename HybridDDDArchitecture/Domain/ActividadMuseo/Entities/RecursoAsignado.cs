using Core.Domain.Entities;

using Domain.RecursoMuseo.Entities;
using Domain.Validators.DisponibilidadMuseo;

namespace Domain.ActividadMuseo.Entities
{
    public class RecursoAsignado:DomainEntity<int,RecursoAsignadoValidator>
    {
        public int ActividadId { get; private set; }
        public Actividad Actividad { get; private set; }

        public int RecursoId { get; private set; }
        public Recurso Recurso { get; private set; }

        public int CantidadAsignada { get; private set; }

        public RecursoAsignado(int recursoId, int cantidadAsignada)
        {
            RecursoId = recursoId;
            CantidadAsignada = cantidadAsignada;
        }

        public void ActualizarCantidad(int nuevaCantidad)
        {
            CantidadAsignada = nuevaCantidad;
        }

        private RecursoAsignado() { }
    }
}

