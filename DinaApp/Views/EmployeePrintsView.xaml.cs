namespace DinaApp.Views;

public partial class EmployeePrintsView : ContentPage
{
    public Employee emp;
    public EmployeePrintsView(Employee employee)
    {
        InitializeComponent();
        BindingContext = new EmployeePrintsVM(employee);
        emp = employee;
    }

    protected override void OnAppearing()
    {
        //base.OnAppearing();
        //            Xamarin.Forms.MessagingCenter.Subscribe<Page2, string>(this, "SendingMessage", (sender, message) =>
        //MessagingCenter.Subscribe<MainPage,string>(this,"UpdateCollectionView"=>
        MessagingCenter.Subscribe<string, string>("MyApp", "UpdateBinding", (sender, result) =>
        {
            
        });
    }

    protected override void OnDisappearing()
    {
        //base.OnDisappearing();
        MessagingCenter.Unsubscribe<MainPage>(this, "UpdateCollectionView");
    }

}