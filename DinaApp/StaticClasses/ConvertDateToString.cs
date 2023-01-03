namespace DinaApp.StaticClasses;

public static class ConvertDateToString
{
    public static string dateToString(DateTime date)
    {
        string strDate = "";
        int month = date.Month;
        int day = date.Day;
        int year = date.Year;
        strDate = month + "/" + day + "/" + year;

        return strDate;
    }

    public static DateTime stringToDate(string date)
    {
        if (string.IsNullOrEmpty(date))
            return DateTime.Now;

        int month = Convert.ToInt32(date.Split('/')[0]);
        int day = Convert.ToInt32(date.Split('/')[1]);
        int year = Convert.ToInt32(date.Split('/')[2]);

        return new DateTime(year, month, day);
    }
}
