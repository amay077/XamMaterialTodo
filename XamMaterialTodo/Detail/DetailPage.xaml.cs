using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamMaterialTodo.DataModels;
using XamMaterialTodo.Usecases;

namespace XamMaterialTodo.Detail
{

    public partial class DetailPage : ContentPage
    {
        private readonly DetailPageViewModel viewModel;

        public DetailPage(TodoUsecase todoUsecase, TodoItem item)
        {
            InitializeComponent();
            viewModel = new DetailPageViewModel(todoUsecase, item);
            this.BindingContext = viewModel;

            viewModel.ActionLabel.Subscribe(x =>
            {
                toolBarItem.Text = x;
            });

            viewModel.ClosePageRequest.Subscribe(async _ => 
            {
                await this.Navigation.PopAsync();
            });
        }

        ~DetailPage()
        {
            viewModel.Dispose();
        }
    }
}
