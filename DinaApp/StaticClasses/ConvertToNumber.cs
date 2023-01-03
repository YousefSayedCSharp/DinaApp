namespace DinaApp.StaticClasses;

public static class ConvertToNumber
{

    public static int GetNumberOnly(string text)
    {
        string NewText = "";
        foreach (char c in text.ToCharArray())
        {
            if ((c == '0') || (c == '1') || (c == '2') || (c == '3') || (c == '4') || (c == '5') || (c == '6') || (c == '7') || (c == '8') || (c == '9'))
                NewText += c;
        }
        if (NewText == "") NewText = "0";
        int NewInt = Convert.ToInt32(NewText);
        return NewInt;
    }

    public static string toEnglishNumber(string input)
    {
        string EnglishNumbers = "";

        for (int i = 0; i < input.Length; i++)
        {
            if (Char.IsDigit(input[i]))
            {
                EnglishNumbers += char.GetNumericValue(input, i);
            }
            else
            {
                EnglishNumbers += input[i].ToString();
            }
        }
        return EnglishNumbers;
    }

    public static int UseConverter(string str)
    {
        string convertToEn = toEnglishNumber(str);
        return GetNumberOnly(convertToEn);
    }

}
