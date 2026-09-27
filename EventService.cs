using EventManagementApp.Models;

namespace EventManagementApp.Services;

public class EventService
{
    private readonly List<Event> _events;
    private readonly List<Registration> _registrations;
    private readonly List<Attendance> _attendance;

    private int _registrationId = 1;
    private int _attendanceId = 1;

    public EventService()
    {
        _events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Tech Innovation Summit 2026",
                Description =
                    "A professional technology summit featuring software engineering, artificial intelligence, cloud computing and modern development practices.",
                Location = "Islamabad Convention Centre",
                StartDate = new DateTime(2026, 10, 10, 9, 0, 0),
                EndDate = new DateTime(2026, 10, 10, 17, 0, 0),
                Capacity = 250,
                Category = "Technology",
                ImageUrl =
                    "https://images.unsplash.com/photo-1540575467063-178a50c2df87?auto=format&fit=crop&w=1200&q=80",
                RegisteredCount = 125,
                IsActive = true
            },

            new Event
            {
                Id = 2,
                Title = "Business Leadership Forum",
                Description =
                    "An interactive business leadership forum covering management, entrepreneurship, strategy and organizational growth.",
                Location = "Pearl Continental Lahore",
                StartDate = new DateTime(2026, 10, 18, 10, 0, 0),
                EndDate = new DateTime(2026, 10, 18, 16, 0, 0),
                Capacity = 180,
                Category = "Business",
                ImageUrl =
                    "https://images.unsplash.com/photo-1556761175-b413da4baf72?auto=format&fit=crop&w=1200&q=80",
                RegisteredCount = 84,
                IsActive = true
            },

            new Event
            {
                Id = 3,
                Title = "Digital Marketing Masterclass",
                Description =
                    "Learn practical digital marketing strategies including SEO, social media marketing, content strategy and analytics.",
                Location = "Lahore Expo Centre",
                StartDate = new DateTime(2026, 11, 5, 11, 0, 0),
                EndDate = new DateTime(2026, 11, 5, 15, 0, 0),
                Capacity = 120,
                Category = "Marketing",
                ImageUrl =
                    "https://images.unsplash.com/photo-1556761175-5973dc0f32e7?auto=format&fit=crop&w=1200&q=80",
                RegisteredCount = 67,
                IsActive = true
            },

            new Event
            {
                Id = 4,
                Title = "AI & Future Technology Conference",
                Description =
                    "Explore artificial intelligence, automation, machine learning and emerging technologies shaping the future.",
                Location = "Karachi Arts Council",
                StartDate = new DateTime(2026, 11, 20, 9, 30, 0),
                EndDate = new DateTime(2026, 11, 20, 18, 0, 0),
                Capacity = 300,
                Category = "AI",
                ImageUrl =
                    "https://images.unsplash.com/photo-1485827404703-89b55fcc595e?auto=format&fit=crop&w=1200&q=80",
                RegisteredCount = 198,
                IsActive = true
            }
        };

        _registrations = new List<Registration>();

        _attendance = new List<Attendance>();
    }

    public List<Event> GetEvents()
    {
        return _events
            .Where(e => e.IsActive)
            .OrderBy(e => e.StartDate)
            .ToList();
    }

    public Event? GetEventById(int id)
    {
        return _events.FirstOrDefault(e => e.Id == id);
    }

    public List<Registration> GetRegistrations(int eventId)
    {
        return _registrations
            .Where(r => r.EventId == eventId)
            .OrderByDescending(r => r.RegisteredAt)
            .ToList();
    }

    public List<Attendance> GetAttendance(int eventId)
    {
        return _attendance
            .Where(a => a.EventId == eventId)
            .OrderBy(a => a.ParticipantName)
            .ToList();
    }

    public bool RegisterParticipant(Registration registration)
    {
        var selectedEvent = GetEventById(registration.EventId);

        if (selectedEvent is null)
            return false;

        if (selectedEvent.IsFull)
            return false;

        var alreadyRegistered = _registrations.Any(r =>
            r.EventId == registration.EventId &&
            r.Email.Equals(
                registration.Email,
                StringComparison.OrdinalIgnoreCase));

        if (alreadyRegistered)
            return false;

        registration.Id = _registrationId++;
        registration.RegisteredAt = DateTime.Now;
        registration.IsConfirmed = true;

        _registrations.Add(registration);

        selectedEvent.RegisteredCount++;

        return true;
    }

    public bool MarkAttendance(int registrationId)
    {
        var registration = _registrations
            .FirstOrDefault(r => r.Id == registrationId);

        if (registration is null)
            return false;

        var existing = _attendance
            .FirstOrDefault(a => a.RegistrationId == registrationId);

        if (existing is not null)
        {
            existing.IsPresent = true;
            existing.CheckInTime = DateTime.Now;

            return true;
        }

        _attendance.Add(new Attendance
        {
            Id = _attendanceId++,
            EventId = registration.EventId,
            RegistrationId = registration.Id,
            ParticipantName = registration.FullName,
            Email = registration.Email,
            IsPresent = true,
            CheckInTime = DateTime.Now
        });

        return true;
    }

    public int GetTotalEvents()
    {
        return _events.Count(e => e.IsActive);
    }

    public int GetTotalRegistrations()
    {
        return _events.Sum(e => e.RegisteredCount);
    }

    public int GetTotalCapacity()
    {
        return _events.Sum(e => e.Capacity);
    }

    public int GetTotalAttendance()
    {
        return _attendance.Count(a => a.IsPresent);
    }
}