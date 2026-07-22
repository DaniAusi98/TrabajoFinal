using Core.Domain.Entities;

using Domain.Validators.UsuarioMuseo;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class AreaPuesto: DomainEntity<int, AreaPuestoValidator>

    {
        public int AreaId { get; private set; }
        public Area Area { get; private set; }

        public int PuestoId { get; private set; }
        public Puesto Puesto { get; private set; }
    }
}
