namespace DinaApp.Helper;

public class BooksCategoryFBH
{
    //new connection and set url
    FirebaseClient firebase = new FirebaseClient(DatabaseURL.url);

    public ObservableCollection<Book> DatabaseItems { get; set; } = new ObservableCollection<Book>();

    //the model name in firebase project database
    string tblName = "Books";

    public ObservableCollection<Book> GetAllBooksByCategory(int categoryId)
    {
        try
        {
            var observable = firebase
          .Child(tblName)
          .AsObservable<Book>().Where(item => item.Object.CategoryId == categoryId)
          .Subscribe(d =>
          {
              if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete)
              {
                  if (DatabaseItems.Where(c => c.Id == d.Object.Id).FirstOrDefault() != null)
                      DatabaseItems.Remove(d.Object);
              }

              if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
              {
                  if (DatabaseItems.Where(c => c.Id == d.Object.Id).FirstOrDefault() != null)
                      DatabaseItems.Remove(d.Object);

                  if (d.Object.CategoryId == categoryId )
                      DatabaseItems.Add(d.Object);
              }

          });
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
        return DatabaseItems;
    }

    //get all of list to get max id
    public async Task<List<Book>> GetAll()
    {
        List<Book> cm = new List<Book>();
        try
        {
            cm = (await firebase
          .Child(tblName)
          .OnceAsync<Book>()).Select(item => new Book
          {
              Id = item.Object.Id,
              BookId = item.Object.BookId,
              Name = item.Object.Name,
              CategoryId = item.Object.CategoryId,
              YearId = item.Object.YearId,
              PaperCount = item.Object.PaperCount,
              Count = item.Object.Count,
              Finish = item.Object.Finish,
              UnFinish = item.Object.UnFinish,
              Date = item.Object.Date,
              IsShow = item.Object.IsShow
          }).ToList();
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
        return cm;
    }

    //add new
    public async Task AddBook(Book model)
    {
        try
        {
            List<Book> books = await GetAll();
            int id = (books.Count == 0 || books == null) ? 1 : books.Max(c => c.Id) + 1;

            if (id == 1)
                await msg.ShowToast(msg.RestartApp, true);

            await firebase
              .Child(tblName)
              .PostAsync(new Book()
              {
                  Id = id,
                  BookId = model.BookId,
                  Name = model.Name,
                  CategoryId = model.CategoryId,
                  YearId = model.YearId,
                  PaperCount = model.PaperCount,
                  Count = model.Count,
                  Finish = 0,
                  UnFinish = model.UnFinish,
                  Date = model.Date,
                  IsShow = model.IsShow
              });
            //await App.Current.MainPage.DisplayAlert("", DatabaseItems.Count + "", "OK");
            await msg.ShowToast(msg.AddSuccessfully);
            //await msg.ShowToast(cats.Count+"");
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

    //get one item of list
    public async Task<Book> GetBook(int id)
    {
        var toGetBook = (await firebase
          .Child(tblName)
          .OnceAsync<Book>()).Where(a => a.Object.Id == id).FirstOrDefault();
        if (toGetBook == null)
            return null;

        return toGetBook.Object;
    }

    //update
    public async Task UpdateBook(int id, Book model)
    {
        try
        {
            //if (id == 1)
            //{
            //    await msg.ShowToast(msg.NoEditOrDelete, true);
            //    return;
            //}

            var ToUpdateBook = (await firebase
          .Child(tblName)
          .OnceAsync<Book>()).Where(a => a.Object.Id == id).FirstOrDefault();

            int prented = await Calculator.AllPrinted(firebase, "Printeds", model.Id);
            //await App.Current.MainPage.DisplayAlert("", prented+"","OK");
            await firebase
              .Child(tblName)
              .Child(ToUpdateBook.Key)
              .PutAsync(new Book()
              {
                  Id = id,
                  BookId = model.BookId,
                  Name = model.Name,
                  CategoryId = model.CategoryId,
                  YearId = model.YearId,
                  PaperCount = model.PaperCount,
                  Count = model.Count,
                  Finish = prented,
                  UnFinish = model.Count - prented,
                  Date = model.Date,
                  IsShow = model.IsShow
              });
            await msg.ShowToast(msg.EditSuccessfully);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

    //delete
    public async Task DeleteBook(int id)
    {
        try
        {
            //if (id == 1)
            //{
            //    await msg.ShowToast(msg.NoEditOrDelete, true);
            //    return;
            //}

            var toDeleteBook = (await firebase
          .Child(tblName)
          .OnceAsync<Book>()).Where(a => a.Object.Id == id).FirstOrDefault();

            await firebase.Child(tblName).Child(toDeleteBook.Key).DeleteAsync();
            await msg.ShowToast(msg.DeleteSuccessfully);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

}//end cclass
