using OwnerGPT.Models.Abstracts.Interfaces;

namespace OwnerGPT.Models
{
    public class Notification : IEntity
    {
        public int Id { get; set; }

        public List<string> SearchableProperties()
        {
            throw new NotImplementedException();
        }
    }
}
