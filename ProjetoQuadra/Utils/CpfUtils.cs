using System.Text.RegularExpressions;

namespace ProjetoQuadra.Utils
{
    public class CpfUtils
    {
        public static string LimparCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return string.Empty;
            }
            return Regex.Replace(cpf, @"[^\d]", "");
        }

        public static bool NomeNaoContemNumero(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return false;
            }
            return !Regex.IsMatch(nome, @"\d");
        }
    }
}
