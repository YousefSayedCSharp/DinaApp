namespace DinaApp.Views;

public partial class BooksCategoryView : ContentPage
{
    public BooksCategoryView(Category category)
    {
        InitializeComponent();
        BindingContext = new BooksCategoryVM(category);
    }
}