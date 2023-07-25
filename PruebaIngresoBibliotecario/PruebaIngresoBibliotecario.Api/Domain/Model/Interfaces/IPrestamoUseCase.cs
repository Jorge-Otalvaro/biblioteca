using PruebaIngresoBibliotecario.Api.Domain.Model.Entities;
using PruebaIngresoBibliotecario.Api.Domain.Model.Entities.Dtos;
using System;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Domain.Model.Interfaces
{
    public interface IPrestamoUseCase
    {
        Task<Loan> CreateLoan(LoanRequestDto loanRequest);

        Task<bool> LoanExistsForUser(string identificacionUsuario);

        Task<LoanResponseDto> GetLoanById(Guid idPrestamo);
    }
}
