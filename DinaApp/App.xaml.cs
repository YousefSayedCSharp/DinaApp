namespace DinaApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new LoadPage();
    }

    protected override async void OnStart()
    {
        base.OnStart();
        string check = await CheckActivation.Check();
        if (!string.IsNullOrEmpty(check))
        {
            MainPage = new ActivationView(check);
            return;
        }
        //MainPage = new EmployeesView();
        tabs();
        MessagingCenter.Subscribe<string, string>("MyApp", "UpdateCollectionView", (sender, result) =>
        {
            //if (Children.FirstOrDefault() != null)
            tabs();
            //MainPage = new AppShell();
        });
    }

    public async void tabs()
    {
        bool b = await new CheckLogin().login();
        if (!b)
        {
            MainPage = new LoginView();
            return;
        }

        if (b)
            MainPage = new AppShell();
    }

}//end class
