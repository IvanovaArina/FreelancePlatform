using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase
{
    private readonly IMinioClient _minio;

    public ImageController(IMinioClient minio)
    {
        _minio = minio;
    }

    [HttpPost("upload-temp")]
    public async Task<IActionResult> UploadTemp(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file");

        var key = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var objectName = $"temp/{key}";

        using var stream = file.OpenReadStream();

        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket("portfolio-images")
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
        );

        return Ok(new { tempImageKey = key });
    }
}
