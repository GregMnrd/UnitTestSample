namespace MyProject.Entities
{
    public class ProductEntity
    {
        public int Id { get; }
        public InfoProductEntity? InfoProductEntity { get; }
        public virtual StockEntity? StockEntity { get; }
        public bool IsAvailable { get; set; }

        protected ProductEntity()
        {
        }

        public ProductEntity(int id, InfoProductEntity? infoProductEntity, StockEntity? stockEntity)
        {
            Id = id;
            InfoProductEntity = infoProductEntity;
            StockEntity = stockEntity;
        }


    }
}
