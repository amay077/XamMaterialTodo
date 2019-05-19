using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamMaterialTodo.DataModels;
using XamMaterialTodo.Usecases;

namespace XamMaterialTodo.Presentations.Detail
{

    public partial class DetailPage : ContentPage
    {
        public DetailPage(TodoUsecase todoUsecase, TodoItem item, bool isNew)
        {
            InitializeComponent();
            var viewModel = new DetailPageViewModel(todoUsecase, item, isNew);
            this.BindingContext = viewModel;

            viewModel.ClosePageRequest += async (sender, e) => 
            {
                await this.Navigation.PopAsync();
            };
        }
    }
}
