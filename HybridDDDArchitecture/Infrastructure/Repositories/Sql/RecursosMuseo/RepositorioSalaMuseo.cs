using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.MuseumResources.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.RecursoMuseo.Entities;

namespace Infrastructure.Repositories.Sql.RecursosMuseo
{
    internal sealed class RepositorioSalaMuseo(MuseoDbContext context) : BaseRepository<Sala>(context), IRepositorioSala
    {
    }
}
/* internal sealed class DummyEntityRepository(MuseoDbContext context) : BaseRepository<DummyEntity>(context), IDummyEntityRepository
    {
    }*/
