namespace DinaApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        AddTabbed();
    }

    public async void AddTabbed()
    {
        try
        {
            ShellSection home = new ShellSection { Title = "الرئيسية", };
            home.Items.Add(new ShellContent() { Content = new HomeTabPageView() });
            tab.Items.Add(home);
            if (UserData.userRool)
            {
                ShellSection years = new ShellSection { Title = "الكتب", };
                years.Items.Add(new ShellContent() { Content = new YearsView() });

                ShellSection Employees = new ShellSection { Title = "الموظفين", };
                Employees.Items.Add(new ShellContent() { Content = new EmployeesView() });

                ShellSection categories = new ShellSection { Title = "الأقسام", };
                categories.Items.Add(new ShellContent() { Content = new CategoriesView() });

                tab.Items.Add(years);
                tab.Items.Add(Employees);
                tab.Items.Add(categories);
            }

            return;
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

}