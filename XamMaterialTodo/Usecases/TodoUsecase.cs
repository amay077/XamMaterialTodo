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

        private ReactiveProperty<bool> IsVisibleDoneInternal { get; } = new ReactiveProperty<bool>(true);
        public ReadOnlyReactiveProperty<bool> IsVisibleDone { get; }

        public TodoUsecase(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;

            IsVisibleDone = IsVisibleDoneInternal.ToReadOnlyReactiveProperty();
            TodoItems = TodoItemsInternal.ToReadOnlyReactiveCollection();
        }

        public async Task ToggleShowDone()
        {
            IsVisibleDoneInternal.Value = !IsVisibleDoneInternal.Value;
            await LoadItemsInternal();
        }

        public async Task LoadItems()
        {
            await LoadItemsInternal();
        }

        private async Task LoadItemsInternal()
        {
            var items = await todoRepository.ReadAll(IsVisibleDone.Value);
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
            await Update(item.MakeDone(true));
        }

        public async Task Undone(TodoItem item)
        {
            await Update(item.MakeDone(false));
        }
    }
}
