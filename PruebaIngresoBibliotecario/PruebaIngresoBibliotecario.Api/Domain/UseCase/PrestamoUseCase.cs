using Microsoft.EntityFrameworkCore;
using PruebaIngresoBibliotecario.Api.Domain.Model.Entities;
using PruebaIngresoBibliotecario.Api.Domain.Model.Entities.Dtos;
using PruebaIngresoBibliotecario.Api.Domain.Model.Interfaces;
using PruebaIngresoBibliotecario.Infrastructure;
using System;
using System.Threading.Tasks;
using static PruebaIngresoBibliotecario.Api.Domain.Model.Entities.Loan;

namespace PruebaIngresoBibliotecario.Api.Domain.UseCase
{
    public class PrestamoUseCase : IPrestamoUseCase
    {
        private readonly PersistenceContext _dbContext;

        public PrestamoUseCase(PersistenceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> LoanExistsForUser(string identificacionUsuario)
        {            
            return await _dbContext.Loans.AnyAsync(l => l.IdentificacionUsuario == identificacionUsuario);
        }

        // Crear un préstamo de un libro
        public async Task<Loan> CreateLoan(LoanRequestDto loanRequest)
        {
            DateTime fechaMaximaDevolucion = CalculateDueDate(loanRequest.TipoUsuario);

            Loan response = new Loan
            {
                Id = Guid.NewGuid(),
                Isbn = loanRequest.Isbn,
                IdentificacionUsuario = loanRequest.IdentificacionUsuario,
                TipoUsuario = (TipoUsuarioEnum)loanRequest.TipoUsuario,
                FechaMaximaDevolucion = fechaMaximaDevolucion.ToString("dd/MM/yyyy")
            };

            _dbContext.Loans.Add(response);
            await _dbContext.SaveChangesAsync();

            return response;
        }

        // Consulta de un préstamo por ID
        public async Task<LoanResponseDto> GetLoanById(Guid idPrestamo)
        {
            Loan loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == idPrestamo);

            if (loan == null)
            {
                return null;
            }

            LoanResponseDto loanResponse = new LoanResponseDto
            {
                Id = loan.Id,
                Isbn = loan.Isbn,
                IdentificacionUsuario = loan.IdentificacionUsuario,
                TipoUsuario = (int)loan.TipoUsuario,
                FechaMaximaDevolucion = DateTime.Parse(loan.FechaMaximaDevolucion)
            };

            return loanResponse;
        }
        
        private DateTime CalculateDueDate(int tipoUsuario)
        {
            var fechaMaximaDevolucion = DateTime.Today;
            int daysToAdd = 0;

            switch (tipoUsuario)
            {
                case 1: // Usuario afiliado
                    daysToAdd = 10;
                    break;
                case 2: // Usuario empleado
                    daysToAdd = 8;
                    break;
                case 3: // Usuario invitado
                    daysToAdd = 7;
                    break;
            }

            while (daysToAdd > 0)
            {
                fechaMaximaDevolucion = fechaMaximaDevolucion.AddDays(1);

                if (fechaMaximaDevolucion.DayOfWeek != DayOfWeek.Saturday && fechaMaximaDevolucion.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysToAdd--;
                }
            }

            return fechaMaximaDevolucion;
        }
    }
}
