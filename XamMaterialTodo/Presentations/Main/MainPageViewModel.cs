using System;
using System.Reactive.Linq;
using Reactive.Bindings;
using XamMaterialTodo.DataModels;
using XamMaterialTodo.Usecases;

namespace XamMaterialTodo.Presentations.Main
{
    public sealed class MainPageViewModel
    {
        public ReadOnlyReactiveCollection<TodoItem> TodoItems { get; }

        public ReadOnlyReactiveProperty<bool> IsVisibleDone { get; }
        public ReactiveCommand ToggleShowDoneCommand { get; } = new ReactiveCommand();
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

            IsVisibleDone = todoUsecase.IsVisibleDone;
            FilterLabel = IsVisibleDone.Select(x => x ? "Hide Done" : "Show Done").ToReadOnlyReactiveProperty();

            ToggleShowDoneCommand.Subscribe(async _ => 
            {
                await todoUsecase.ToggleShowDone();
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
