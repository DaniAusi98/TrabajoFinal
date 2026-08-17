using Application.VisitaGrupal.UseCases.Comands.CrearVisitaAutoguiada;
using Application.VisitaGrupal.UseCases.Comands.CrearVisitaGuiada;
using Core.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VisitaAutoguiadaController(ICommandQueryBus commandQueryBus) : ControllerBase
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Create(
        CrearVisitaAutoguiadaCommand command)
        {
            if (command is null)
                return BadRequest();
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            command.UsuarioVisitanteId = usuarioId;
            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });

        }

        [HttpGet("DisponibilidadTurnosVisitasAutoguiadas")]
        public async Task<IActionResult> DisponibilidadTurnosVisitasAutoguiadas(
          [FromQuery] DateTime fechaDesde,
          [FromQuery] DateTime fechaHasta,
          [FromQuery] uint pageIndex = 1,
          [FromQuery] uint pageSize = 10)
        {
            var visitas = await _commandQueryBus.Send(
                new Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaAutoguiada.DisponibilidadTurnosVisitasAutoguiadasQuery(fechaDesde, fechaHasta)
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize
                });

            return Ok(visitas);
        }
        //GET /api/Visitas/DisponibilidadTurnosVisitasAutoguiadas?fechaDesde=2026-08-10&fechaHasta=2026-08-15&salaIds=1&salaIds=3&salaIds=5&pageIndex=1&pageSize=10
    }
}
