using MyProject.Entities;

namespace MyProject.Repositories
{
    internal interface IStockRepository
    {
        StockEntity? GetStockById(int id);
    }
}
