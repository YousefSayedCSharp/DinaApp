namespace DinaApp.ViewModels;

public class BooksCategoryVM : BaseVM
{
    BooksCategoryFBH fbh = new BooksCategoryFBH();
    
    public BooksCategoryVM(Category sendedCategory)
    {
        category = sendedCategory;
        books = fbh.GetAllBooksByCategory(category.Id);
        checkLoadData();
        title = "الأقسام: " + category.Name;
        SubscribeMessaging();
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

            if(selectedItem.Id>1)
            App.Current.MainPage.Navigation.PushModalAsync(new AddBooksFinishView(selectedItem));

            SetValue(ref _selectedItem, null);

            //end
        }
    }

    private Category _category;
    public Category category
    {
        get => _category;
        set { SetValue(ref _category,value); }
    }

    private string _title;
    public string title
    {
        get => _title;
        set { SetValue(ref _title,value); }
    }

    public ICommand btnNew
    {
        get
        {
            return new Command(() =>
            {
                checkLoadData();
                fbh.DatabaseItems = new ObservableCollection<Book>();
                //selectedItem = new Book();
                books = fbh.GetAllBooksByCategory(category.Id);
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

    private async void Add()
    {
        if(category.Id==1)
        {
            await msg.ShowToast("لا يمكن استخدام هذا الزر في هذا القسم.",true);
            return;
        }

        await App.Current.MainPage.Navigation.PushAsync(new SelectBookToAddCategoryView());
    }//end function

    public void SubscribeMessaging()
    {
        MessagingCenter.Subscribe<string, Book>("MyApp", "UpdateCollectionView", async (sender, result) =>
        {
            bool check = await msg.ShowAlertYesNo("سيتم إضافة هذا الكتاب "+result.Name+" إلى هذا القسم.");
            if(check)
            {
                Book newBook = new Book()
                {
                    BookId=result.BookId,
                    Name =result.Name,
                    CategoryId=category.Id,
                    YearId=result.YearId,
                    PaperCount=result.PaperCount,
                    Count = result.Count,
                    Finish = 0,
                    UnFinish = result.Count,
                    Date = result.Date,
                    IsShow = result.IsShow,
                };
                await fbh.AddBook(newBook);
            }
        });
    }

    public ICommand btnBack
    {
        get
        {
            return new Command(() =>
            {
                App.Current.MainPage.Navigation.PopAsync();
            });
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
        if (books.Count == 0)
        {
            await Task.Delay(1000);
            checkLoadData();
            return;
        }
        loading = false;
    }

}
