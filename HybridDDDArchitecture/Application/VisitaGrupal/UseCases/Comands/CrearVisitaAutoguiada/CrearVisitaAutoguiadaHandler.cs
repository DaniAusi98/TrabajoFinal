using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Application.MuseumResources.UseCases.MuseumGallery.Queries.GetAllSalas;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;

namespace Application.VisitaGrupal.UseCases.Comands.CrearVisitaAutoguiada
{
    internal sealed class CrearVisitaAutoguiadaHandler(ICommandQueryBus commandQueryBus,IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,IRepositorioSala repositorioSala, IRepositorioTematicas _tematicaRepository) : IRequestCommandHandler<CrearVisitaAutoguiadaCommand, string>
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        private readonly IRepositorioTematicas _tematicaRepository = _tematicaRepository ?? throw new ArgumentNullException(nameof(_tematicaRepository));
        public async Task<string> Handle(CrearVisitaAutoguiadaCommand request, CancellationToken cancellationToken)
        {
            var timeSlot = new TimeSlot(
               request.Inicio,
               request.Fin
           );
            var tematicas = await _tematicaRepository
              .GetByIdsAsync(request.TematicasIds);

            if (tematicas.Count != request.TematicasIds.Count)
            {
                throw new BussinessException(
                    "Una o más temáticas no existen.");
            }
            var salas= await _repositorioSala.ObtenerSalasporIdsAsync(request.SalasIds);
                
            var visita = new Domain.VisitasGrupales.Entities.VisitaGrupalAutoguiada(
                usuarioVisitanteId: request.UsuarioVisitanteId,
                institucion: request.Institucion,
                emailInstitucion: new Email(request.EmailInstitucion),
                provinciaInstitucion: request.ProvinciaInstitucion,
                departamentoInstitucion: request.DepartamentoInstitucion,
                ciudadInstitucion: request.LocalidadInstitucion,
                descripcionDiversidad: request.DiversidadFuncionalDescripcion,
                observaciones: request.Observaciones,
                cantidadPersonas: request.CantidadPersonas,
                salas: salas,
                tematicas: tematicas,

                timeSlots: [timeSlot]
            );
            try
            {
                object createdId = await _repositorioVisitaGrupalAutoguiada.AddAsync(visita);

                //await _domainBus.Publish(entity.To<DummyEntityCreated>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }


    }
}
