using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Core.Domain.Entities;

using Domain.Common.Exceptions;

namespace Domain.VisitasGrupales.Entities
{
    public class BloqueoVisitaGuiada : DomainEntity<int>
    {
        public DateTime FechaDesde { get; private set; }
        public DateTime FechaHasta { get; private set; }

        public string Motivo { get; private set; }

        protected BloqueoVisitaGuiada()
        {
            // EF Core
        }

        public BloqueoVisitaGuiada(
            DateTime fechaDesde,
            DateTime fechaHasta,
            string motivo)
        {
            if (fechaHasta <= fechaDesde)
                throw new DomainException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new DomainException(
                    "El motivo del bloqueo es obligatorio.");

            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Motivo = motivo.Trim();
        }

        public bool SolapaConFechas(DateTime inicio, DateTime fin)
        {
            return inicio < FechaHasta &&
                   fin > FechaDesde;
        }

        public void ActualizarFechas(
            DateTime nuevaFechaDesde,
            DateTime nuevaFechaHasta)
        {
            if (nuevaFechaHasta <= nuevaFechaDesde)
                throw new DomainException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");

            FechaDesde = nuevaFechaDesde;
            FechaHasta = nuevaFechaHasta;
        }

        public void ActualizarMotivo(string nuevoMotivo)
        {
            if (string.IsNullOrWhiteSpace(nuevoMotivo))
                throw new DomainException(
                    "El motivo del bloqueo es obligatorio.");

            Motivo = nuevoMotivo.Trim();
        }
    }
}
