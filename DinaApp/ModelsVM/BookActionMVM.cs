namespace DinaApp.ModelsVM;

public class BookActionMVM:List<BookAction>
{
    public string BookName { get; set; }
    //public string YearTitle { get; set; }

    public BookActionMVM(string bName/*,string yTitle*/)
    {
        BookName = bName;
        //YearTitle=yTitle;
    }
}
