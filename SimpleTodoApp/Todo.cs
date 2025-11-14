using System;
using System.Text;

namespace SimpleTodoApp
{
    public class Todo
    {
        private const int MaxTitleLength = 100;
        private const int MaxDescLength = 500;

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; }
        public int Completed { get; set; }
        public long CreatedAt { get; set; }  // Unix timestamp (seconds since epoch)
        public long CompletedAt { get; set; }  // Unix timestamp (seconds since epoch)

        public DateTime CreatedAtDateTime
        {
            get => CreatedAt == 0 ? DateTime.MinValue : DateTimeOffset.FromUnixTimeSeconds(CreatedAt).DateTime;
            set => CreatedAt = value == DateTime.MinValue ? 0 : new DateTimeOffset(value).ToUnixTimeSeconds();
        }

        public DateTime? CompletedAtDateTime
        {
            get => CompletedAt == 0 ? null : (DateTime?)DateTimeOffset.FromUnixTimeSeconds(CompletedAt).DateTime;
            set => CompletedAt = value.HasValue ? new DateTimeOffset(value.Value).ToUnixTimeSeconds() : 0;
        }

        public bool IsCompleted => Completed == 1;
    }

    public class TodoList
    {
        public Todo[] Todos { get; set; } = new Todo[100];
        public int Count { get; set; }
    }
}

