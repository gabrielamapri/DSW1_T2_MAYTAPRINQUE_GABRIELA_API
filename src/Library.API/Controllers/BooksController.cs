using Library.Application.DTOs.Book;
using Library.Application.Interfaces;
using Library.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound(new { error = "Recurso no encontrado" });
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound(new { error = "Recurso no encontrado" });
            }
            catch (BusinessRuleException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpPost("dar-baja/{id}")]
        public async Task<IActionResult> DarBaja(int id, Library.Application.DTOs.Book.DarBajaDto dto)
        {
            try
            {
                var liquidated = await _service.DarBajaAsync(id, dto);
                if (liquidated)
                    return Ok(new { message = "Artículo dado de baja correctamente. Liquidación registrada." });
                return Ok(new { message = "Artículo dado de baja correctamente." });
            }
            catch (Library.Domain.Exceptions.NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (System.Exception)
            {
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }
    }
}
