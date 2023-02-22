using UcomGridView.Data.Entities.Interfaces;

namespace UcomGridView.Data.Entities
{
    public class User : BaseEntity, ITrackable, IDeletable
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string AvatarPath { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
    }
}
