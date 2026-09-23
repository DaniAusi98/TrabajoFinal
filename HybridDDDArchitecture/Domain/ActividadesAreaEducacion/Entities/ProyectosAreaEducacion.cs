using Core.Domain.Entities;

namespace Domain.ActividadesAreaEducacion.Entities
{
    public class ProyectosAreaEducacion:DomainEntity<string>
    {
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Dictionary<int, (string Nombre, string Institucion)> EquipoTrabajoResponsable { get; set; }
        public string Publico { get; set; }

        // Additional properties and methods can be added here as needed
    }
}