using Core.Domain.Entities;

using Domain.Validators.UsuarioMuseo;
namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class Puesto: DomainEntity<int, PuestoValidator>

    {
        public string Nombre { get; private set; }

        public List<AreaPuesto> AreaPuestos { get; private set; } = [];
    }
}
