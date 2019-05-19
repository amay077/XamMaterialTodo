using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamMaterialTodo.DataModels;

namespace XamMaterialTodo.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> ReadAll(bool containsDone);
        Task Update(TodoItem item);
        Task Add(TodoItem item);
        Task Delete(TodoItem item);
    }
}
