
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PortadoresService.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
            AllowMultiple = false)]
    public class CpfAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            var cpf = Regex.Replace((string)value, "[^0-9]", "");

            if (cpf.Length != 11 || !ValidaCpf(cpf))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        private bool ValidaCpf(string cpf)
        {
            return Mod11(cpf.Substring(0, 9)) == cpf[9].ToString() && Mod11(cpf.Substring(0, 10)) == cpf[10].ToString();
        }

        private string Mod11(string m)
        {
            var soma = 0;

            for (var i = 0; i < m.Length; i++)
            {
                soma += (m[i] - '0') * (m.Length + 1 - i);
            }

            int resto = soma % 11;

            return (resto < 2 ? 0 : 11 - resto).ToString();
        }
    }
}