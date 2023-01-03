namespace DinaApp.Helper;

public class FinishBooksActionFBH
{
    //new connection and set url
    FirebaseClient firebase = new FirebaseClient(DatabaseURL.url);

    public ObservableCollection<BookAction> DatabaseItems { get; set; } = new ObservableCollection<BookAction>();

    //the model name in firebase project database
    string tblName = "Printeds";

    //get all of list to get max id
    public async Task<List<BookAction>> GetAll()
    {
        List<BookAction> cm = new List<BookAction>();
        try
        {
            cm = (await firebase
          .Child(tblName)
          .OnceAsync<BookAction>()).Select(item => new BookAction
          {
              Id = item.Object.Id,
              BookId = item.Object.BookId,
              CategoryId= item.Object.CategoryId,
              EmployeeId= item.Object.EmployeeId,
              Count= item.Object.Count,
              Date= item.Object.Date
          }).ToList();
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message,"Error");
        }
        return cm;
    }

    //add new
    public async Task AddPrent(BookAction model)
    {
        try
        {
            List<BookAction> prints = await GetAll();
            int id = (prints.Count == 0 || prints == null) ? 1 : prints.Max(c => c.Id) + 1;

            //if (id == 1)
                //await msgToast.ShowToast("الرجاء إغلاق التطبيق وإعادة تشغيله مرة أخرى بعد نجاح هذه الإضافة.", true);

            await firebase
              .Child(tblName)
              .PostAsync(new BookAction()
              {
                  Id = id,
                  BookId = model.BookId,
                  CategoryId = model.CategoryId,
                  EmployeeId= UserData.userId,
                  Count= model.Count,
                  Date=model.Date
              });
            //await App.Current.MainPage.DisplayAlert("", DatabaseItems.Count + "", "OK");
            await msg.ShowToast("تمت الإضافة  بنجاح.");
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message,"Error");
        }
    }

    //Delete all printeds with selected book
    public async Task DeletePrintByBookId(int id)
    {
        try
        {
            var toDeletePrints = (await firebase
          .Child(tblName)
          .OnceAsync<BookAction>()).Where(a => a.Object.BookId == id);

            bool check = false;
            foreach (var item in toDeletePrints)
            {
                await firebase.Child(tblName).Child(item.Key).DeleteAsync();
                check = true;
            }

            if (check)
                await msg.ShowToast("تم حذف كافة المتعلقات الخاصة ب هذا الكتاب بنجاح.", true);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message,"Error");
        }
    }


}
