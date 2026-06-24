using Core.Domain.Entities;
using Domain.Exceptions;
using Domain.Validators.RecursoMuseoValidators;
using static Domain.Enums.RecursoMuseoEnums.Enums;

namespace Domain.Entities.RecursoMuseo
{
    public class Recurso : DomainEntity<int, RecursoValidator>
    {
        public string NombreRecurso { get; private set; }
        public TipoRecurso TipoRecurso { get; private set; }
        public string Descripcion { get; private set; }
        public EstadoRecurso Estado { get; private set; }
        public int CantidadTotal { get; private set; }
        public Recurso(string nombreRecurso, TipoRecurso tipoRecurso, string descripcion, int cantidadDisponible)
        {
            SetNombreRecurso(nombreRecurso);
            TipoRecurso = tipoRecurso;
            SetDescripcion(descripcion);
            CantidadTotal = cantidadDisponible;
            Estado = EstadoRecurso.Disponible;

        }
        public void SetNombreRecurso(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException(
                    "El nombre del recurso es obligatorio.");

            NombreRecurso = value.Trim();
        }

        public void SetTipoRecurso(TipoRecurso value)
        {
            TipoRecurso = value;
        }
        public void SetDescripcion(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException(
                    "La descripción del recurso es obligatoria.");

            Descripcion = value.Trim();
        }
        public void SetCantidadTotal(int value)
        {
            if(value <= 0)
                throw new DomainException(
                    "La cantidad debe ser mayor a 0");
            CantidadTotal = value;

        }
        public void SetEstadoRecurso(EstadoRecurso value)
        {
            Estado = value;
        }
       
        public Recurso()
        {

        }
    }
}
