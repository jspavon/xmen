using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmen.Infrastructure.Repository
{
    /// <summary>
    /// The repository factory.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// The repository.
        /// </returns>
        IRepositoryData<TEntity> CreateRepository<TEntity>()
            where TEntity : class;
    }
}
