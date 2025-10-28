using System;
using System.Collections.Generic;
using Models.Entities;

public class ContactMessageListViewModel
{
    public string UserName { get; set; } = string.Empty;
    public List<ContactMessageItemViewModel> Messages { get; set; } = new();
    public string? FeedbackMessage { get; set; }
}

public class ContactMessageItemViewModel
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? Language { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public static ContactMessageItemViewModel FromEntity(ContactMessage entity)
    {
        return new ContactMessageItemViewModel
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            Message = entity.Message,
            Status = entity.Status.ToString(),
            Language = entity.Language,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(entity.CreationTime).UtcDateTime,
            UpdatedAt = entity.UpdatedTime == 0 ? (DateTime?)null : DateTimeOffset.FromUnixTimeSeconds(entity.UpdatedTime).UtcDateTime
        };
    }
}
