using MassTransit;
using PortfolioService.Data;
using SharedEvents;

namespace PortfolioService.Consumers
{
    public class ImageProcessedConsumer : IConsumer<ImageProcessed>
    {
        private readonly PortfolioDbContext _db;

        public ImageProcessedConsumer(PortfolioDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<ImageProcessed> context)
        {
            var item = await _db.PortfolioItems.FindAsync(context.Message.PortfolioId);
            if (item == null) return;

            item.ImageUrl = context.Message.FinalImageUrl;
            item.ImageStatus = "ready";
            item.TempImageKey = null;

            await _db.SaveChangesAsync();
        }
    }

}
