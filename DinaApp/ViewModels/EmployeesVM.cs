namespace DinaApp.ViewModels;

public class EmployeesVM : BaseVM
{
    EmployeesFBH fbh = new EmployeesFBH();
    bool RunStart = false;
    public EmployeesVM()
    {
        btnIsEnabled(false);
        employees = fbh.GetAllEmployees();
        checkLoadData();
        New();
        RunStart = true;
    }

    private ObservableCollection<Employee> _employees;
    public ObservableCollection<Employee> employees
    {
        get => _employees;
        set { SetValue(ref _employees, value); }
    }

    private string _name;
    public string name
    {
        get => _name;
        set { SetValue(ref _name, value); }
    }

    private string _password;
    public string password
    {
        get => _password;
        set { SetValue(ref _password, value); }
    }

    private bool _isAdmin;
    public bool isAdmin
    {
        get => _isAdmin;
        set { SetValue(ref _isAdmin, value); }
    }

    private bool _isActive;
    public bool isActive
    {
        get => _isActive;
        set { SetValue(ref _isActive, value); }
    }

    private string _appId;
    public string appId
    {
        get => _appId;
        set { SetValue(ref _appId, value); }
    }

    private Employee _selectedItem;
    public Employee selectedItem
    {
        get => _selectedItem;
        set
        {
            if (value == null)
                return;

            SetValue(ref _selectedItem, value);

            //App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new BookView(true, selectedItem)));
            name = selectedItem.Name;
            password = selectedItem.Password;
            isAdmin = selectedItem.IsAdmin;
            isActive = selectedItem.IsActive;
            btnIsEnabled(true);
            if(!show)
            {
                show = true;
                txtShow = "\uf078";
                txtRead = "مفتوح";
            }
            //SetValue(ref _selectedItem, null);

            //end
        }
    }

    private bool _btnAddIsEnabled;
    public bool btnAddIsEnabled
    {
        get => _btnAddIsEnabled;
        set { SetValue(ref _btnAddIsEnabled, value); }
    }

    private bool _btnEditIsEnabled;
    public bool btnEditIsEnabled
    {
        get => _btnEditIsEnabled;
        set { SetValue(ref _btnEditIsEnabled, value); }
    }

    private bool _btnDeleteIsEnabled;
    public bool btnDeleteIsEnabled
    {
        get => _btnDeleteIsEnabled;
        set { SetValue(ref _btnDeleteIsEnabled, value); }
    }

    public ICommand btnNew
    {
        get
        {
            return new Command(() =>
            {
                fbh.DatabaseItems = new ObservableCollection<Employee>();
                employees = fbh.GetAllEmployees();
                selectedItem = new Employee();
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

    public ICommand btnEdit
    {
        get
        {
            return new Command(() =>
            {
                Edit();
            });
        }
    }

    public ICommand btnDelete
    {
        get
        {
            return new Command(() =>
            {
                Delete();
            });
        }
    }

    public ICommand btnOpen
    {
        get
        {
            return new Command((Id) =>
            {
                int id = Convert.ToInt32(Id);
                Open(id);
            });
        }
    }

    private void New()
    {
        if (selectedItem != null)
            selectedItem = null;
        if (_selectedItem != null)
            _selectedItem = null;

        if(RunStart)
            selectedItem = new Employee();

        txtRead = "مغلق";
        txtShow = "\uf077";
        show = false;

        name = "";
        password = "";
        isAdmin = false;
        isActive= true;
        btnIsEnabled(false);
        employees.Distinct();
    }

    private async void Add()
    {
        if (await CheckEntryData())
        {
            await fbh.AddEmployee(new Employee()
            {
                Name = name,
                Password=password,
                 IsAdmin= isAdmin,
                IsActive= isActive
            });
            New();
        }//end if
    }//end function

    private async void Edit()
    {
        if (selectedItem != null)
        {
            if (await CheckEntryData())
            {
                await fbh.UpdateEmployee(selectedItem.Id, new Employee()
                {
                    Name= name,
                    Password= password,
                    IsAdmin= isAdmin,
                    IsActive = isActive
                });
                New();
            }//end if
        }//end if
        else
            await msg.ShowToast("الرجاء تحديد قسم لتعديله.");
    }//end function

    private async void Delete()
    {
        bool b = await msg.ShowAlertDeleteCancel(msg.msgDelete, "");
        if (b)
        {
            if (selectedItem != null)
            {
                await fbh.DeleteEmployee(selectedItem.Id);
                New();
            }//end if
            else
                await msg.ShowToast("الرجاء تحديد قسم لحذفه.");
        }
    }//end function

    private async void Open(int id)
    {
        await App.Current.MainPage.Navigation.PushAsync(new EmployeePrintsView(employees.FirstOrDefault(item=>item.Id==id)));
        string msg = "";
        MessagingCenter.Send("MyApp", "UpdateBinding", msg);
    }

    private async Task<bool> CheckEntryData()
    {
        bool b = true;

        if (string.IsNullOrEmpty(name))
        {
            b = false;
            await msg.ShowToast("الرجاء إدخال إسم الموظف.");
            return b;
        }

        if (string.IsNullOrEmpty(password))
        {
            b = false;
            await msg.ShowToast("الرجاء إدخال كلمة مرور للموظف.");
            return b;
        }

        return b;
    }

    private void btnIsEnabled(bool b)
    {
        if (b)
            btnAddIsEnabled = false;
        else
            btnAddIsEnabled = true;

        btnEditIsEnabled = b;
        btnDeleteIsEnabled = b;
    }

    private bool _show;
    public bool show
    {
        get => _show;
        set { SetValue(ref _show, value); }
    }

    private string _txtShow;
    public string txtShow
    {
        get => _txtShow;
        set { SetValue(ref _txtShow, value); }
    }

    public ICommand btnShow
    {
        get
        {
            return new Command(() =>
            {
                if (show)
                {
                    //opend
                    show = false;
                    txtShow = "\uf077";
                    txtRead = "مغلق";
                }
                else
                {
                    //closed
                    show = true;
                    txtShow = "\uf078";
                    txtRead = "مفتوح";
                }
            });
        }
    }

    private string _txtRead;
    public string txtRead
    {
        get => _txtRead;
        set { SetValue(ref _txtRead, value); }
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
        if (employees.Count == 0)
        {
            await Task.Delay(1000);
            checkLoadData();
            return;
        }
        loading = false;
    }

}
