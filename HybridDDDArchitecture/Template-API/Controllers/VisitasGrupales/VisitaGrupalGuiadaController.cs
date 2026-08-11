using System.Security.Claims;

using Application.ActividadMuseo.UseCases.ActividadesMuseo.Queries;
using Application.VisitaGrupal.UseCases.Comands.CancelarVisitaGuiada;
using Application.VisitaGrupal.UseCases.Comands.CrearVisitaGuiada;
using Application.VisitaGrupal.UseCases.Comands.NewFolder;
using Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaGuiada;
using Application.VisitaGrupal.UseCases.Queries.GetAllGroupVisitCalendar;
using Application.VisitaGrupal.UseCases.Queries.GetReservationById;
using Application.VisitaGrupal.UseCases.Queries.GetReservationsById;
using Application.VisitaGrupal.UseCases.Queries.GetTematicaVisitaGrupal;
using Core.Application;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.VisitasGrupales;

[ApiController]
[Route("api/v1/[controller]")]
public class VisitasGuiadasController(ICommandQueryBus commandQueryBus) : ControllerBase
{
    private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<IActionResult> Create(
        CrearVisitaGuiadaCommand command)
    {
        if (command is null)
            return BadRequest();
        var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        command.UsuarioVisitanteId = usuarioId;
        var id = await _commandQueryBus.Send(command);

        return Created($"api/[Controller]/{id}", new { Id = id });

    }
    [HttpGet("DisponibilidadTurnosVisitasGuiadas")]
    public async Task<IActionResult> DisponibilidadTurnosVisitasGuiadas(
        DateTime fechaDesde,
        DateTime fechaHasta,
        uint pageIndex = 1,
        uint pageSize = 10)
    {
        var visitas = await _commandQueryBus.Send(
            new ConsultarDisponibilidadTurnosDiaQuery(fechaDesde, fechaHasta)
            {

                PageIndex = pageIndex,
                PageSize = pageSize
            });

        return Ok(visitas);
    }
    [HttpGet("TematicasDisponibles")]
    public async Task<IActionResult> TematicasDisponibles()
    {
        var tematicas = await _commandQueryBus.Send(
            new GetAllTematicasQuery());
        return Ok(tematicas);
    }



    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("reservations")]
    public async Task<IActionResult> GetReservations()
    {
        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
            return Unauthorized();

        var visitas = await _commandQueryBus.Send(
            new GetReservationsByUserIdQuery
            {
                UsuarioVisitanteId = usuarioId
            });

        return Ok(visitas);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservationById(int id)
    {
        if (id <= 0)
            return BadRequest();
        var entity= await _commandQueryBus.Send(new GetReservationByIdQuery { ReservationId = id });
        return Ok(entity);
    }
    

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        if (id <= 0)
            return BadRequest();

        await _commandQueryBus.Send(
            new CancelarVisitaGuiadaCommand
            {
                ReservationId = id
            });

        return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("{id}/reprogram")]
    public async Task<IActionResult> Reprogram(
        int id,
        ReprogramarCommand command)
    {
        var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        command.UsuarioVisitanteId = usuarioId;
        command.VisitaReprogramadaId = id;

        var nuevaVisitaId = await _commandQueryBus.Send(command);

        return Created($"api/v1/VisitasGuiadas/{nuevaVisitaId}",new { Id = nuevaVisitaId });
    }

    [HttpGet("calendar")]
    public async Task<IActionResult> GetAllGroupVisitCalendar(
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta)
    {
        if (!fechaDesde.HasValue || !fechaHasta.HasValue)
        {
            return BadRequest("Parámetros 'fechaDesde' y 'fechaHasta' requeridos en la query. Formato ISO: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss");
        }

        var groupVisits = await _commandQueryBus.Send(new GetAllGroupVisitCalendarQuery(fechaDesde.Value, fechaHasta.Value));
        return Ok(groupVisits);
    }



    /*

[HttpGet]
    public async Task<IActionResult> GetAll(
        uint pageIndex = 1,
        uint pageSize = 10)
    {
        var visitas = await _commandQueryBus.Send(
            new GetAllVisitasGuiadasQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            });

        return Ok(visitas);
    }
  

 [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _commandQueryBus.Send(
            new DeleteVisitaGuiadaCommand
            {
                Id = id
            });

        return NoContent();
    }
 
 
 
 
 */
}
