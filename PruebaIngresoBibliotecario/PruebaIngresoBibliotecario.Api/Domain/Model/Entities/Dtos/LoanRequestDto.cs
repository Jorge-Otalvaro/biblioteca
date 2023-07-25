namespace PruebaIngresoBibliotecario.Api.Domain.Model.Entities.Dtos
{
    public class LoanRequestDto
    {
        public string Isbn { get; set; }
        public string IdentificacionUsuario { get; set; }
        public int TipoUsuario { get; set; }
    }
}
