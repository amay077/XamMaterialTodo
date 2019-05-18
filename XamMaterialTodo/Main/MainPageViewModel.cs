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

        public MainPageViewModel(TodoUsecase todoUsecase)
        {
            TodoItems = todoUsecase.TodoItems.ToReadOnlyReactiveCollection();
            OpenDetailPageRequest = openDetailPageRequestInner.ToReadOnlyReactiveProperty();

            IsVisibleCompleted = todoUsecase.IsVisibleCompleted;
            FilterLabel = IsVisibleCompleted.Select(x => x ? "Hide Done" : "Show Done").ToReadOnlyReactiveProperty();

            ToggleShowCompletedCommand.Subscribe(_ => 
            {
                todoUsecase.ToggleShowCompleted();
            });

            AddCommand.Subscribe(async _ => 
            {
                var newItem = await todoUsecase.Add();
                openDetailPageRequestInner.Value = newItem;
            });

            todoUsecase.LoadItems();
        }
    }
}
