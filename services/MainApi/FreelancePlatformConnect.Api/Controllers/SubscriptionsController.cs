using AutoMapper;
using FreelancePlatformConnect.Api.Models.Domain;
using FreelancePlatformConnect.Api.Models.DTOs;
using FreelancePlatformConnect.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/subscriptions")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionRepository _repo;
    private readonly IMapper _mapper;

    public SubscriptionsController(ISubscriptionRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    // ВСЕ МОГУТ: посмотреть подписки любого фрилансера
    [HttpGet("freelancer/{freelancerId}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<SubscriptionDto>>> GetByFreelancerId(int freelancerId)
    {
        var subs = await _repo.GetByFreelancerIdAsync(freelancerId);
        return Ok(_mapper.Map<List<SubscriptionDto>>(subs));
    }

    // ТОЛЬКО ФРИЛАНСЕР: получить СВОИ подписки (по токену, без ID в URL!)
    [HttpGet("my")]
    [Authorize(Roles = "freelancer")]
    public async Task<ActionResult<List<SubscriptionDto>>> GetMySubscriptions()
    {
        var subs = await _repo.GetByFreelancerIdAsync(UserId);
        return Ok(_mapper.Map<List<SubscriptionDto>>(subs));
    }

    // ТОЛЬКО ФРИЛАНСЕР: создать свою подписку
    [HttpPost]
    [Authorize(Roles = "freelancer")]
    public async Task<ActionResult<SubscriptionDto>> Create(CreateSubscriptionRequest req)
    {
        var sub = new Subscription
        {
            FreelancerId = UserId,
            Name = req.Name,
            Description = req.Description,
            Price = req.Price,
            WhatIncludes = req.WhatIncludes
        };

        await _repo.AddAsync(sub);
        return CreatedAtAction(nameof(GetOne), new { id = sub.Id }, _mapper.Map<SubscriptionDto>(sub));
    }

    // ВСЕ МОГУТ: получить одну подписку по ID
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<SubscriptionDto>> GetOne(int id)
    {
        var sub = await _repo.GetByIdAsync(id);
        if (sub == null) return NotFound();
        return Ok(_mapper.Map<SubscriptionDto>(sub));
    }

    // ТОЛЬКО ВЛАДЕЛЕЦ: обновить
    [HttpPut("{id}")]
    [Authorize(Roles = "freelancer")]
    public async Task<IActionResult> Update(int id, UpdateSubscriptionRequest req)
    {
        var sub = await _repo.GetByIdAsync(id);
        if (sub == null || sub.FreelancerId != UserId) return Forbid();

        sub.Name = req.Name;
        sub.Description = req.Description;
        sub.Price = req.Price;
        sub.IsActive = req.IsActive;
        sub.WhatIncludes = req.WhatIncludes;

        await _repo.UpdateAsync(sub);
        return NoContent();
    }

    // ТОЛЬКО ВЛАДЕЛЕЦ: удалить
    [HttpDelete("{id}")]
    [Authorize(Roles = "freelancer")]
    public async Task<IActionResult> Delete(int id)
    {
        var sub = await _repo.GetByIdAsync(id);
        if (sub == null || sub.FreelancerId != UserId) return Forbid();

        await _repo.DeleteAsync(sub);
        return NoContent();
    }
}