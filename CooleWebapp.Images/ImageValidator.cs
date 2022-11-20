using CooleWebapp.Application.ImageService;
using SixLabors.ImageSharp;

namespace CooleWebapp.Images;

internal class ImageValidator : IImageValidator
{
  public Task<bool> ValidateImage(byte[] image, CancellationToken ct)
  {
    try
    {
      return Task.FromResult(Image.Identify(image) is not null);
    }
    catch(Exception)
    {
      return Task.FromResult(false);
    }
  }
}
