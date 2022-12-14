using System;
using System.Windows.Input;
using Tobias.App.Models;
using Tobias.App.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tobias.App.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public IDataStore<Item> DataStoreOfItem { get; } = Xamarin.Forms.DependencyService.Resolve<IDataStore<Item>>(fallbackFetchTarget: DependencyFetchTarget.GlobalInstance);

        public AboutViewModel()
        {
            Title = "About";
        }
    }
}