using System;
using System.Collections.Generic;
using Tobias.App.ViewModels;
using Tobias.App.Views;
using Xamarin.Forms;

namespace Tobias.App
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
