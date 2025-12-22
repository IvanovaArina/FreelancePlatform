using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioService.Models.Domain;
using PortfolioService.Models.DTOs;
using PortfolioService.Repoositories.Interfaces;
using SharedEvents;
using System.Security.Claims;

namespace PortfolioService.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [Authorize(Roles = "freelancer")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioRepository _repo;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publish;

        public PortfolioController(IPortfolioRepository repo, IMapper mapper, IPublishEndpoint publish)
        {
            _repo = repo;
            _mapper = mapper;
            _publish = publish;
        }

        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        public async Task<ActionResult<List<PortfolioItemDto>>> GetMyPortfolio()
            => Ok(_mapper.Map<List<PortfolioItemDto>>(await _repo.GetByFreelancerIdAsync(UserId)));

        [HttpPost]
        public async Task<ActionResult<PortfolioItemDto>> Create(CreatePortfolioItemRequest req)
        {
            var item = new PortfolioItem
            {
                FreelancerId = UserId,
                Title = req.Title,
                Description = req.Description,
                TempImageKey = req.TempImageKey,
                ImageStatus = req.TempImageKey != null ? "pending" : "none",
                ImageUrl = null
            };

            await _repo.AddAsync(item);

            if (req.TempImageKey != null)
                await _publish.Publish(new ProjectImageUploaded(item.Id, req.TempImageKey));

            return CreatedAtAction(nameof(GetOne), new { id = item.Id }, _mapper.Map<PortfolioItemDto>(item));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PortfolioItemDto>> GetOne(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null || item.FreelancerId != UserId) return Forbid();
            return Ok(_mapper.Map<PortfolioItemDto>(item));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePortfolioItemRequest req)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null || item.FreelancerId != UserId) return Forbid();

            item.Title = req.Title;
            item.Description = req.Description;

            if (req.TempImageKey != null)
            {
                item.TempImageKey = req.TempImageKey;
                item.ImageUrl = null;
                item.ImageStatus = "pending";

                await _publish.Publish(new ProjectImageUploaded(item.Id, req.TempImageKey));
            }

            await _repo.UpdateAsync(item);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null || item.FreelancerId != UserId)
                return Forbid();

            var imageUrl = item.ImageUrl; // запомним, чтобы передать в событии

            await _repo.DeleteAsync(item);

            await _publish.Publish(new ProjectDeleted(id, imageUrl));

            return NoContent();
        }
    }
}

