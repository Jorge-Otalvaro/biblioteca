using Microsoft.AspNetCore.Mvc;
using PruebaIngresoBibliotecario.Api.Domain.Model.Entities;
using PruebaIngresoBibliotecario.Api.Domain.Model.Entities.Dtos;
using PruebaIngresoBibliotecario.Api.Domain.Model.Interfaces;
using System;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PrestamoController : ControllerBase
    {        
        private readonly IPrestamoUseCase _loanService;

        public PrestamoController(IPrestamoUseCase loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("/api/prestamo")]
        public async Task<IActionResult> CreateLoanAsync([FromBody] LoanRequestDto loanRequest)
        {
            if (!IsIsbnValid(loanRequest.Isbn))
            {
                return BadRequest(new { mensaje = "El campo isbn es requerido y debe tener una longitud maxima de 13 caracteres" });
            }

            if (!IsTipoUsuarioValid(loanRequest.TipoUsuario))
            {
                return BadRequest(new { mensaje = "El campo tipoUsuario debe ser un valor entre 1 y 3" });
            }

            if (!IsIdentificacionUsuarioValid(loanRequest.IdentificacionUsuario))
            {
                return BadRequest(new { mensaje = "El campo identificacionUsuario es requerido y debe tener una longitud maxima de 10 caracteres" });
            }
            
            if (loanRequest.TipoUsuario == 3 && await _loanService.LoanExistsForUser(loanRequest.IdentificacionUsuario))
            {
                string errorMessage = $"El usuario con identificacion {loanRequest.IdentificacionUsuario} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo";
                return BadRequest(new { mensaje = errorMessage });
            }

            Loan response = await _loanService.CreateLoan(loanRequest);

            return Ok(new { 
                id = response.Id,
                fechaMaximaDevolucion = response.FechaMaximaDevolucion
            });
        }

        [HttpGet("/api/prestamo/{idPrestamo}")]
        public async Task<IActionResult> GetLoanById(Guid idPrestamo)
        {            
            if (idPrestamo == Guid.Empty)
            {
                return BadRequest(new { mensaje = "El campo idPrestamo es requerido" });
            }

            LoanResponseDto loanResponse = await _loanService.GetLoanById(idPrestamo);

            if (loanResponse == null)
            {
                return NotFound(new { mensaje = $"El préstamo con ID {idPrestamo} no existe" });
            }

            return Ok(loanResponse);
        }

        private bool IsIsbnValid(string isbn)
        {            
            return !string.IsNullOrEmpty(isbn) && isbn.Length <= 36;
        }

        private bool IsTipoUsuarioValid(int tipoUsuario)
        {
            return tipoUsuario >= 1 && tipoUsuario <= 3;
        }

        private bool IsIdentificacionUsuarioValid(string identificacionUsuario)
        {
            return !string.IsNullOrEmpty(identificacionUsuario) && identificacionUsuario.Length <= 10;
        }
    }
}
