
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PortadoresService.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CpfAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            var cpf = Regex.Replace((string) value, "[^0-9]", "");

            if (cpf.Length != 11 || !ValidaCpf(cpf))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        private bool ValidaCpf(string cpf)
        {
            int soma1=0, soma2=0;

            for (var i=0; i<10; i++)
            {
                int c = cpf[i]-'0';
                
                if (i<9) soma1 += c*(10-i);
                soma2 += c*(11-i);
            }
            
            int c10 = cpf[9]-'0';
            int c11 = cpf[10]-'0';
            
            return ((soma1*10)%11)==c10 && ((soma2*10)%11)==c11;
        }
    }
}