namespace DinaApp.ViewModels;

public class HomeTabPageVM : BaseVM
{
    BooksFBH fbh = new BooksFBH();
    bool RunStart = false;

    public HomeTabPageVM()
    {
        books = fbh.GetAllBooksByCategory(1);
        checkLoadData();
        New();
        RunStart = true;
    }

    private ObservableCollection<Book> _books;
    public ObservableCollection<Book> books
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

            App.Current.MainPage.Navigation.PushAsync(new AddBooksFinishView(selectedItem));

            SetValue(ref _selectedItem, null);

            //end
        }
    }

    public ICommand btnNew
    {
        get
        {
            return new Command(() =>
            {
                fbh.DatabaseItems = new ObservableCollection<Book>();
                //books = fbh.GetAllBooksByYear(1);
                selectedItem = new Book();
                New();
            });
        }
    }

    public ICommand btnAdd
    {
        get
        {
            return new Command(() =>
            {
                Add();
            });
        }
    }

    private void New()
    {
        if (selectedItem != null)
            selectedItem = null;
        if (_selectedItem != null)
            _selectedItem = null;

        if (RunStart)
            selectedItem = new Book();

        books.Distinct();
    }

    private void Add()
    {
        
    }//end function

    private bool _loading;
    public bool loading
    {
        get => _loading;
        set { SetValue(ref _loading, value); }
    }

    private async void checkLoadData()
    {
        loading = true;
        if (books.Count == 0)
        {
            await Task.Delay(1000);
            checkLoadData();
            return;
        }
        loading = false;
    }

}
