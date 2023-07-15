using System;
using System.Text.Json;

namespace Construmart.Core.Domain.Events
{
    public class Event
    {
        public Event(string message)
        {
            EventId = Guid.NewGuid();
            EventRaisedAt = DateTime.UtcNow;
            Message = message;
        }
        public Guid EventId { get; private set; }
        public DateTime EventRaisedAt { get; private set; }
        public string Message { get; private set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}