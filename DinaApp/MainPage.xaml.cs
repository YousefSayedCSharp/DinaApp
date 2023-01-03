namespace DinaApp;

public partial class MainPage : ContentPage
{
    public MainPage(string msg)
    {
        InitializeComponent();
        lbl.Text = msg;
    }
}

