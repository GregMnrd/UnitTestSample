namespace MyProject.Entities
{
    public class StockEntity
    {
        public int Id { get; }
        public virtual int Quantity { get; }

        protected StockEntity()
        {
        }
        
        public StockEntity(int id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
}
