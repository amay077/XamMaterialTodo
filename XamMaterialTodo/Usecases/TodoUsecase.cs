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
        private readonly ITodoRepository todoRepository;

        private ReactiveCollection<TodoItem> TodoItemsInternal { get; } = new ReactiveCollection<TodoItem>();
        public ReadOnlyReactiveCollection<TodoItem> TodoItems { get; }

        private ReactiveProperty<bool> IsVisibleCompletedInternal { get; } = new ReactiveProperty<bool>(true);
        public ReadOnlyReactiveProperty<bool> IsVisibleCompleted { get; }

        public TodoUsecase(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;

            IsVisibleCompleted = IsVisibleCompletedInternal.ToReadOnlyReactiveProperty();
            TodoItems = TodoItemsInternal.ToReadOnlyReactiveCollection();
        }

        public async Task ToggleShowCompleted()
        {
            IsVisibleCompletedInternal.Value = !IsVisibleCompletedInternal.Value;
            await LoadItemsInternal();
        }

        public async Task LoadItems()
        {
            await LoadItemsInternal();
        }

        private async Task LoadItemsInternal()
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
            var newItem = new TodoItem(DateTime.Now.Ticks.ToString(), "無題のTODO", false, string.Empty, 0, null, DateTimeOffset.Now);
            await todoRepository.Add(newItem);
            await LoadItemsInternal();
            return newItem;
        }

        public async Task Update(TodoItem item)
        {
            await todoRepository.Update(item);
            await LoadItemsInternal();
        }

        public async Task Delete(TodoItem item)
        {
            await todoRepository.Delete(item);
            await LoadItemsInternal();
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
