using DinaApp.ViewModels;

namespace DinaApp.Views;

public partial class SelectBookToAddCategoryView : ContentPage
{
    public SelectBookToAddCategoryView()
    {
        InitializeComponent();
        BindingContext = new SelectBookToAddCategoryVM();
    }
}