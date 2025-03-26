namespace Server.DTO;

public class PagedDataDTO<T>
{
    public int PageSize { get; set; }
    public int LastItemIndex { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public List<T> Data { get; set; } = new();
}