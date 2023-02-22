namespace UcomGridView.Shared.Dtos
{
    public class UserDto 
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Avatar { get; set; }
        public int StatusId { get; set; }
    }
}
