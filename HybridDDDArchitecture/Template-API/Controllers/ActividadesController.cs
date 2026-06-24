using Application.ActividadMuseo.UseCases.ActividadesMuseo.Queries;

using Core.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ActividadesController(ICommandQueryBus commandQueryBus): BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));


        [HttpGet]
        public async Task<IActionResult> GetAll(
        DateTime fechaDesde,
        DateTime fechaHasta,
        uint pageIndex = 1,
        uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllActivitiesQuery(fechaDesde,fechaHasta) { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }







    }
}
