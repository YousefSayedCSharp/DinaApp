namespace DinaApp.Views;

public partial class CategoriesView : ContentPage
{
    public CategoriesView()
    {
        InitializeComponent();
        BindingContext = new CategoriesVM();
    }
}