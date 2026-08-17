namespace Domain.Reportes.DomainServices
{
    public class TasaOcupacionVisitas
    {
        public static decimal Calcular(
        int cantidadPersonas,
        int capacidadDisponible)
        {
            if (capacidadDisponible == 0)
                return 0;

            return (decimal)cantidadPersonas
                / capacidadDisponible * 100;
        }
    }
}
