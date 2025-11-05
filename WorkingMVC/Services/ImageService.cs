using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using WorkingMVC.Interfaces;

namespace WorkingMVC.Services
{
    public class ImageService(IConfiguration configuration) : IImageService
    {
        public async Task<string> UploadImageAsync(IFormFile file)
        {

            try
            {
                using MemoryStream memoryStream = new();
                await file.CopyToAsync(memoryStream);
                string fileName =  Guid.NewGuid().ToString() + ".webp";
                var dirPath = configuration.GetValue<string>("DirPath");
                var fullDirPath = Path.Combine(Directory.GetCurrentDirectory(), dirPath);
                var filePath = Path.Combine(fullDirPath, fileName);
                var bytes = memoryStream.ToArray();
                using var image = Image.Load(bytes);
                image.Mutate(imgc =>
                {
                    imgc.Resize(new ResizeOptions
                    {
                        Size = new Size(500, 500),
                        Mode = ResizeMode.Max
                    });
                });
                
                await image.SaveAsync(filePath, new WebpEncoder());
                return $"/{dirPath}/{fileName}" ;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
    }
}
