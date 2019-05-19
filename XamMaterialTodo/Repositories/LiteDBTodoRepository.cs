using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using XamMaterialTodo.DataModels;

namespace XamMaterialTodo.Repositories
{
    public sealed class LiteDbTodoRepository : ITodoRepository
    {
        private readonly string dataBasePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal), "todo.litedb");

        public Task<IEnumerable<TodoItem>> ReadAll(bool containsDone)
        {
            return Task.Factory.StartNew<IEnumerable<TodoItem>>(() => 
            {
                using (var db = new LiteDatabase(dataBasePath))
                {
                    var items = db.GetCollection<TodoItem>();
                    return items.FindAll()
                        .Where(x => containsDone ? true : !x.IsDone)
                        .OrderByDescending(x => x.Priority)
                        .ThenByDescending(x => x.CreateDate);
                }
            });
        }

        public Task Update(TodoItem item)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new LiteDatabase(dataBasePath))
                {
                    var items = db.GetCollection<TodoItem>();
                    items.Update(item.Id, item);
                }
            });
        }

        public Task Add(TodoItem item)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new LiteDatabase(dataBasePath))
                {
                    var items = db.GetCollection<TodoItem>();
                    items.Insert(item.Id, item);
                }
            });
        }

        public Task Delete(TodoItem item)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new LiteDatabase(dataBasePath))
                {
                    var items = db.GetCollection<TodoItem>();
                    items.Delete(item.Id);
                }
            });
        }
    }
}
