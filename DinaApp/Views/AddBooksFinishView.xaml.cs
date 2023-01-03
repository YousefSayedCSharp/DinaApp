namespace DinaApp.Views;

public partial class AddBooksFinishView : ContentPage
{
    public AddBooksFinishView(Book book)
    {
        InitializeComponent();
        BindingContext = new AddBooksFinishVM(book);
    }
}