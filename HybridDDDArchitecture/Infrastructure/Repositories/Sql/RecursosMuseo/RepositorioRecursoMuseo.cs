using Application.MuseumResources.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.RecursoMuseo.Entities;

namespace Infrastructure.Repositories.Sql.RecursosMuseo
{
    internal sealed class RepositorioRecursoMuseo(MuseoDbContext context) : BaseRepository<Recurso>(context), IRepositorioRecurso
    {
    }
}

