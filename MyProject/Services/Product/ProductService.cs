using MyProject.Entities;
using MyProject.Repositories;

namespace MyProject.Services.Product
{
    internal class ProductService : IProductService
    {
        public IInfoProductRepository InfoProductRepository { get; }
        public IStockRepository StockRepository { get; }

        public ProductService(IInfoProductRepository infoProductRepository, IStockRepository stockRepository)
        {
            InfoProductRepository = infoProductRepository;
            StockRepository = stockRepository;
        }

        public virtual ProductEntity? GetProductById(int id)
        {
            // Throw exception if id <= 0
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            StockEntity? stockEntity = null;
            bool isAvailable = false;

            // Get Info Product
            InfoProductEntity? infoProductEntity = this.InfoProductRepository.GetInfoProductById(id);

            // Get Stock if info product exist
            if (infoProductEntity != null)
            {
                // Get Stock
                stockEntity = this.StockRepository.GetStockById(id);
                
                // Compute availability
                isAvailable = stockEntity?.Quantity > 0;
            }

            // Build prdouct
            ProductEntity product = new(id, infoProductEntity, stockEntity);
            // Set availability
            product.IsAvailable = isAvailable;

            // Return product
            return product;
        }

        public List<ProductEntity?> GetAvailabilityOfTwoProductsById(int idProduct1, int idProduct2)
        {
            if (IsProductEnabled(idProduct1))
            {
                return new List<ProductEntity?>() { this.GetProductById(idProduct1), this.GetProductById(idProduct2) };
            }
            else
            {
                return new List<ProductEntity?>() { };
            }
        }

        protected virtual bool IsProductEnabled(int id)
        {
            return false;
        }
    }
}
