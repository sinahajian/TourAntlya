using System.ComponentModel.DataAnnotations;

public class ContactRequestInputModel
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Website { get; set; }

    public string? Language { get; set; }
}
