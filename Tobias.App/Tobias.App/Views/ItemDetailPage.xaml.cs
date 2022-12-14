using System.ComponentModel;
using Tobias.App.ViewModels;
using Xamarin.Forms;

namespace Tobias.App.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}