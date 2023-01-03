namespace DinaApp.ViewModels;

public class SelectBookToAddCategoryVM:BaseVM
{
    public SelectBookToAddCategoryVM()
    {
        start();
    }

    private async void start()
    {
        checkLoadData();
        List<Book> bbb = await new BooksFBH().GetAll();
        books = bbb.Where(item=> item.CategoryId == 1).OrderByDescending(item => item.Id).ToList();
    }

    private List<Book> _books;
    public List<Book> books
    {
        get => _books;
        set { SetValue(ref _books, value); }
    }

    private Book _selectedItem;
    public Book selectedItem
    {
        get => _selectedItem;
        set
        {
            if (value == null)
                return;

            SetValue(ref _selectedItem, value);
            App.Current.MainPage.Navigation.PopAsync();
            Book msg = selectedItem;
            MessagingCenter.Send("MyApp", "UpdateCollectionView", msg);
            SetValue(ref _selectedItem, null);

            //end
        }
    }

    private bool _loading;
    public bool loading
    {
        get => _loading;
        set { SetValue(ref _loading, value); }
    }

    private async void checkLoadData()
    {
        loading = true;
        if(books!=null)
        { 
        if (books.Count == 0)
        {
            await Task.Delay(1000);
            checkLoadData();
            return;
        }
        }
        loading = false;
    }

}
