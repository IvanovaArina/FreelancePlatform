using SharedEvents;
using FreelancePlatformConnect.Api.Data;
using MassTransit;

public class ImageProcessedConsumer : IConsumer<ImageProcessed>
{
    private readonly AppDbContext _db;

    public ImageProcessedConsumer(AppDbContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<ImageProcessed> context)
    {
        var msg = context.Message;

        var item = await _db.PortfolioItems.FindAsync(msg.PortfolioId);
        if (item == null) return;

        item.ImageUrl = msg.FinalImageUrl;
        item.ImageStatus = "ready";
        item.TempImageKey = null;

        await _db.SaveChangesAsync();
    }
}
