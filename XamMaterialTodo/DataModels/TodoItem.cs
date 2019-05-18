using System;
namespace XamMaterialTodo.DataModels
{
    public sealed class TodoItem
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Priority { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTimeOffset? DueDate { get; private set; }
        public DateTimeOffset CreateDate { get; private set; }

        private TodoItem()
        {
        }

        public TodoItem(string id, string title, bool isCompleted, string description, int priority, DateTimeOffset? dueDate, DateTimeOffset createDate)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Priority = priority;
            this.IsCompleted = isCompleted;
            this.DueDate = dueDate;
            this.CreateDate = createDate;
        }

        public TodoItem MakeCompleted(bool isCompleted)
        {
            return new TodoItem(Id, Title, isCompleted, Description, Priority, DueDate, CreateDate);
        }
    }
}
