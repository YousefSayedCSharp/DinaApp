using DinaApp.ViewModels;

namespace DinaApp.Views;

public partial class HomeTabPageView : ContentPage
{
    public HomeTabPageView()
    {
        InitializeComponent();
        BindingContext = new HomeTabPageVM();
    }
}