using MyProject.Entities;

namespace MyProject.Services.Product
{
    public interface IProductService
    {
        ProductEntity? GetProductById(int id);
        List<ProductEntity?> GetAvailabilityOfTwoProductsById(int idProduct1, int idProduct2);
    }
}
