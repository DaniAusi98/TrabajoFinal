using Domain.ValueObjets;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjets.UsuarioMuseo
{
    public class PasswordHash : ValueObject
    {
        public string Valor { get; }

        protected PasswordHash() { } // Para EF Core


        public PasswordHash(string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException("El hash no puede estar vacío.", nameof(hashedPassword));

            Valor = hashedPassword;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }
    }
}
