namespace WorkingMVC.Interfaces
{
    public interface IImageService
    {
        public Task<string> UploadImageAsync(IFormFile imageFile);
        public Task DeleteImageAsync(string imagePath);
    }
}
