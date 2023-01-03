using DinaApp.ViewModels;

namespace DinaApp.Views;

public partial class YearsView : ContentPage
{
    public YearsView()
    {
        InitializeComponent();
        BindingContext = new YearsVM();
    }

    private void CheckBox_Focused(object sender, FocusEventArgs e)
    {

    }
}