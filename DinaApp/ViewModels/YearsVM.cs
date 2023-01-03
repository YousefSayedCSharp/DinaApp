namespace DinaApp.ViewModels;

public class YearsVM : BaseVM
{
    YearsFBH fbh = new YearsFBH();
    bool RunStart = false;
    public YearsVM()
    {
        btnIsEnabled(false);
        //years = fbh.GetAllIsShow();
        isToggled = true;
        //years.Select(item=>item.IsShow);
        New();
        RunStart = true;
        //years.Distinct();
    }

    private ObservableCollection<Year> _years;
    public ObservableCollection<Year> years
    {
        get => _years;
        set { SetValue(ref _years, value); }
    }

    private string _title;
    public string title
    {
        get => _title;
        set { SetValue(ref _title, value); }
    }

    private bool _isShow;
    public bool isShow
    {
        get => _isShow;
        set { SetValue(ref _isShow, value); }
    }

    private Year _selectedItem;
    public Year selectedItem
    {
        get => _selectedItem;
        set
        {
            if (value == null)
                return;

            SetValue(ref _selectedItem, value);

            //App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new BookView(true, selectedItem)));
            title = selectedItem.Title;
            isShow = selectedItem.IsShow;
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
                checkLoadData();
                fbh.DatabaseItems = new ObservableCollection<Year>();
                //years = fbh.GetAllIsShow();
                selectedItem = new Year();
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
        //return;
        if (selectedItem != null)
            selectedItem = null;
        if (_selectedItem != null)
            _selectedItem = null;
        
        if (RunStart)
        {
        selectedItem = new Year();
        isToggled = true;
        }


        txtRead = "مغلق";
        txtShow = "\uf077";
        show = false;
        
        title = "";
        isShow = true;
        btnIsEnabled(false);
        //if(RunStart&&isToggled!=null)
            
        //years.Distinct();
    }

    private async void Add()
    {
        if (await CheckEntryData())
        {
            await fbh.AddYear(new Year() { Title = title, IsShow = isShow });
            New();
        }//end if
    }//end function

    private async void Edit()
    {
        if (selectedItem != null)
        {
            if (await CheckEntryData())
            {
                await fbh.UpdateYear(selectedItem.Id, new Year() { Title = title, IsShow = isShow });
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
                await fbh.DeleteYear(selectedItem.Id);
                New();
            }//end if
            else
                await msg.ShowToast("الرجاء تحديد قسم لحذفه.");
        }
    }//end function

    private  async void Open(int id)
    {
        await App.Current.MainPage.Navigation.PushAsync(new BooksView(years.FirstOrDefault(item=>item.Id==id)));
    }

    private async Task<bool> CheckEntryData()
    {
        bool b = true;

        if (string.IsNullOrEmpty(title))
        {
            b = false;
            await msg.ShowToast("الرجاء إدخال عنوان للعام.");
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

    private bool _isToggled;
    public bool isToggled
    {
        get => _isToggled;
        set
        {
            SetValue(ref _isToggled, value);
            if (!RunStart)
                return;
            fbh.DatabaseItems = new ObservableCollection<Year>();
            if(years!=null)
            checkLoadData();
            if (isToggled)
            {
                //toggledRead = "عرض المحدد";
                years = fbh.GetAllIsShow();
            }
            else
            {
                //toggledRead = "عرض الكل";
                years = fbh.GetAllYears();
            }

        }
    }

    private string _toggledRead;
    public string toggledRead
    {
        get => _toggledRead;
        set { SetValue(ref _toggledRead,value); }
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
        if (years.Count == 0)
        {
            await Task.Delay(1000);
            checkLoadData();
            return;
        }
        loading = false;
    }

}
