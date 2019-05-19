using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamMaterialTodo.DataModels;
using XamMaterialTodo.Detail;
using XamMaterialTodo.Usecases;

namespace XamMaterialTodo.Main

{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        private readonly TodoUsecase todoUsecase;
        private readonly MainPageViewModel viewModel;

        public MainPage(TodoUsecase todoUsecase)
        {
            this.todoUsecase = todoUsecase;
            InitializeComponent();

            viewModel = new MainPageViewModel(todoUsecase);
            this.BindingContext = viewModel;

            viewModel.FilterLabel.Subscribe(x => 
            {
                toolBarItem.Text = x;
            });

            viewModel.OpenDetailPageRequest.Where(x => x != null).Subscribe(async todoItem =>
            {
                await this.Navigation.PushAsync(new DetailPage(todoUsecase, todoItem, true));
            });

        }

        async void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            await this.Navigation.PushAsync(new DetailPage(todoUsecase, e.Item as TodoItem, false));
        }

        async void DeleteMenu_Clicked(object sender, EventArgs e)
        {
            if (await this.DisplayAlert("Confirm", "Confirm delete?", "Yes", "No"))
            {
                var todoItem = (sender as MenuItem)?.CommandParameter as TodoItem;
                if (viewModel.DeleteCommand.CanExecute() && todoItem != null)
                {
                    viewModel.DeleteCommand.Execute(todoItem);
                }
            }
        }
    }
}
