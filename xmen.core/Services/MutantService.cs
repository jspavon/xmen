using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xmen.core.Dtos;
using xmen.core.Interfaces;
using System.Text.RegularExpressions;
using xmen.Infrastructure.Repository;
using xmen.infraestructure.Entities;
using xmen.Infrastructure.UnitOfWork;
using AutoMapper;
using xmen.common.Constants;
using Newtonsoft.Json;
using xmen.common.Helpers;

namespace xmen.core.Services
{
    public class MutantService : IMutantService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryData<Mutant> _repository;
        /// <summary>
        /// the IUnitOfWork
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MutantService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentNullException">mapper</exception>
        public MutantService(IRepositoryData<Mutant> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = unitOfWork.CreateRepository<Mutant>();
            _unitOfWork = unitOfWork;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<MutantResponse>> GetAllAsync(int page, int limit, string orderBy, bool ascending = true)
        {
            var result = _repository.GetAllAsync(x => !x.Deleted, page, limit, orderBy, ascending).Result.OrderBy(x => x.Id);
            var mapper = _mapper.Map<IEnumerable<MutantResponse>>(result);

            return mapper;
        }

        public (bool status, int id) Post(MutantDto entity)
        {

            var getAll = _repository.GetAllAsync(x => !x.Deleted).Result.OrderBy(x => x.Id).ToList();
            if(getAll.Count > 0)
            {
                string adnComparer = JsonConvert.SerializeObject(entity.adn);

                foreach (var adn in getAll)
                {
                    if (adn.Adn == adnComparer) throw new Exception("Secuence no valid");
                }
            }

            var (humans, mutants) = getHumansMutants(entity);

            Mutant obj = new();

            obj.Adn = JsonConvert.SerializeObject(entity.adn);
            obj.Humans = humans;
            obj.Mutants = mutants;
            obj.Radio = humans / mutants;
            obj.CreationUser = GenericConstant.GENERIC_USER;
            obj.CreationDate = DateTime.Now;

            if (mutants > 0) Insert(obj);

            var result = _unitOfWork.Save();

            return (result > 0, mutants); ;
        }

        public (bool status, int id) Insert(Mutant entity)
        {
            _repository.Insert(entity);
            var result = _unitOfWork.Save() > 0;
            return (result, entity.Id);
        }

        public (int, int) getHumansMutants(MutantDto entity) 
        {
            int mutantAdn = 0;
            int humanAdn = 0;

            for (int i = 0; i < entity.adn.Length; i++)
            {
                for (int j = 0; j < entity.adn[i].Length; j++)
                {
                    // Chequeo combinaciones en forma horizontal
                    if (j < entity.adn[i].Length - 3)
                    {
                        if (MutantHelper.AreEqualDna(entity.adn[i].Substring(j, 1), entity.adn[i].Substring(j + 1, 1), entity.adn[i].Substring(j + 2, 1), entity.adn[i].Substring(j + 3, 1)))
                        {
                            mutantAdn++;
                        }
                        else 
                        {
                            humanAdn++;
                        }
                    }

                    // Chequeo combinaciones en forma Vertical
                    if (i < entity.adn[i].Length - 3)
                    {
                        if (MutantHelper.AreEqualDna(entity.adn[i].Substring(j, 1), entity.adn[i + 1].Substring(j, 1), entity.adn[i + 2].Substring(j, 1), entity.adn[i + 3].Substring(j, 1)))
                        {
                            mutantAdn++;
                        }
                        else
                        {
                            humanAdn++;
                        }
                    }

                    // Chequeo combinaciones en forma Oblicua, Derecha a izquierda & Arriba a abajo
                    if (j < entity.adn[i].Length - 3 && i < entity.adn[i].Length - 3)
                    {
                        if (MutantHelper.AreEqualDna(entity.adn[i].Substring(j, 1), entity.adn[i + 1].Substring(j + 1, 1), entity.adn[i + 2].Substring(j + 2, 1), entity.adn[i + 3].Substring(j + 3, 1)))
                        {
                            mutantAdn++;
                        }
                        else
                        {
                            humanAdn++;
                        }
                    }

                    // Chequeo combinaciones en forma Oblicua, Izquierda a derecha & Abajo a arriba
                    if (entity.adn[i].Length > 3 && j < entity.adn[i].Length - 3 && i > 2)
                    {
                        if (MutantHelper.AreEqualDna(entity.adn[i].Substring(j, 1), entity.adn[i - 1].Substring(j + 1, 1), entity.adn[i - 2].Substring(j + 2, 1), entity.adn[i - 3].Substring(j + 3, 1)))
                        {
                            mutantAdn++;
                        }
                        else
                        {
                            humanAdn++;
                        }
                    }
                }
            }

            return (humanAdn, mutantAdn);
        }

    }
}
