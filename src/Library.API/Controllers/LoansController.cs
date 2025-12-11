using Library.Application.DTOs.Loan;
using Library.Application.Interfaces;
using Library.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _service;

        public LoansController(ILoanService service)
        {
            _service = service;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var list = await _service.GetActiveLoansAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLoanDto dto)
        {
            try
            {
                var created = await _service.CreateLoanAsync(dto);
                return CreatedAtAction(nameof(GetActive), new { id = created.Id }, created);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("return/{id}")]
        public async Task<IActionResult> Return(int id)
        {
            try
            {
                await _service.ReturnLoanAsync(id);
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
    }
}
