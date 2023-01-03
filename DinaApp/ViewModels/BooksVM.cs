namespace DinaApp.ViewModels;

    public class BooksVM : BaseVM
    {

        BooksFBH fbh = new BooksFBH();
        bool RunStart = false;
        Year selectedYear=new Year();
        int sendedYearId=0;

        public BooksVM(Year year)
        {
            selectedYear = year;
            sendedYearId = year.Id;
            yearName = "الكتب: " + selectedYear.Title;
            btnIsEnabled(false);
            //test();

            books = fbh.GetAllBooksByYear(sendedYearId);
            New();
            RunStart = true;
            SumPaper();
        }
        public async void test()
        {
            List<Book> bbb = await fbh.GetAll();
            foreach (Book b in bbb.Where(item => item.YearId == sendedYearId))
            {
                books.Add(b);
            }
            fbh.DatabaseItems = fbh.GetAllBooksByYear(sendedYearId);
        }

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> books
        {
            get => _books;
            set { SetValue(ref _books, value); }
        }

        private string _yearName;
        public string yearName
        {
            get => _yearName;
            set { SetValue(ref _yearName,value); }
        }

        private string _name;
        public string name
        {
            get => _name;
            set { SetValue(ref _name, value); }
        }

        private string _paperCount;
        public string paperCount
        {
            get => _paperCount;
            set { SetValue(ref _paperCount,value); }
        }

        private string _count;
        public string count
        {
            get => _count;
            set { SetValue(ref _count, value); }
        }

        private string _finish;
        public string finish
        {
            get => _finish;
            set { SetValue(ref _finish, value); }
        }

        private string _unFinish;
        public string unFinish
        {
            get => _unFinish;
            set { SetValue(ref _unFinish, value); }
        }

        private DateTime _date;
        public DateTime date
        {
            get => _date;
            set { SetValue(ref _date,value); }
        }

        private bool _isShoww;
        public bool isShow
        {
            get => _isShoww;
            set { SetValue(ref _isShoww, value); }
        }

        private Book _selectedItem;
        public Book selectedItem
        {
            get => _selectedItem;
            set
            {
                SumPaper();
                if (value == null)
                {
                    SumPaper();
                    return;
                }

                SetValue(ref _selectedItem, value);

                //App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new BookView(true, selectedItem)));
                name = selectedItem.Name;
                paperCount = selectedItem.PaperCount+"";
                count= selectedItem.Count+"";
                date= ConvertDateToString.stringToDate(selectedItem.Date);
                isShow = selectedItem.IsShow;
                btnIsEnabled(true);
                if (!show)
                {
                    show = true;
                    txtShow = "\uf078";
                    txtRead = "مفتوح";
                }

                int sumpapers = selectedItem.Finish*selectedItem.PaperCount;
                if (selectedItem != null)
                    strSumPaper = "\uf2f9 " + sumpapers;
                else
                    SumPaper();

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
                    fbh.DatabaseItems = new ObservableCollection<Book>();
                    books = fbh.GetAllBooksByYear(sendedYearId);
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
            if (RunStart)
            {

                if (selectedItem != null)
                    selectedItem = null;
                if (_selectedItem != null)
                    _selectedItem = null;


                selectedItem = new Book();

            }

            yearName = "الكتب: "+selectedYear.Title;

            txtRead = "مغلق";
            txtShow = "\uf077";
            show = false;
            
            name = "";
            paperCount = "";
            count = "";
            date = DateTime.Now.Date;
            isShow = true;
            btnIsEnabled(false);
            //books.Distinct();
            //SumPaper();
        }

        private async void Add()
        {
            //await msg.ShowToast(books.Count+"");
            //return;
            if (await CheckEntryData())
            {
                if (date<=DateTime.Now.Date)
                {
                    await msg.ShowToast("الرجاء تحديد تاريخ التسليم بشكل صحيح.");
                    return;
                }
                
                await fbh.AddBook(new Book()
                {
                    Name = name,
                    CategoryId=1,
                    YearId=sendedYearId,
                    PaperCount=ConvertToNumber.UseConverter(paperCount),
                    Count =ConvertToNumber.UseConverter(count),
                    UnFinish =ConvertToNumber.UseConverter(count),
                    Date=ConvertDateToString.dateToString(date),
                    IsShow=isShow
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
                    await fbh.UpdateBook(selectedItem.Id, new Book()
                    {
                        BookId=selectedItem.BookId,
                        Name = name,
                        CategoryId = selectedItem.CategoryId,
                        YearId = selectedItem.YearId,
                        PaperCount = ConvertToNumber.UseConverter(paperCount),
                        Count = ConvertToNumber.UseConverter(count),
                        Finish=selectedItem.Finish,
                        UnFinish = selectedItem.UnFinish,
                        Date = ConvertDateToString.dateToString(date),
                        IsShow = isShow

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
                    await fbh.DeleteBook(selectedItem.Id);
                    New();
                }//end if
                else
                    await msg.ShowToast("الرجاء تحديد قسم لحذفه.");
            }
        }//end function

        private async void Open(int id)
        {
            await App.Current.MainPage.Navigation.PushAsync(new BookEmployeesActionView(books.FirstOrDefault(item => item.Id == id)));
        }

        private async Task<bool> CheckEntryData()
        {
            bool b = true;

            if (string.IsNullOrEmpty(name))
            {
                b = false;
                await msg.ShowToast("الرجاء إدخال عنوان للكتاب.");
                return b;
            }

            if (string.IsNullOrEmpty(paperCount))
            {
                b = false;
                await msg.ShowToast("الرجاء إدخال عدد أوراق نسخة واحدة من الكتاب.",true);
                return b;
            }

            if (string.IsNullOrEmpty(count))
            {
                b = false;
                await msg.ShowToast("الرجاء إدخال عدد نسخ الكتاب.");
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

        private string _strSumPaper;
        public string strSumPaper 
        {
            get => _strSumPaper;
            set { SetValue(ref _strSumPaper,value); }
        }

        public ICommand btnSumPaper
        {
            get
            {
                return new Command(() =>
                {
                    SumPaper();
                });
            }
        }

        private async void SumPaper()
        {
            loading=true;
            if(fbh.DatabaseItems.Count==0)
            {
                await Task.Delay(1000);
                //System.Threading.Thread.Sleep(1000);
                SumPaper();
                return;
            }

            int sum = 0;
            foreach (var item in books.Where(item=>item.CategoryId==1))
            {
                sum += item.Finish * item.PaperCount;
            }
            strSumPaper = "\uf2f9 " + sum;
            loading = false;
        }

        private bool _loading;
        public bool loading
        {
            get => _loading;
            set { SetValue(ref _loading, value); }
        }

    }
