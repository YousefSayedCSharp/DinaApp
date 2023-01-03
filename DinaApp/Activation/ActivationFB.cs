namespace DinaApp.Activation;

public class ActivationFB
{
    //new connection and set url
    //AppManagement
    FirebaseClient firebase = new FirebaseClient(DatabaseURL.ActivationURL);

    //the model name in firebase project database
    string tblName = "MyApps";

    public async Task<AppModel> GetActivation()
    {
        try
        {
            var GetApp = (await firebase
          .Child(tblName)
          .OnceAsync<AppModel>()).Where(a => a.Object.AppId == MyAppId.AppId).FirstOrDefault();

            return GetApp.Object;
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        return null;
    }

}
