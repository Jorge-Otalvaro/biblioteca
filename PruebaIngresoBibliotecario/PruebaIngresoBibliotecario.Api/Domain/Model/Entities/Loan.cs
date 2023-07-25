using System;
using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Api.Domain.Model.Entities
{
    public class Loan
    {
        public Guid Id { get; set; }

        [Required]        
        public string Isbn { get; set; }

        [Required]
        [MaxLength(10), MinLength(1)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string IdentificacionUsuario { get; set; }

        [Required]
        public TipoUsuarioEnum TipoUsuario { get; set; }        

        [Required]
        public string FechaMaximaDevolucion { get; set; }

        // 1: usuario afilado
        // 2: usuario empleado de la biblioteca
        // 3: usuario invitado

        public enum TipoUsuarioEnum
        {
            Afiliado = 1,
            Empleado = 2,
            Invitado = 3
        }
    }
}
