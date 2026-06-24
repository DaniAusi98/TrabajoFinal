using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.Usuario.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DomainEvents;
using Application.Usuario.UseCases.Commands.Register;

using Core.Application;



namespace Application.Usuario.UseCases.Commands.Register
{
    internal sealed class RegistrarVisitanteHandler(
        IIdentityService identityService
    ) : IRequestCommandHandler<RegistrarVisitanteCommand, string>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));

        public async Task<string> Handle(RegistrarVisitanteCommand request, CancellationToken cancellationToken)
        {
            if (await _identityService.UserExistsAsync(request.Email)) throw new EntityDoesExistException();

            var userId= await _identityService.RegisterAsync(request.Nombre, request.Apellido, request.FechaNac, request.Email, request.Telefono, request.Password);

            return userId;

        }

       
    }
}

