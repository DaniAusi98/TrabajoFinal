using Core.Domain.Entities;

using Domain.RecursoMuseo.Entities;
using Domain.Validators.DisponibilidadMuseo;

using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.ActividadMuseo.Entities
{
    public class SalaBloqueadaMuseo:DomainEntity<int,SalaBloqueadaValidator>
    {
        public DateTime FechaDesde { get; private set; }

        public DateTime FechaHasta { get; private set; }

        public List<Sala> SalasBloqueadas = [];

        public TipoBloqueoSala Motivo { get; private set; }

        public string Observaciones { get; private set; } = string.Empty;
        protected SalaBloqueadaMuseo() { }

        public SalaBloqueadaMuseo(DateTime fechaInicio, DateTime fechaFin, IEnumerable<Sala> salasBloqueadas, TipoBloqueoSala motivo)
        {
            if (fechaFin <= fechaInicio)
                throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");
            if (salasBloqueadas == null || !salasBloqueadas.Any())
                throw new ArgumentException("Debe especificar al menos una sala bloqueada.");
            FechaDesde = fechaInicio;
            FechaHasta = fechaFin;
            SalasBloqueadas.AddRange(salasBloqueadas);
            Motivo = motivo;
        }

        public void ActualizarFechas(DateTime nuevaFechaDesde, DateTime nuevaFechaHasta)
        {
            if (nuevaFechaHasta<= nuevaFechaDesde)
                throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");
            FechaDesde = nuevaFechaDesde;
            FechaHasta = nuevaFechaHasta;
        }
        public void ActualizarSalasBloqueadas(IEnumerable<Sala> nuevasSalasBloqueadas)
        {
            if (nuevasSalasBloqueadas == null || !nuevasSalasBloqueadas.Any())
                throw new ArgumentException("Debe especificar al menos una sala bloqueada.");
            SalasBloqueadas.Clear();
            SalasBloqueadas.AddRange(nuevasSalasBloqueadas);
        }

        public void ActualizarMotivo (TipoBloqueoSala motivo)
        {
            Motivo = motivo;
        }

        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            if (nuevasObservaciones.Length > 500)
                throw new ArgumentException("Las observaciones no pueden exceder los 500 caracteres.");
            Observaciones = nuevasObservaciones ?? string.Empty;
        }   
    }
}
