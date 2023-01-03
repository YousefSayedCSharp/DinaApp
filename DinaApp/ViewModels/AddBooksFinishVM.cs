namespace DinaApp.ViewModels;

public class AddBooksFinishVM : BaseVM
{
    public AddBooksFinishVM(Book thisBook)
    {
        book = thisBook;
        lblTitle = thisBook.Name;
    }

    private Book _book;
    public Book book
    {
        get => _book;
        set { SetValue(ref _book, value); }
    }

    private string _lblTitle;
    public string lblTitle
    {
        get => _lblTitle;
        set { SetValue(ref _lblTitle, value); }
    }

    private string _printedCount;
    public string printedcount
    {
        get => _printedCount;
        set { SetValue(ref _printedCount, value); }
    }

    public ICommand btnSave
    {
        get
        {
            return new Command(() =>
            {
                save();
            });
        }
    }

    public ICommand btnCancel
    {
        get
        {
            return new Command(() =>
            {
                cancel();
            });
        }
    }

    private async void save()
    {
        BookAction ba = new BookAction()
        {
            BookId=book.Id,
            CategoryId=book.CategoryId,
            EmployeeId=UserData.userId,
            Count= ConvertToNumber.UseConverter(printedcount),
            Date=DateTime.Now
        };
        
        await new FinishBooksActionFBH().AddPrent(ba);
        await new BooksFBH().UpdateBook(book.Id,book);

        cancel();
    }

    private void cancel()
    {
        App.Current.MainPage.Navigation.PopAsync();
    }

}
