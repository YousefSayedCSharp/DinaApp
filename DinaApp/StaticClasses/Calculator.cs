namespace DinaApp.StaticClasses;

public static class Calculator
{
    public static async Task<int> AllPrinted(FirebaseClient firebase, string tblName, int bookId)
    {
        int PrentedCount = 0;
        try
        {
            List<BookAction> ft = (await firebase
                      .Child(tblName)
                      .OnceAsync<BookAction>()).Select(item => new BookAction
                      {
                          Id = item.Object.Id,
                          BookId = item.Object.BookId,
                          CategoryId = item.Object.CategoryId,
                          EmployeeId = item.Object.EmployeeId,
                          Count = item.Object.Count,
                          Date = item.Object.Date
                      }).ToList();

            if (ft.Count == 0)
                return PrentedCount;

            //PrentedCount = ft.Sum(b=>b.Prented);
            PrentedCount = ft.Where(b => b.BookId == bookId).Sum(b => b.Count);
            //await msg.ShowToast(PrentedCount+"");
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message,"Error");
            //msg(ex.Message);
        }
        return PrentedCount;
    }
}
