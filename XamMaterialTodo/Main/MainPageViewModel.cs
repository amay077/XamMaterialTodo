using System;
using System.Reactive.Linq;
using Reactive.Bindings;
using XamMaterialTodo.DataModels;
using XamMaterialTodo.Usecases;

namespace XamMaterialTodo.Main
{
    public sealed class MainPageViewModel
    {
        public ReadOnlyReactiveCollection<TodoItem> TodoItems { get; }

        public ReadOnlyReactiveProperty<bool> IsVisibleCompleted { get; }
        public ReactiveCommand ToggleShowCompletedCommand { get; } = new ReactiveCommand();
        public ReadOnlyReactiveProperty<string> FilterLabel { get; }

        private readonly ReactiveProperty<TodoItem> openDetailPageRequestInner = new ReactiveProperty<TodoItem>(initialValue: null, mode: ReactivePropertyMode.None);
        public ReadOnlyReactiveProperty<TodoItem> OpenDetailPageRequest { get; }

        public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<TodoItem> DeleteCommand { get; } = new ReactiveCommand<TodoItem>();
        public ReactiveCommand<TodoItem> UndoneCommand { get; } = new ReactiveCommand<TodoItem>();

        public MainPageViewModel(TodoUsecase todoUsecase)
        {
            TodoItems = todoUsecase.TodoItems.ToReadOnlyReactiveCollection();
            OpenDetailPageRequest = openDetailPageRequestInner.ToReadOnlyReactiveProperty();

            IsVisibleCompleted = todoUsecase.IsVisibleCompleted;
            FilterLabel = IsVisibleCompleted.Select(x => x ? "Hide Done" : "Show Done").ToReadOnlyReactiveProperty();

            ToggleShowCompletedCommand.Subscribe(async _ => 
            {
                await todoUsecase.ToggleShowCompleted();
            });

            AddCommand.Subscribe(async _ => 
            {
                var newItem = await todoUsecase.Add();
                openDetailPageRequestInner.Value = newItem;
            });

            DeleteCommand.Subscribe(async item =>
            {
                await todoUsecase.Delete(item);
            });

            UndoneCommand.Subscribe(async item =>
            {
                await todoUsecase.Undone(item);
            });


            todoUsecase.LoadItems();
        }
    }
}
