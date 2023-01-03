namespace DinaApp.ModelsVM;

public class BookEmployeesActionMVM : List<BookAction>
{
    public string EmployeeName { get; set; }
    //public string YearTitle { get; set; }

    public BookEmployeesActionMVM(string eName/*,string yTitle*/)
    {
        EmployeeName = eName;
        //YearTitle=yTitle;
    }
}
