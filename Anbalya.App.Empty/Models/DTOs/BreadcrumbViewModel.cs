using System.Collections.Generic;

public record BreadcrumbItem(string Text, string? Url);

public record BreadcrumbViewModel(string Title, IReadOnlyList<BreadcrumbItem> Items);
