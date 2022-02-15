using System.ComponentModel.DataAnnotations;

namespace PortadoresService.Dtos
{
    public class PortadorCreateDto
    {
        [Required]
        public string Nome { get; set; }
        
        [Required]
        public string Cpf { get; set; }
    }
}