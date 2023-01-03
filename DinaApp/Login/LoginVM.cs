namespace DinaApp.Login;

public class LoginVM : BaseVM
{
    SaveLogin saveLogin = new SaveLogin();

    public LoginVM()
    {
        isVisible = true;
    }

    private bool _isVisible;
    public bool isVisible
    {
        get => _isVisible;
        set { SetValue(ref _isVisible, value); }
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

    public ICommand btnLogin
    {
        get
        {
            return new Command(() =>
            {
                login();
            });
        }
    }

    private async void login()
    {
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))
        {
            Employee exist = await new EmployeesFBH().GetEmployee(name, password);
            if (exist == null)
            {
                await msg.ShowToast("البيانات التي تم إدخالها غير صحيحة الرجاء التأكد من البيانات وإعادة المحاولة!", true);
                return;
            }

            if (exist != null)
            {
                UserData.userId = exist.Id;
                UserData.userName = exist.Name;
                UserData.userPassword = exist.Password;
                UserData.userRool = exist.IsAdmin;
                saveLogin.Add(exist);
                isVisible = false;
                //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new Views.MainPage()));
                string msg = "";
                MessagingCenter.Send("MyApp", "UpdateCollectionView", msg);
                //await App.Current.MainPage.Navigation.PopAsync();
                return;
            }
        }
        else
        {
            if (string.IsNullOrEmpty(name))
            {
                await msg.ShowToast("الرجاء إدخال الإسم");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                await msg.ShowToast("الرجاء إدخال كلمة المرور");
                return;
            }
        }

    }

}
