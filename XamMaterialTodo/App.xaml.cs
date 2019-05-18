using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamMaterialTodo.Main;
using XamMaterialTodo.Repositories;
using XamMaterialTodo.Usecases;

namespace XamMaterialTodo
{
    public partial class App : Application
    {
        private readonly TodoUsecase todoUsecase;

        public App()
        {
            InitializeComponent();

            todoUsecase = new TodoUsecase(new LiteDBTodoRepository());
            MainPage = new NavigationPage(new MainPage(todoUsecase));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
