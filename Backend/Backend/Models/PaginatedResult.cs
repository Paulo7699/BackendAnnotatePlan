﻿namespace Backend.Models;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? NextPageToken { get; set; }
    public int TotalItems { get; set; }
}
