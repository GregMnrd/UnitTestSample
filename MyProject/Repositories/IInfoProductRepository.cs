using MyProject.Entities;

namespace MyProject.Repositories
{
    public interface IInfoProductRepository
    {
        InfoProductEntity? GetInfoProductById(int id);
    }
}
