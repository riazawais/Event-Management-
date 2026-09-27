namespace EventManagementApp.Models;

public class Attendance
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int RegistrationId { get; set; }

    public string ParticipantName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsPresent { get; set; }

    public DateTime? CheckInTime { get; set; }
}