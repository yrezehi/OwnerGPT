namespace OwnerGPT.Models.Entities.Interfaces
{
    public interface IEntity {
        int Id { set; get; }
        List<string> SearchableProperties() => new List<string>() { "" };
    }
}
