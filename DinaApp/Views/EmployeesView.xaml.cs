namespace DinaApp.Views;

public partial class EmployeesView : ContentPage
{
    public EmployeesView()
    {
        InitializeComponent();
        BindingContext = new EmployeesVM();
    }
}