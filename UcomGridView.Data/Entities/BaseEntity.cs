using UcomGridView.Data.Entities.Interfaces;

namespace UcomGridView.Data.Entities
{
    public class BaseEntity : IEntity<int>
    {
        public int Id { get; set; }
    }
}
