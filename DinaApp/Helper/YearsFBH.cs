namespace DinaApp.Helper;

public class YearsFBH
{
    //new connection and set url
    FirebaseClient firebase = new FirebaseClient(DatabaseURL.url);

    public ObservableCollection<Year> DatabaseItems { get; set; } = new ObservableCollection<Year>();

    //the model name in firebase project database
    string tblName = "Years";

    //connect real time 
    public ObservableCollection<Year> GetAllYears()
    {
        try
        {
            var observable = firebase
            .Child(tblName)
          .AsObservable<Year>()
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

                  //if (d.Object.IsShow&&b)
                      DatabaseItems.Add(d.Object);
              }
              
          });
            DatabaseItems.Distinct();
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
        return DatabaseItems;
    }

    public ObservableCollection<Year> GetAllIsShow()
    {
        try
        {
            var observable = firebase
          .Child(tblName)
          .AsObservable<Year>().Where(item => item.Object.IsShow)
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

                  if (d.Object.IsShow )
                      DatabaseItems.Add(d.Object);
              }
              
          });
            DatabaseItems.Distinct();
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
        return DatabaseItems;
    }

    //get all of list to get max id
    public async Task<List<Year>> GetAll()
    {
        List<Year> cm = new List<Year>();
        try
        {
            cm = (await firebase
          .Child(tblName)
          .OnceAsync<Year>()).Select(item => new Year
          {
              Id = item.Object.Id,
              Title= item.Object.Title,
              IsShow= item.Object.IsShow
          }).ToList();
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
        return cm;
    }

    //add new
    public async Task AddYear(Year model)
    {
        try
        {
            List<Year> years = await GetAll();
            int id = (years.Count == 0 || years == null) ? 1 : years.Max(c => c.Id) + 1;

            if (id == 1)
                await msg.ShowToast(msg.RestartApp, true);

            await firebase
              .Child(tblName)
              .PostAsync(new Year() { Id = id, Title= model.Title,IsShow=model.IsShow});
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
    public async Task<Year> GetYear(int id)
    {
        var toGetYear = (await firebase
          .Child(tblName)
          .OnceAsync<Year>()).Where(a => a.Object.Id == id).FirstOrDefault();
        if (toGetYear == null)
            return null;

        return toGetYear.Object;
    }

    //update
    public async Task UpdateYear(int id, Year model)
    {
        try
        {
            if (id == 1)
            {
                await msg.ShowToast(msg.NoEditOrDelete, true);
                return;
            }

            var ToUpdateYear = (await firebase
          .Child(tblName)
          .OnceAsync<Year>()).Where(a => a.Object.Id == id).FirstOrDefault();

            await firebase
              .Child(tblName)
              .Child(ToUpdateYear.Key)
              .PutAsync(new Year() { Id = id, Title= model.Title,IsShow=model.IsShow});
            await msg.ShowToast(msg.EditSuccessfully);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

    //delete
    public async Task DeleteYear(int id)
    {
        try
        {
            if (id == 1)
            {
                await msg.ShowToast(msg.NoEditOrDelete, true);
                return;
            }

            var toDeleteYear = (await firebase
          .Child(tblName)
          .OnceAsync<Year>()).Where(a => a.Object.Id == id).FirstOrDefault();

            await firebase.Child(tblName).Child(toDeleteYear.Key).DeleteAsync();
            await msg.ShowToast(msg.DeleteSuccessfully);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

}//end cclass
