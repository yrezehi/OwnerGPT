namespace OwnerGPT.Models.Abstracts.Interfaces
{
    public interface IEntity
    {
        int Id { set; get; }
        List<string> SearchableProperties() => new List<string>() { "" };
    }
}
