using System;
using System.ComponentModel;
using Tobias.App.Models;
using Tobias.App.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tobias.App.Views
{
    public partial class AboutPage : ContentPage
    {
        public IDataStore<Item> DataStore { get; private set; }
        public AboutPage()
        {
            InitializeComponent();
            DataStore = DependencyService.Resolve<ServerDb>();
            DataStore = DependencyService.Resolve<IDataStore<Item>>();
        }
    }
}