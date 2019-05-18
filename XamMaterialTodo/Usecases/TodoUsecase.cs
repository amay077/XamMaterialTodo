using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Reactive.Bindings;
using XamMaterialTodo.DataModels;
using XamMaterialTodo.Repositories;

namespace XamMaterialTodo.Usecases
{
    public sealed class TodoUsecase
    {
        private readonly LiteDBTodoRepository todoRepository;

        private ReactiveCollection<TodoItem> TodoItemsInternal { get; } = new ReactiveCollection<TodoItem>();
        public ReadOnlyReactiveCollection<TodoItem> TodoItems { get; }

        private ReactiveProperty<bool> IsVisibleCompletedInternal { get; } = new ReactiveProperty<bool>(false);
        public ReadOnlyReactiveProperty<bool> IsVisibleCompleted { get; }

        public TodoUsecase(LiteDBTodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;


            IsVisibleCompleted = IsVisibleCompletedInternal.ToReadOnlyReactiveProperty();
            TodoItems = TodoItemsInternal.ToReadOnlyReactiveCollection();
        }

        public void ToggleShowCompleted()
        {
            IsVisibleCompletedInternal.Value = !IsVisibleCompletedInternal.Value;
            LoadItems();
        }

        public async Task LoadItems()
        {
            var items = await todoRepository.ReadAll(IsVisibleCompleted.Value);
            TodoItemsInternal.Clear();
            foreach (var item in items)
            {
                TodoItemsInternal.Add(item);
            }
        }

        public async Task<TodoItem> Add()
        {
            var newItem = new TodoItem(DateTime.Now.Ticks.ToString(), string.Empty, false, string.Empty, 0, null);
            await todoRepository.Add(newItem);
            LoadItems();
            return newItem;
        }

        public async Task Update(TodoItem item)
        {
            await todoRepository.Update(item);
            //var hit = TodoItemsInternal.Select((todo, index) => (todo, index)).FirstOrDefault(x => x.todo.Id == item.Id);
            //TodoItemsInternal[hit.index] = item;
            LoadItems();
        }

        public async Task Done(TodoItem item)
        {
            await Update(item.MakeCompleted(true));
        }

        public async Task Undone(TodoItem item)
        {
            await Update(item.MakeCompleted(false));
        }
    }
}
