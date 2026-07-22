using Core.Domain.Entities;

using Domain.RecursoMuseo.Entities;
using Domain.Common.Exceptions;
using Domain.Validators.DisponibilidadMuseo;
using Domain.RecursoMuseo.Enums;

using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.ActividadMuseo.Entities
{
    public class DiaCierreMuseo : DomainEntity<int, DiaCierreMuseoValidator>
    {
        public DateTime Fecha { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public MotivoCierreMuseo Motivo { get; private set; }
        public string Observaciones { get; private set; }
        protected DiaCierreMuseo() { }

        public DiaCierreMuseo(DateTime fecha, DateTime fechaHasta, MotivoCierreMuseo motivo, string observaciones)
        {
            if (fechaHasta <= fecha)
                throw new DomainException("La fecha de fin no puede ser anterior a la fecha de inicio.");

            Fecha = fecha;
            FechaHasta = fechaHasta;
            Motivo = motivo;
            Observaciones = observaciones;
        }

        public void ActualizarFechas(DateTime nuevaFechaDesde, DateTime nuevaFechaHasta)
        {
            if ( nuevaFechaHasta <= nuevaFechaDesde)
                throw new DomainException("La fecha de fin no puede ser anterior a la fecha de inicio.");
            Fecha = nuevaFechaDesde;
            FechaHasta = nuevaFechaHasta;
        }

        public void ActualizarMotivo(MotivoCierreMuseo nuevoMotivo)
        {
                Motivo = nuevoMotivo;
        }

        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            if (nuevasObservaciones != null && nuevasObservaciones.Length > 500)
                throw new DomainException("Las observaciones no pueden exceder los 500 caracteres.");
            Observaciones = nuevasObservaciones;
        }

        public bool SolapaConFechas(DateTime inicio ,DateTime fin)
        {
            return inicio < FechaHasta && fin > Fecha;
            ;

        }

    }
}
