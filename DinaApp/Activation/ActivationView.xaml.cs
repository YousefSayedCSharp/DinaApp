namespace DinaApp.Activation;

public partial class ActivationView : ContentPage
{
    public ActivationView(string msg)
    {
        InitializeComponent();
        lbl.Text = msg;
    }
}