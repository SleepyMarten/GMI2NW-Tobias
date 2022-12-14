using System;
using Tobias.App.Services;
using Tobias.App.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tobias.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<ServerDb>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
