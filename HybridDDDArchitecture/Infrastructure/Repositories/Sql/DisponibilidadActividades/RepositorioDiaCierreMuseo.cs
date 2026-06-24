using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.ApplicationMuseo.Repositories;
using Application.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.Entities;
using Domain.Entities.DisponibilidadMuseo;

namespace Infrastructure.Repositories.Sql.DisponibilidadActividades
{
    internal sealed class RepositorioDiaCierreMuseo(MuseoDbContext context) : BaseRepository<DiaCierreMuseo>(context), IRepositorioDiaCierreMuseo
    {
    }
}
