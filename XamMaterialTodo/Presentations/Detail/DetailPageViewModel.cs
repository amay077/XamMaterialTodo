using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Xamarin.Forms;
using XamMaterialTodo.DataModels;
using XamMaterialTodo.Usecases;

namespace XamMaterialTodo.Presentations.Detail
{
    public sealed class DetailPageViewModel
    {
        public ReactiveProperty<string> Title { get; }
        public ReactiveProperty<string> Description { get; }
        public ReactiveProperty<bool> IsDone { get; }
        public ReadOnlyReactiveProperty<string> ActionLabel { get; }
        public ReactiveProperty<int> Priority { get; }

        public ReactiveProperty<bool> HasDueDate { get; }
        public ReactiveProperty<DateTime> DueDate { get; }
        public ReactiveCommand DeleteDueDateCommand { get; } = new ReactiveCommand();

        private ReadOnlyReactiveProperty<TodoItem> UpdatedItem { get; }


        public ReadOnlyReactiveProperty<bool> IsVisibleDone { get; }
        public ReactiveCommand DoneCommand { get; } = new ReactiveCommand();

        public event EventHandler<Unit> ClosePageRequest;

        public DetailPageViewModel(TodoUsecase todoUsecase, TodoItem item, bool isNew)
        {
            Title = new ReactiveProperty<string>(item.Title);
            Description = new ReactiveProperty<string>(item.Description);
            IsDone = new ReactiveProperty<bool>(item.IsDone);
            Priority = new ReactiveProperty<int>(item.Priority);

            HasDueDate = new ReactiveProperty<bool>(item.DueDate.HasValue);
            DueDate = new ReactiveProperty<DateTime>(item.DueDate.HasValue ? item.DueDate.Value.LocalDateTime : DateTime.Today);

            UpdatedItem = Observable.CombineLatest(
                Title, Description, Priority, HasDueDate, DueDate,
                (title, description, priority, hasDueDate, dueDate) =>
                    new TodoItem(item.Id, title, IsDone.Value, description, priority, hasDueDate ? dueDate : (DateTimeOffset?)null, item.CreateDate))
                .ToReadOnlyReactiveProperty();

            UpdatedItem.Subscribe(async x => 
            {
                await todoUsecase.Update(x);
            });

            DueDate.Subscribe(x => 
            {
                if (x == DateTime.Today)
                {
                    return;
                }
                HasDueDate.Value = true;
            });

            HasDueDate.Subscribe(x => 
            {
                System.Diagnostics.Debug.WriteLine($"{x}");
            });

            DeleteDueDateCommand.Subscribe(_ => 
            {
                HasDueDate.Value = false;
            });

            IsVisibleDone = Observable.Return(!isNew).ToReadOnlyReactiveProperty();
            DoneCommand.Subscribe(async _ => 
            {
                await todoUsecase.Done(UpdatedItem.Value);
                IsDone.Value = true;
                ClosePageRequest?.Invoke(this, Unit.Default);
            });
        }
    }
}
