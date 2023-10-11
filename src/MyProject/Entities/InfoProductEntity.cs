namespace MyProject.Entities
{
    public class InfoProductEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public InfoProductEntity(int id, string? name)
        {
            Id = id;
            Name = name;
        }
    }
}
