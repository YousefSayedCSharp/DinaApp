namespace DinaApp.Activation;

public static class CheckActivation
{
    //public static bool logined = false;

    public static async Task<string> Check()
    {
        string msg = "";

        AppModel model = await new ActivationFB().GetActivation();
        //await App.Current.MainPage.DisplayAlert("", "", "OK");
        if (model != null)
        {
            if (!model.IsActive)
            {
                msg = "تم إقاف هذا التطبيق من قبل المطور.\nللاستفسار أو مزيد من المعلومات الرجاء التواصل مع مطور هذا التطبيق.";
                return msg;
            }

            DateTime dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            string finish = model.AppFinish;
            int m = Convert.ToInt32(finish.Split('/')[0]);
            int d = Convert.ToInt32(finish.Split('/')[1]);
            int y = Convert.ToInt32(finish.Split('/')[2]);
            DateTime dtActivation = new DateTime(y, m, d);
            if (dtNow.Date > dtActivation.Date)
            {
                msg = "إنتهت فطرت الإشتراك هذه.\nالرجاء التواصل مع مطور هذا التطبيق للإشتراك في فطرة جديدة.";
                return msg;
            }
            //else
            //msg = " 111 خطأ غير معروف\nالرجاء إغلاق التطبيق وإعادة تشغيله.\nوإذا استمرت المشكلة الرجاء التواصل مع مطور هذا التطبيق.";
        }
        else
            msg = "222 خطأ غير معروف\nالرجاء إغلاق التطبيق وإعادة تشغيله.\nوإذا استمرت المشكلة الرجاء التواصل مع مطور هذا التطبيق.";

        //if (model != null)
        //{
        //    string strDtNow = DateTime.Now.ToString("M/d/yyyy");
        //    DateTime dtNow = Convert.ToDateTime(strDtNow);
        //    DateTime dtActivation = Convert.ToDateTime(model.AppFinish);
        //    //await App.Current.MainPage.DisplayAlert("", dtActivation+"","OK");
        //    //if (dtActivation.Year > dtNow.Year&& dtActivation.Month > dtNow.Month&& dtActivation.Day> dtNow.Day)
        //    //if (dtNow> dtActivation)
        //    //isActive = false;
        //}

        return msg;
    }
}
