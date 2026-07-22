using Core.Domain.Entities;

using Domain.Common.Exceptions;
using Domain.Validators.VisitasGrupalesValidators;

namespace Domain.VisitasGrupales.Entities
{
    public class TematicaVisita : DomainEntity<int, TematicasValidator>
    {
        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public bool Disponible { get; private set; } = true;

        protected TematicaVisita() { }

        public TematicaVisita(string nombre, string descripcion = "")
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("La temática es obligatoria.");

            Nombre = nombre.Trim();
            Descripcion = string.IsNullOrWhiteSpace(descripcion)
                ? string.Empty
                : descripcion.Trim();
        }

        public void Actualizar(string nombre, string descripcion = "")
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("La temática es obligatoria.");
            Nombre = nombre.Trim();
            Descripcion = string.IsNullOrWhiteSpace(descripcion)
                ? string.Empty
                : descripcion.Trim();
        }
        public void MarcarComoNoDisponible()
        {
            if (!Disponible)
            {
                throw new DomainException("La temática ya está marcada como no disponible.");
            }
            Disponible = false;
        }
    }
}
