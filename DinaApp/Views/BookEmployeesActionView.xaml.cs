namespace DinaApp.Views;

public partial class BookEmployeesActionView : ContentPage
{
    public BookEmployeesActionView(Book book)
    {
        InitializeComponent();
        BindingContext = new BookEmployeesPrintsVM(book);
    }
}