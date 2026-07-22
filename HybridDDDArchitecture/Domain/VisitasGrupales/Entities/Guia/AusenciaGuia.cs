using Core.Domain.Entities;

using Domain.Common.Exceptions;
using Domain.Validators.VisitasGrupalesValidators;

namespace Domain.VisitasGrupales.Entities.Guia
{
    public class AusenciaGuia : DomainEntity<int, AusenciaProgramadaValidator>
    {
        public int GuiaId { get; private set; }
        public Guia Guia { get; private set; }
        public DateTime FechaDesde { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public string Motivo { get; private set; }

        // Un único método de creación claro. Lo que le mandes, lo guarda.
        public static AusenciaGuia Crear(int guiaId, DateTime fechaDesde, DateTime fechaHasta, string motivo)
        {
            if (guiaId <= 0)
                throw new ArgumentException("El ID del guía debe ser válido.", nameof(guiaId));

            if (fechaHasta < fechaDesde)
                throw new DomainException("La fecha hasta no puede ser menor a la fecha desde.");

            return new AusenciaGuia(guiaId, fechaDesde, fechaHasta, motivo);
        }

        private AusenciaGuia(int guiaId, DateTime fechaDesde, DateTime fechaHasta, string motivo)
        {
            GuiaId = guiaId;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Motivo = motivo ?? throw new ArgumentNullException(nameof(motivo));
        }

        private AusenciaGuia() { } // EF Core

        public void ActualizarFechas(DateTime nuevaFechaDesde, DateTime nuevaFechaHasta)
        {
            if (nuevaFechaHasta < nuevaFechaDesde)
                throw new DomainException("La fecha hasta no puede ser menor a la fecha desde.");
            FechaDesde = nuevaFechaDesde;
            FechaHasta = nuevaFechaHasta;
        }
    }
}
