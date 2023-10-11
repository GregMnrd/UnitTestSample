using MyProject.Entities;

namespace MyProject.Repositories
{
    public class InfoProductRepository : IInfoProductRepository
    {
        public InfoProductEntity? GetInfoProductById(int id)
        {
            // DATABASE CALL
            throw new NotImplementedException($"Mon message: {id}");
        }
    }
}
