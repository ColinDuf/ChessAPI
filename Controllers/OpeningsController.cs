// OpeningsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpeningExplorer.DTOs;
using OpeningExplorer.Services;

namespace OpeningExplorer.Controllers
{
    /// <summary>
    /// Gestion des ouvertures d'échecs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OpeningsController : ControllerBase
    {
        private readonly IOpeningService _svc;
        public OpeningsController(IOpeningService svc) => _svc = svc;

        /// <summary>
        /// Retourne toutes les ouvertures.
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IEnumerable<OpeningDto>> GetAll() => await _svc.GetAll();

        /// <summary>
        /// Récupère une ouverture spécifique.
        /// </summary>
        /// <param name="id">Identifiant de l'ouverture.</param>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var opening = await _svc.Get(id);
                return Ok(opening);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Crée une nouvelle ouverture.
        /// </summary>
        /// <param name="dto">Données de l'ouverture.</param>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreateOpeningDto dto)
        {
            var id = await _svc.Create(dto);
            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        /// <summary>
        /// Met à jour une ouverture existante.
        /// </summary>
        /// <param name="id">Identifiant de l'ouverture.</param>
        /// <param name="dto">Nouvelles données de l'ouverture.</param>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateOpeningDto dto)
        {
            try
            {
                await _svc.Update(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Supprime une ouverture.
        /// </summary>
        /// <param name="id">Identifiant de l'ouverture.</param>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _svc.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}