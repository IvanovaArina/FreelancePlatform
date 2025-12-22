using MassTransit;
using Minio;
using Minio.DataModel.Args;
using SharedEvents;

public class ProjectDeletedConsumer : IConsumer<ProjectDeleted>
{
    private readonly IMinioClient _minio;

    public ProjectDeletedConsumer(IMinioClient minio)
    {
        _minio = minio;
    }

    public async Task Consume(ConsumeContext<ProjectDeleted> context)
    {
        var msg = context.Message;

        if (string.IsNullOrWhiteSpace(msg.ImageUrl))
            return;

        // Ожидаем формат: http://localhost:9000/portfolio-images/{objectKey}
        var uri = new Uri(msg.ImageUrl);
        var path = uri.AbsolutePath; // "/portfolio-images/final/21.png"
        var prefix = "/portfolio-images/";

        var objectKey = path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? path[prefix.Length..]
            : path.TrimStart('/');

        await _minio.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket("portfolio-images")
            .WithObject(objectKey)
        );
    }
}
