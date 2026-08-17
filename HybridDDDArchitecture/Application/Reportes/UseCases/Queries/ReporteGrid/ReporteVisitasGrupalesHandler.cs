using Application.Common.Repositories;
using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
namespace Application.Reportes.UseCases.Queries.ReporteGrid
{
    internal sealed class ReporteVisitasGrupalesHandler(
        ICommandQueryBus commandQueryBus,
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
        IRepositorioVisitaGuiada repositorioVisitaGuiada,
        IProvinciaRepository repositorioProvincia,
        IDepartamentoRepository repositorioDepartamento,
        ILocalidadRepository repositorioLocalidad) 
        : IRequestQueryHandler<ReporteVisitasGrupalesQuery, QueryResult<ReporteGridDto>>
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus)) ;
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));
        private readonly IProvinciaRepository _repositorioProvincia = repositorioProvincia ?? throw new ArgumentNullException(nameof(repositorioProvincia));
        private readonly IDepartamentoRepository _repositorioDepartamento = repositorioDepartamento ?? throw new ArgumentNullException(nameof(repositorioDepartamento));
        private readonly ILocalidadRepository _repositorioLocalidad = repositorioLocalidad ?? throw new ArgumentNullException(nameof(repositorioLocalidad));
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        public async Task<QueryResult<ReporteGridDto>> Handle(ReporteVisitasGrupalesQuery request, CancellationToken cancellationToken)
        {
            var visitasGrupalesAutoguiadas = await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(request.Desde, request.Hasta);
            var visitasGrupalesGuiadas = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(request.Desde, request.Hasta);
            // Recolectar ids únicos
            var provinciaIds = visitasGrupalesGuiadas.Select(v => v.ProvinciaInstitucion)
                .Concat(visitasGrupalesAutoguiadas.Select(v => v.ProvinciaInstitucion))
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            var departamentoIds = visitasGrupalesGuiadas.Select(v => v.DepartamentoInstitucion)
                .Concat(visitasGrupalesAutoguiadas.Select(v => v.DepartamentoInstitucion))
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            var localidadIds = visitasGrupalesGuiadas.Select(v => v.LocalidadInstitucion)
                .Concat(visitasGrupalesAutoguiadas.Select(v => v.LocalidadInstitucion))
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            // Consultas en bloque
            var provincias = await _repositorioProvincia.GetByIdsAsync(provinciaIds);
            var departamentos = await _repositorioDepartamento.GetByIdsAsync(departamentoIds);
            var localidades = await _repositorioLocalidad.GetByIdsAsync(localidadIds);

            var mapaProvincias = provincias.ToDictionary(p => p.Id, p => p.Nombre);
            var mapaDepartamentos = departamentos.ToDictionary(d => d.Id, d => d.Nombre);
            var mapaLocalidades = localidades.ToDictionary(l => l.Id, l => l.Nombre);
            var reporteVisitasGrupalesDto = new List<ReporteGridDto>();
            foreach(var visita in visitasGrupalesAutoguiadas)
            {
                var slot = visita.TimeSlots.OrderBy(ts => ts.Inicio).FirstOrDefault();

                reporteVisitasGrupalesDto.Add(new ReporteGridDto
                {

                    Id = visita.Id,
                    Estado = visita.Estado,
                    EstadoConfirmacion = visita.EstadoConfirmacion,
                    Tipo = TipoVisitaGrupal.Autoguiada,
                    CantidadPersonas = visita.CantidadPersonas??0,
                    Institucion = visita.Institucion,
                    Provincia = mapaProvincias.TryGetValue(visita.ProvinciaInstitucion, out var provinciaNombre) ? provinciaNombre : null,
                    Departamento = mapaDepartamentos.TryGetValue(visita.DepartamentoInstitucion, out var departamentoNombre) ? departamentoNombre : null,
                    Localidad = mapaLocalidades.TryGetValue(visita.LocalidadInstitucion, out var localidadNombre) ? localidadNombre : null,
                    HoraInicio = slot?.Inicio,
                    HoraFin = slot?.Fin
                });
            }

            foreach(var visita in visitasGrupalesGuiadas)
            {
                if (visita == null) continue;
                var slot = visita.TimeSlots.OrderBy(ts => ts.Inicio).FirstOrDefault();


                reporteVisitasGrupalesDto.Add(new ReporteGridDto
                {
                    Id = visita.Id,
                    Estado = visita.Estado,
                    EstadoConfirmacion = visita.EstadoConfirmacion,
                    Tipo = TipoVisitaGrupal.Guiada,
                    CantidadPersonas = visita.CantidadPersonas??0,
                    Institucion = visita.Institucion,
                    Provincia = mapaProvincias.TryGetValue(visita.ProvinciaInstitucion, out var provinciaNombre) ? provinciaNombre : null,
                    Departamento = mapaDepartamentos.TryGetValue(visita.DepartamentoInstitucion, out var departamentoNombre) ? departamentoNombre : null,
                    Localidad = mapaLocalidades.TryGetValue(visita.LocalidadInstitucion, out var localidadNombre) ? localidadNombre : null,
                    HoraInicio = slot?.Inicio,
                    HoraFin = slot?.Fin
                });
            }

            long totalElementos = reporteVisitasGrupalesDto.Count;
            uint tamañoPagina = (uint)reporteVisitasGrupalesDto.Count;
            uint indicePagina = 1;

            return new QueryResult<ReporteGridDto>(
                reporteVisitasGrupalesDto,
                totalElementos,
                tamañoPagina,
                indicePagina
            );
        }
    }
}
