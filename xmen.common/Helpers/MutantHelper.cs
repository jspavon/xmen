using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmen.common.Helpers
{
    public static class MutantHelper
    {
        public static bool AreEqualDna(string a, string b, string c, string d)
        {
            ValidateMatriz(a, b, c, d);
            var comparativo = a.ToLower() == b.ToLower() & b.ToLower() == c.ToLower() & c.ToLower() == d.ToLower();
            return comparativo;
        }

        private static void ValidateMatriz(string a, string b, string c, string d)
        {
            var validSecuence = "acgt";

            if (!validSecuence.Contains(a.ToLower()) || !validSecuence.Contains(b.ToLower()) || !validSecuence.Contains(c.ToLower()) || !validSecuence.Contains(d.ToLower())) throw new Exception("Secuence no valid");
        }

    }
}
