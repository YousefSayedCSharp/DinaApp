namespace DinaApp.StaticClasses;

public static class msg
{

    public static string AddSuccessfully = "تمت الإضافة بنجاح.";
    public static string EditSuccessfully = "تم التعديل بنجاح.";
    public static string DeleteSuccessfully = "تم الحذف بنجاح.";
    public static string NoEditOrDelete = "لا يمكن حذفه أو التعديل عليهي بشكل كامل لأنه مرتبط بقاعدة البيانات مباشرة.";
    public static string RestartApp = "الرجاء إغلاق التطبيق وإعادة تشغيله مرة أخرى بعد نجاح الإضافة.";
    public static string msgDelete = "سيتم الحذف نهائية,\nلتأكيد عملية الحذف هذه الرجاء الضغت على حذف وللتراجع الضغت على إلغاء ؟";

    public static async Task ShowToast(string msg, bool IsLong = false)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        string text = msg;

        ToastDuration duration = ToastDuration.Short;
        if (IsLong)
            duration = ToastDuration.Long;

        double fontSize = 14;

        var toast = Toast.Make(text, duration, fontSize);

        await toast.Show(cancellationTokenSource.Token);
    }

    public static async void ShowAlertOK(string m,string t)
    {
        if (m.ToLower().Contains(DatabaseURL.url))
            m = "الرجاء التحقق من إتصال الإنترنيت وإعادة المحاولة!";

        await App.Current.MainPage.DisplayAlert(t, m, "OK");
    }

    public static async Task<bool> ShowAlertDeleteCancel(string m,string title )
    {
        bool b = false;

        if (await App.Current.MainPage.DisplayAlert(title, m, "حذف", "إلغاء"))
            b = true;

            return b;
    }

    public static async Task<bool> ShowAlertYesNo(string m, string title="")
    {
        bool b = false;

        if (await App.Current.MainPage.DisplayAlert(title, m, "نعم", "لا"))
            b = true;

        return b;
    }

}
