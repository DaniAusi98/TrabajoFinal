using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;

using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Enums;


namespace Application.VisitaGrupal.UseCases.Comands.CrearVisitaGuiada
{
    internal sealed class CrearVisitaGuiadaHandler(ICommandQueryBus domainBus, IRepositorioVisitaGuiada repositorioVisitaGuiada,IRepositorioTematicas tematicaRepository) : IRequestCommandHandler<CrearVisitaGuiadaCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IRepositorioTematicas _tematicaRepository = tematicaRepository ?? throw new ArgumentNullException(nameof(tematicaRepository));

        public async Task<string> Handle(CrearVisitaGuiadaCommand request, CancellationToken cancellationToken)
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
            var salas = tematicas
                .SelectMany(t => t.Salas)
                .DistinctBy(s => s.Id)
                .ToList();

            var visita = new VisitaGrupalGuiada(
                usuarioVisitanteId: request.UsuarioVisitanteId,
                nivelEducativo: request.NivelEducativo,
                anioGrado: request.AnioGrado,
                cantidadPersonas: request.CantidadPersonas,
                institucion: request.Institucion,
                emailInstitucion: new Email(request.EmailInstitucion),
                telefonoInstitucion: new Telefono(request.TelefonoInstitucion),
                provinciaInstitucion: request.ProvinciaInstitucion,
                departamentoInstitucion: request.DepartamentoInstitucion,
                ciudadInstitucion: request.LocalidadInstitucion,
                descripcionDiversidad: request.DiversidadFuncionalDescripcion,
                motivoVisita: request.MotivoRelacionVisita,
                observaciones: request.Observaciones,
                timeSlots: [timeSlot],
                tematicas: tematicas,
                salas: salas
            );
          
            try
            {
                object createdId = await _repositorioVisitaGuiada.AddAsync(visita);

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
/*using Application.ApplicationMuseo.ApplicationServices;
using Application.ApplicationMuseo.Constants;
using Application.ApplicationMuseo.DomainEvents;
using Application.ApplicationMuseo.Repositories;
using Application.Exceptions;
using Core.Application;

namespace Application.ApplicationMuseo.UseCases.DummyEntity.Commands.CreateDummyEntity
{
    /// <summary>
    /// Ejemplo de handler que responde al comando <see cref="CreateDummyEntityCommand"/>
    /// y ejecuta el proceso para el caso de uso en cuestion.
    /// Todo handler debe implementar la interfaz <see cref="IRequestCommandHandler{TRequest, TResponse}"/>
    /// si devuelve una respuesta donde <c TRequest> es del tipo <see cref="CreateDummyEntityCommand"/>
    /// y <c TResponse> del tipo de dato definido para la respuesta
    /// /// </summary>
    internal sealed class CreateDummyEntityHandler(ICommandQueryBus domainBus, IDummyEntityRepository dummyEntityRepository, IDummyEntityApplicationService dummyEntityApplicationService) : IRequestCommandHandler<CreateDummyEntityCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IDummyEntityRepository _context = dummyEntityRepository ?? throw new ArgumentNullException(nameof(dummyEntityRepository));
        private readonly IDummyEntityApplicationService _dummyEntityApplicationService = dummyEntityApplicationService ?? throw new ArgumentNullException(nameof(dummyEntityApplicationService));
        public async Task<string> Handle(CreateDummyEntityCommand request, CancellationToken cancellationToken)
        {
            Domain.CommonDomain.Entities.DummyEntity entity = new(request.dummyPropertyOne, request.dummyPropertyTwo);

            if (!entity.IsValid) throw new InvalidEntityDataException(entity.GetErrors());

            if (_dummyEntityApplicationService.DummyEntityExist(entity.Id)) throw new EntityDoesExistException();

            try
            {
                object createdId = await _context.AddAsync(entity);

                await _domainBus.Publish(entity.To<DummyEntityCreated>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
*/
