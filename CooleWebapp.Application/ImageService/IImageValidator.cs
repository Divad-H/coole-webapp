namespace CooleWebapp.Application.ImageService;

public interface IImageValidator
{
  Task<bool> ValidateImage(byte[] image, CancellationToken ct);
}
