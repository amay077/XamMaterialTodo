using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using XamMaterialTodo.DataModels;
using XamMaterialTodo.Usecases;

namespace XamMaterialTodo.Detail
{
    public sealed class DetailPageViewModel : IDisposable
    {
        public ReactiveProperty<string> Title { get; }
        public ReactiveProperty<string> Description { get; }
        private ReactiveProperty<bool> IsCompleted { get; }
        public ReadOnlyReactiveProperty<string> ActionLabel { get; }
        public ReactiveProperty<int> Priority { get; }
        public ReactiveProperty<bool> IsDueDateEnabled { get; }
        public ReactiveProperty<DateTime> DueDate { get; }
        public ReadOnlyReactiveProperty<TodoItem> UpdatedItem { get; }


        public ReactiveCommand DoneCommand { get; } = new ReactiveCommand();

        private readonly ReactiveProperty<Unit> closePageRequestInner = new ReactiveProperty<Unit>(mode: ReactivePropertyMode.None);
        public ReadOnlyReactiveProperty<Unit> ClosePageRequest { get; }

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public DetailPageViewModel(TodoUsecase todoUsecase, TodoItem item)
        {
            Title = new ReactiveProperty<string>(item.Title);
            Description = new ReactiveProperty<string>(item.Description);
            IsCompleted = new ReactiveProperty<bool>(item.IsCompleted);
            Priority = new ReactiveProperty<int>(item.Priority);

            ClosePageRequest = closePageRequestInner.ToReadOnlyReactiveProperty();

            IsDueDateEnabled = new ReactiveProperty<bool>(item.DueDate.HasValue);
            DueDate = new ReactiveProperty<DateTime>(item.DueDate.HasValue ? item.DueDate.Value.LocalDateTime : DateTimeOffset.Now.LocalDateTime);

            UpdatedItem = Observable.CombineLatest(
                Title, Description, Priority, IsDueDateEnabled, DueDate,
                (title, description, priority, isDueDateEnabled, dueDate) =>
                    new TodoItem(item.Id, title, IsCompleted.Value, description, priority, isDueDateEnabled ? dueDate : (DateTimeOffset?)null))
                .ToReadOnlyReactiveProperty();

            UpdatedItem.Subscribe(async x => 
            {
                await todoUsecase.Update(x);
            }).AddTo(disposables);

            DoneCommand.Subscribe(async _ => 
            {
                await todoUsecase.Done(UpdatedItem.Value);
                IsCompleted.Value = true;
                closePageRequestInner.Value = Unit.Default;
            });
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
