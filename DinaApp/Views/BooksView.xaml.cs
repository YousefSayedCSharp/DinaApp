namespace DinaApp.Views;

public partial class BooksView : ContentPage
{
    public BooksView(Year year)
    {
        InitializeComponent();
        BindingContext = new BooksVM(year);
    }
}