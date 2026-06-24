using Core.Domain.Entities;

using Domain.Validators.UsuarioMuseo;

namespace Domain.Entities.UsuarioMuseo.UsuarioInterno
{
    public class Area: DomainEntity<int, AreaValidator>
 
    {
     
        public string Nombre { get; private set; }
        public List<AreaPuesto> AreaPuestos { get; private set; } = [];
    }
}
