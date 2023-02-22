namespace UcomGridView.Shared.Dtos
{
    public class GetUsersDto
    {
        public int Take { get; set; }
        public int Page { get; set; }
        public FilterDto Filter { get; set; }
    }
}
