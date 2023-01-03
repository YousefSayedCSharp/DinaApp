namespace DinaApp.ViewModels;

public class BookEmployeesPrintsVM : BaseVM
{

    public BookEmployeesPrintsVM(Book bookSended)
    {
        book = bookSended;
        run();
    }

    private Book _book;
    public Book book
    {
        get => _book;
        set { SetValue(ref _book, value); }
    }

    private List<BookEmployeesActionMVM> _group;
    public List<BookEmployeesActionMVM> group
    {
        get => _group;
        set { SetValue(ref _group, value); }
    }

    public async void run()
    {
        group = new List<BookEmployeesActionMVM>();
        checkLoadData();
        List<Employee> employees1 = await new EmployeesFBH().GetAll();
        List<Employee> employees = employees1.OrderByDescending(item => item.Id).ToList();

        List<BookAction> PrintsBooks = await new FinishBooksActionFBH().GetAll();

        List<BookEmployeesActionMVM> AllGroups = new List<BookEmployeesActionMVM>();
        foreach (Employee emp in employees)
        {
            List<BookAction> g = PrintsBooks.Where(item => item.EmployeeId == emp.Id && item.BookId == book.Id).OrderByDescending(item => item.Id).ToList();
            BookEmployeesActionMVM h = new BookEmployeesActionMVM(emp.Name);
            bool check = false;
            foreach (BookAction bookAction in g)
            {
                check = true;
                h.Add(bookAction);
            }
            if (check)
                AllGroups.Add(h);
        }//end foreach
        group = AllGroups;
    }

    public ICommand btnNew
    {
        get
        {
            return new Command(() =>
            {
                group = new List<BookEmployeesActionMVM>();
                run();
            });
        }
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

    public ICommand btn
    {
        get
        {
            return new Command(() =>
            {
                run();
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
        if (group.Count == 0)
        {
            await Task.Delay(1000);
            checkLoadData();
            return;
        }
        loading = false;
    }

}//end class
