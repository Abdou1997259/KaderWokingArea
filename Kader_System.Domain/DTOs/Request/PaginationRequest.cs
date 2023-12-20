namespace Kader_System.Domain.DTOs.Request;

public class PaginationRequest
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}
