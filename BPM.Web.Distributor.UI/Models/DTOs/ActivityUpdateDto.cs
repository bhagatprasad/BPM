public class ActivityUpdateDto
{
    public Guid ActivityId { get; set; }

    public string ActivityName { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}