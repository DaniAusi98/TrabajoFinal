using Domain.VisitasGrupales.Options;
using System.Threading.Tasks;

namespace Application.VisitaGrupal.ApplicationServices
{
    public interface IConfiguracionVisitasGrupalesGuiadasService
    {
        Task<TurnosVisitasOptions> GetAsync();
        Task UpdateAsync(TurnosVisitasOptions options);
    }
}
