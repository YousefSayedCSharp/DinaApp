namespace DinaApp.ViewModels;

public class CategoriesVM : BaseVM
{
    CategoriesFBH fbh = new CategoriesFBH();
    bool RunStart = false;
    public CategoriesVM()
    {
        btnIsEnabled(false);
        categories = fbh.GetAllCategories();
        checkLoadData();
        New();
        RunStart = true;
    }

    private ObservableCollection<Category> _categories;
    public ObservableCollection<Category> categories
    {
        get => _categories;
        set { SetValue(ref _categories, value); }
    }

    private string _name;
    public string name
    {
        get => _name;
        set { SetValue(ref _name, value); }
    }

    private Category _selectedItem;
    public Category selectedItem
    {
        get => _selectedItem;
        set
        {
            if (value == null)
                return;

            SetValue(ref _selectedItem, value);

            //App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new BookView(true, selectedItem)));
            name = selectedItem.Name;
            btnIsEnabled(true);
            if (!show)
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
        set
        {
            SetValue(ref _btnEditIsEnabled, value);
        }
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
                checkLoadData();
                fbh.DatabaseItems = new ObservableCollection<Category>();
                categories = fbh.GetAllCategories();
                selectedItem = new Category();
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

        if (RunStart)
            selectedItem = new Category();

        txtRead = "مغلق";
        txtShow = "\uf077";
        show = false;

        name = "";
        btnIsEnabled(false);
        categories.Distinct();
    }

    private async void Add()
    {
        if (await CheckEntryData())
        {
            await fbh.AddCategory(new Category() { Name = name });
            New();
        }//end if
    }//end function

    private async void Edit()
    {
        if (selectedItem != null)
        {
            if (await CheckEntryData())
            {
                await fbh.UpdateCategory(selectedItem.Id, new Category() { Name = name });
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
                await fbh.DeleteCategory(selectedItem.Id);
                New();
            }//end if
            else
                await msg.ShowToast("الرجاء تحديد قسم لحذفه.");
        }
    }//end function

    private void Open(int id)
    {
        App.Current.MainPage.Navigation.PushAsync(new BooksCategoryView(categories.FirstOrDefault(item => item.Id == id)));
    }

    private async Task<bool> CheckEntryData()
    {
        bool b = true;

        if (string.IsNullOrEmpty(name))
        {
            b = false;
            await msg.ShowToast("الرجاء إدخال عنوان للقسم.");
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
        if (categories.Count == 0)
        {
            await Task.Delay(1000);
            checkLoadData();
            return;
        }
        loading = false;
    }

}
