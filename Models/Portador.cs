using System.ComponentModel.DataAnnotations;

namespace PortadoresService.Models
{
    public class Portador
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Nome { get; set; }
        
        [Required]
        public string Cpf { get; set; }
    }
}