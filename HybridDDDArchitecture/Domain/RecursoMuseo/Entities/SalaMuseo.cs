using Core.Domain.Entities;

using Domain.Validators.RecursoMuseoValidators;

using static Domain.RecursoMuseo.Enums.Enums;

namespace Domain.RecursoMuseo.Entities
{
    public class Sala : DomainEntity<int, SalaValidator>
    {
        public string Nombre { get; private set; }
        public string CodigoSala {get; private set; }   
        public TipoSala TipoSala { get; private set; }
        public int Capacidad { get; private set; }
        public UbicacionSala Ubicacion { get; private set; }


        public Sala(string nombre, TipoSala tipoSala, int capacidad, UbicacionSala ubicacionSala,string codigoSala)
        {
            Nombre = nombre;
            TipoSala = tipoSala;
            Capacidad = capacidad;
            Ubicacion = ubicacionSala;
            CodigoSala = codigoSala;

        }
        public Sala(int id, string nombre, TipoSala tipoSala, int capacidad, UbicacionSala ubicacionSala, string codigoSala)
        {
            Id = id;
            Nombre = nombre;
            TipoSala = tipoSala;
            Capacidad = capacidad;
            Ubicacion = ubicacionSala;
            CodigoSala = codigoSala;
        }
        public Sala() { }

        public void SetNombre(string nombre)
        {
                if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la sala no puede ser nulo o vacío.", nameof(nombre));
            Nombre = nombre.Trim();
        }
        public void SetTipoSala(TipoSala tipoSala)
        {
            TipoSala = tipoSala;
        }
        public void SetCapacidad(int capacidad)
        {
            if (capacidad <= 0)
                throw new ArgumentException("La capacidad de la sala debe ser mayor a 0.", nameof(capacidad));
            Capacidad = capacidad;
        }
        public void SetUbicacion(UbicacionSala ubicacion)
        {
            Ubicacion = ubicacion;
        }

        public void SetCodigoSala (string nuevoCodigo)
        {
                if (string.IsNullOrWhiteSpace(nuevoCodigo))
                    throw new ArgumentException("El código de la sala no puede ser nulo o vacío.", nameof(nuevoCodigo));
            CodigoSala = nuevoCodigo.Trim();
        }

    }

}
