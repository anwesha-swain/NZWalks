using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //create walk
        //POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //map dto to domain model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);

            //map domain model to dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        
        }

        //get the walks
        //GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //domain model to dto
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }
    

        //get walk by id
        //Get: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walksDomainModel = await walkRepository.GetByIdAsync(id);
            if(walksDomainModel == null)
            {
                return NotFound();
            }

            //map domain model to dto
            return Ok(mapper.Map<WalkDto>(walksDomainModel));
        }

        //update walk
        //PUT: /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            //map dto to domain model
            var walksDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            walksDomainModel = await walkRepository.UpdateAsync(id, walksDomainModel);

            if (walksDomainModel == null)
            {
                return NotFound();
            }

            //map domain model to dto
            return Ok(mapper.Map<WalkDto>(walksDomainModel));
        }

        //delete walk
        //DELETE: /api/walks/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
            if(deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            //map domain model to dto
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
