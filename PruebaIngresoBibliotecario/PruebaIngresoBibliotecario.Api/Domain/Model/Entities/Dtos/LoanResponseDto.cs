using System;

namespace PruebaIngresoBibliotecario.Api.Domain.Model.Entities.Dtos
{
    public class LoanResponseDto
    {
        public Guid Id { get; set; }
        public string Isbn { get; set; }
        public string IdentificacionUsuario { get; set; }
        public int TipoUsuario { get; set; }
        public DateTime FechaMaximaDevolucion { get; set; }
    }
}
