using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmen.core.Dtos
{
    public class MutantDto
    {
        public string[] adn { get; set; }

    }

    public class MutantResponse 
    {
        public int Id { get; set; }
        public string Adn { get; set; }
        public int Humans { get; set; }
        public int Mutants { get; set; }
        public decimal Radio { get; set; }
    }
}
