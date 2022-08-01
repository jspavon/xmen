using AutoMapper;
using xmen.api.Models;
using xmen.common.Constants;
using xmen.common.Dtos;
using xmen.common.Enums;
using xmen.common.Enums.Exts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using xmen.core.Interfaces;
using xmen.core.Dtos;

namespace xmen.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutantController : ControllerBase
    {
        /// <summary>
        /// The integration sap service
        /// </summary>
        private readonly IMutantService _service;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<MutantController> _logger;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The browser detector
        /// </summary>
        private readonly IBrowserDetector _browserDetector;


        public MutantController(ILogger<MutantController> logger, IMapper mapper, IBrowserDetector browserDetector, IMutantService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _browserDetector = browserDetector ?? throw new ArgumentNullException(nameof(browserDetector));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }


        /// <summary>
        /// get all as an asynchronous operation.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("stats")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseService<IEnumerable<MutantResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            _logger.LogInformation(nameof(GetAllAsync));

            var result = await _service.GetAllAsync(1, 1000, "Id").ConfigureAwait(false);

            var mapper = (result != null && result.Count() > 0) ? result.ToList() : new List<MutantResponse>();

            
            var response = new ResponseService<IEnumerable<MutantResponse>>
            {
                Status = mapper.Any(),
                Message = mapper.Any() ? GenericEnumerator.Status.Ok.ToStringAttribute() : GenericEnumerator.Status.Error.ToStringAttribute(),
                Data = mapper
            };
            return Ok(response);
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseService<string>), (int)HttpStatusCode.Created)]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(MutantDto))]
        public IActionResult Post([FromBody] MutantModel request)
        {
            _logger.LogInformation(nameof(Post));

            var objRequest = _mapper.Map<MutantDto>(request);
              
            var (status, id) = _service.Post(objRequest);

            return Ok(new ResponseService<int>
            {
                Status = status,
                Data = status ? id : default
            });
        }

    }
}
