using System.ComponentModel.DataAnnotations;
using PortadoresService.DataAnnotations;

namespace PortadoresService.Dtos
{
    public class PortadorCreateDto
    {
        [Required]
        public string Nome { get; set; }
        
        [Cpf]
        [Required]
        public string Cpf { get; set; }
    }
}