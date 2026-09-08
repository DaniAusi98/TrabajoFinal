using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioEvento(MuseoDbContext context):BaseRepository<Evento>(context),IRepositorioEvento
    {
    }
}
