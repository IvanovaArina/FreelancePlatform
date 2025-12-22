using MassTransit;
using Minio;
using Minio.DataModel.Args;
using SharedEvents;

public class ProjectImageUploadedConsumer : IConsumer<ProjectImageUploaded>
{
    private readonly IMinioClient _minio;
    private readonly IPublishEndpoint _publish;

    public ProjectImageUploadedConsumer(IMinioClient minio, IPublishEndpoint publish)
    {
        _minio = minio;
        _publish = publish;
    }

    public async Task Consume(ConsumeContext<ProjectImageUploaded> context)
    {
        var msg = context.Message;

        string bucket = "portfolio-images";
        string tempObject = $"temp/{msg.TempImageKey}";
        string ext = Path.GetExtension(msg.TempImageKey);
        string finalObject = $"final/{msg.PortfolioId}{ext}";

        // 1. Скачиваем временный файл в память
        using var tempStream = new MemoryStream();

        await _minio.GetObjectAsync(new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(tempObject)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(tempStream);
            }));

        tempStream.Position = 0;

        // 2. Загружаем файл в final/
        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(finalObject)
            .WithStreamData(tempStream)
            .WithObjectSize(tempStream.Length));

        // 3. Удаляем temp файл
        await _minio.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(tempObject));

        // 4. Формируем URL
        var url = $"http://localhost:9000/{bucket}/{finalObject}";

        // 5. Отправляем событие обратно в MainApi
        await _publish.Publish(new ImageProcessed(msg.PortfolioId, url));
    }
}
