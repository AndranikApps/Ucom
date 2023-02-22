namespace UcomGridView.Shared.Dtos
{
    public class FilterDto
    {
        public string SearchFilter { get; set; }
        public OrderFilter Sorting { get; set; }
    }

    public class OrderFilter
    {
        public string ColumnName { get; set; }
        public string OrderBy { get; set; }
    }
}
