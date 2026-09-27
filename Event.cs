namespace EventManagementApp.Models;

public class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Capacity { get; set; }

    public string Category { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public int RegisteredCount { get; set; }

    public int AvailableSeats => Math.Max(0, Capacity - RegisteredCount);

    public bool IsFull => RegisteredCount >= Capacity;
}