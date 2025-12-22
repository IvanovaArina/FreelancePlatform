using MassTransit;
using ProfileService.Models.Domain;
using ProfileService.Repositories.Interfaces;
using SharedEvents;

namespace ProfileService.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegistered>
{
    private readonly IFreelancerProfileRepository _profiles;

    public UserRegisteredConsumer(IFreelancerProfileRepository profiles)
    {
        _profiles = profiles;
    }

    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
        
        if (context.Message.Role != "freelancer")
            return;

        var existing = await _profiles.GetByUserIdAsync(context.Message.UserId);
        if (existing != null) return;

        await _profiles.AddAsync(new FreelancerProfile
        {
            UserId = context.Message.UserId
        });
    }
}
