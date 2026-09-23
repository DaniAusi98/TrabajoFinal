using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using System.Drawing;
using static Domain.ActividadesAreaEducacion.Enums.Enums;

namespace Domain.ActividadesAreaEducacion.Entities
{
    public class ActividadEducativa :ActividadMuseo.Entities.ActividadMuseo
    {
       public string Titulo { get; set; }
       public TipoActividadEducativa TipoActividadEducativa { get; set; }
       public ProyectosAreaEducacion ProyectoVinculada { get; set; }
       public string InstitucionVinculada { get; set; }
       public string PublicoObjetivo { get; set; }
        public ActividadEducativa()
        {
                
        }

        public ActividadEducativa(
            string titulo,
            TipoActividadEducativa tipoActividadEducativa,
            ProyectosAreaEducacion proyectoVinculada,
            string institucionVinculada,
            string publicoObjetivo,
            TimeSlot horario,
            List<Sala> salas,
            int? cantidadAsistentes = null,
            List<RecursoAsignado>? recursos= null ,
            string? rrule = null
            //List<string>? nombres = null

            ) : base(
                categoria: ActividadMuseo.Enums.Enums.CategoriaActividad.ActividadEducativa,
                tipo: ActividadMuseo.Enums.Enums.TipoActividad.ActividadEducativa,
                cantidadAsistentes: cantidadAsistentes,
                horario : horario,
                salas: salas,
                recursos: recursos,
                rrule: rrule
            )
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El título no puede estar vacío.", nameof(titulo));
            }
            if (proyectoVinculada == null)
            {
                throw new ArgumentNullException(nameof(proyectoVinculada), "El proyecto vinculado no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(institucionVinculada))
            {
                throw new ArgumentException("La institución vinculada no puede estar vacía.", nameof(institucionVinculada));
            }
            if (string.IsNullOrWhiteSpace(publicoObjetivo))
            {
                throw new ArgumentException("El público objetivo no puede estar vacío.", nameof(publicoObjetivo));
            }
            Titulo = titulo;
            TipoActividadEducativa = tipoActividadEducativa;
            ProyectoVinculada = proyectoVinculada;
            InstitucionVinculada = institucionVinculada;
            PublicoObjetivo = publicoObjetivo;
        }
        public void ActualizarActividadEducativa(
            string titulo,
            TipoActividadEducativa tipoActividadEducativa,
            ProyectosAreaEducacion proyectoVinculada,
            string institucionVinculada,
            string publicoObjetivo,
            TimeSlot horario,
            List<Sala> salas,
            int? cantidadAsistentes = null,
            List<RecursoAsignado>? recursos = null,
            string? rrule = null
        )
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El título no puede estar vacío.", nameof(titulo));
            }
            if (proyectoVinculada == null)
            {
                throw new ArgumentNullException(nameof(proyectoVinculada), "El proyecto vinculado no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(institucionVinculada))
            {
                throw new ArgumentException("La institución vinculada no puede estar vacía.", nameof(institucionVinculada));
            }
            if (string.IsNullOrWhiteSpace(publicoObjetivo))
            {
                throw new ArgumentException("El público objetivo no puede estar vacío.", nameof(publicoObjetivo));
            }
            Titulo = titulo;
            TipoActividadEducativa = tipoActividadEducativa;
            ProyectoVinculada = proyectoVinculada;
            InstitucionVinculada = institucionVinculada;
            PublicoObjetivo = publicoObjetivo;
            // Update base class properties

            CambiarCantidadAsistentes(cantidadAsistentes);
            AsignarTimeSlots(horario);  
            AsignarSalas(salas);
            AsignarRecursos(recursos);
            ActualizarRRule(rrule);
        }



        // Additional properties and methods can be added here as needed
    }
}