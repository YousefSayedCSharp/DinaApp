namespace DinaApp.ViewModels;

public class EmployeePrintsVM : BaseVM
{

    public EmployeePrintsVM(Employee employeeSended)
    {
        employee = employeeSended;
        run();
    }

    private Employee _employee;
    public Employee employee
    {
        get => _employee;
        set { SetValue(ref _employee, value); }
    }

    private List<BookActionMVM> _group;
    public List<BookActionMVM> group
    {
        get => _group;
        set { SetValue(ref _group, value); }
    }

    public async void run()
    {
        group = new List<BookActionMVM>();
        checkLoadData();
        List<Book> books1 = await new BooksFBH().GetAll();
        List<Book> books = books1.OrderByDescending(item => item.Id).ToList();

        List<BookAction> PrintsBooks = await new FinishBooksActionFBH().GetAll();

        List<BookActionMVM> AllGroups = new List<BookActionMVM>();
        foreach (Book book in books)
        {
            List<BookAction> g = PrintsBooks.Where(item => item.BookId == book.Id && item.EmployeeId == employee.Id).OrderByDescending(item=>item.Id).ToList();
            BookActionMVM h = new BookActionMVM(book.Name);
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
                group = new List<BookActionMVM>();
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
