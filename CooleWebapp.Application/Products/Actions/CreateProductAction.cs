using CooleWebapp.Application.ImageService;
using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Application.Products.Actions
{
  internal class CreateProductAction : IBusinessAction<CreateProductDto, Int64>
  {
    private readonly IProductDataAccess _productDataAccess;
    private readonly IImageValidator _imageValidator;
    public CreateProductAction(
      IProductDataAccess productDataAccess,
      IImageValidator imageValidator)
    {
      _productDataAccess = productDataAccess;
      _imageValidator = imageValidator;
    }

    public async Task<Int64> Run(CreateProductDto dataIn, CancellationToken ct)
    {
      if (dataIn.Image != null && !await _imageValidator.ValidateImage(dataIn.Image, ct))
        throw new ClientError(ErrorType.InvalidOperation, "The supplied image was invalid.");
      return await _productDataAccess.CreateProduct(new()
      {
        Name = dataIn.Name,
        Description = dataIn.Description,
        Price = dataIn.Price,
        State = dataIn.State,
        ProductImage = dataIn.Image is null ? null : new() { Data = dataIn.Image }
      }, ct);
    }
  }
}
