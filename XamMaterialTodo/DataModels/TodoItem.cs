using System;
namespace XamMaterialTodo.DataModels
{
    public sealed class TodoItem
    {
        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public int Priority { get; }
        public bool IsCompleted { get; }
        public DateTimeOffset? DueDate { get; }

        public TodoItem(string id, string title, bool isCompleted, string description, int priority, DateTimeOffset? dueDate)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Priority = priority;
            this.IsCompleted = isCompleted;
            this.DueDate = dueDate;
        }

        public TodoItem MakeCompleted(bool isCompleted)
        {
            return new TodoItem(Id, Title, isCompleted, Description, Priority, DueDate);
        }
    }
}
