using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RecursoMuseo.Enums
{
    public class Enums
    {
        /// <summary>
        /// Ejemplo de enumeracion Dummy
        /// </summary>
        public enum TipoSala
        {
            SalaExposicionPermanente,
            SalaExposicionTemporal,
            Hall,
            Biblioteca,
            Auditorio,
            AulaEducativa,
            ReservaPatrimonial,
            Documentacion,
            Multifuncion
        }

        public enum DatabaseType
        {
            MYSQL,
            MARIADB,
            SQLSERVER,
            MONGODB
        }

        public enum UbicacionSala
        {
            PlantaBaja,
            PrimerPiso,
            SegundoPiso
        }
        public enum TipoRecurso
        {
            Mobiliario,
            Tecnologico,
            Audiovisual
        }

        public enum EstadoRecurso
        {
            Disponible,
            Prestado,
            EnMantenimiento
        }
    }
}