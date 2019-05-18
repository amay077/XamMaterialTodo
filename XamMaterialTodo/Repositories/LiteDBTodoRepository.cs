using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamMaterialTodo.DataModels;

namespace XamMaterialTodo.Repositories
{
    public class LiteDBTodoRepository
    {
        private readonly IList<TodoItem> source = new List<TodoItem>();

        public LiteDBTodoRepository()
        {
            source.Add(new TodoItem("id1", "title1", true, "desc1", 1, DateTimeOffset.Now + TimeSpan.FromDays(5)));
            source.Add(new TodoItem("id2", "title2", false, "desc2", 2, null));
        }

        public async Task<IEnumerable<TodoItem>> ReadAll(bool containsCompleted)
        {
            return source.Where(x => containsCompleted ? true : !x.IsCompleted);
        }

        public Task Update(TodoItem item)
        {
            return Task.Factory.StartNew(() =>
            {
                var hit = source.Select((todo, index) => (todo, index)).FirstOrDefault(x => x.todo.Id == item.Id);
                source[hit.index] = item;
            });
        }

        public Task Add(TodoItem item)
        {
            return Task.Factory.StartNew(() =>
            {
                source.Add(item);
            });
        }
    }
}
