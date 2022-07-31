using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xmen.api.Models.Abstracts
{
    public class MutantModelValidator : AbstractValidator<MutantModel>
    {
        public MutantModelValidator() 
        {
            RuleFor(x => x.adn)
               .NotNull()
               .NotEmpty();
        }
    }
}
