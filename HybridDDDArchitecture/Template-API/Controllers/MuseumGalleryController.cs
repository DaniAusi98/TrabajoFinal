using Microsoft.AspNetCore.Mvc;

using Application.ApplicationMuseo.UseCases.MuseumGallery.Commands.CreateMuseumGallery;
using Application.ApplicationMuseo.UseCases.MuseumGallery.Commands.UpdateMuseumGallery;
using Application.ApplicationMuseo.UseCases.MuseumGallery.Commands.DeleteMuseumGallery;
using Application.ApplicationMuseo.UseCases.MuseumGallery.Queries.GetAllMuseumGalleries;
using Application.ApplicationMuseo.UseCases.MuseumGallery.Queries.GetMuseumGalleryBy;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    public class MuseumGalleryController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllMuseumGalleriesQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpGet("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetMuseumGalleryByQuery { MuseumGalleryId = id });

            return Ok(entity);
        }

        [HttpPost("api/v1/[Controller]")]
        public async Task<IActionResult> Create(CreateMuseumGalleryCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpPut("api/v1/[Controller]")]
        public async Task<IActionResult> Update(UpdateMuseumGalleryCommand command)
        {
            if (command is null) return BadRequest();

            await _commandQueryBus.Send(command);

            return NoContent();
        }

        [HttpDelete("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            await _commandQueryBus.Send(new DeleteMuseumGalleryCommand { MuseumGalleryId = id });

            return NoContent();
        }
    }
}
