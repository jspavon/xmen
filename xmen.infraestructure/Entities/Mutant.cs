using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xmen.Infrastructure.Entities.Base;

namespace xmen.infraestructure.Entities
{
    public class Mutant : BaseEntity
    {
        public int Id { get; set; }
        public string Adn { get; set; }
        public int Humans { get; set; }
        public int Mutants { get; set; }
        public decimal Radio { get; set; }

    }
}
