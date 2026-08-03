using Domain.VisitasGrupales.Entities;
using System.Threading.Tasks;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioConfiguracionVisitasGrupalesGuiadas
    {
        Task<ConfiguracionVisitasGrupalesGuiadas> ObtenerConfiguracionAsync();
        Task GuardarConfiguracionAsync(ConfiguracionVisitasGrupalesGuiadas configuracion);
    }
}
