using Core.Domain.Entities;

using Domain.Validators.UsuarioMuseo;
namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class PersonalInterno : DomainEntity<int, PersonalValidator>
    {
        public string IdentityUserId { get; private set; }
        public int AreaId { get; private set; }
        public Area Area { get; private set; }

        public int PuestoId { get; private set; }
        public Puesto Puesto { get; private set; }
    }
}
