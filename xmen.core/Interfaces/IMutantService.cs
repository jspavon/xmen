using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xmen.core.Dtos;

namespace xmen.core.Interfaces
{
    public interface IMutantService
    {
        Task<IEnumerable<MutantResponse>> GetAllAsync(int page, int limit, string orderBy, bool ascending = true);
        (bool status, int id) Post(MutantDto entity);
    }

}
