namespace DinaApp.Helper;

public class CategoriesFBH
{
    //new connection and set url
    FirebaseClient firebase = new FirebaseClient(DatabaseURL.url);

    public ObservableCollection<Category> DatabaseItems { get; set; } = new ObservableCollection<Category>();

    //the model name in firebase project database
    string tblName = "Categories";

    //connect real time 
    public ObservableCollection<Category> GetAllCategories()
    {
        try
        {
            var observable = firebase
          .Child(tblName)
          .AsObservable<Category>()
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

                  DatabaseItems.Add(d.Object);
              }
              //DatabaseItems = (ObservableCollection<Category>)DatabaseItems.OrderByDescending(c=>c.Id);
          });
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
        return DatabaseItems;
    }

    //get all of list to get max id
    public async Task<List<Category>> GetAll()
    {
        List<Category> cm = new List<Category>();
        try
        {
            cm = (await firebase
          .Child(tblName)
          .OnceAsync<Category>()).Select(item => new Category
          {
              Id = item.Object.Id,
              Name = item.Object.Name
          }).ToList();
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
        return cm;
    }

    //add new
    public async Task AddCategory(Category model)
    {
        try
        {
            List<Category> cats = await GetAll();
            int id = (cats.Count == 0 || cats == null) ? 1 : cats.Max(c => c.Id) + 1;

            if (id == 1)
                await msg.ShowToast(msg.RestartApp, true);

            await firebase
              .Child(tblName)
              .PostAsync(new Category() { Id = id, Name = model.Name });
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
    public async Task<Category> GetCategory(int id)
    {
        var toGetEmp = (await firebase
          .Child(tblName)
          .OnceAsync<Category>()).Where(a => a.Object.Id == id).FirstOrDefault();
        if (toGetEmp == null)
            return null;

        return toGetEmp.Object;
    }

    //update
    public async Task UpdateCategory(int id, Category model)
    {
        try
        {
            if (id == 1)
            {
                await msg.ShowToast(msg.NoEditOrDelete, true);
                return;
            }

            var ToUpdateCategory = (await firebase
          .Child(tblName)
          .OnceAsync<Category>()).Where(a => a.Object.Id == id).FirstOrDefault();

            if (id == 1)
            {
                await msg.ShowToast(msg.NoEditOrDelete, true);
            }

            await firebase
              .Child(tblName)
              .Child(ToUpdateCategory.Key)
              .PutAsync(new Category() { Id = id, Name = model.Name });
            await msg.ShowToast(msg.EditSuccessfully);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

    //delete
    public async Task DeleteCategory(int id)
    {
        try
        {
            if (id == 1)
            {
                await msg.ShowToast(msg.NoEditOrDelete, true);
                return;
            }

            var toDeleteCategory = (await firebase
          .Child(tblName)
          .OnceAsync<Category>()).Where(a => a.Object.Id == id).FirstOrDefault();

            await firebase.Child(tblName).Child(toDeleteCategory.Key).DeleteAsync();
            await msg.ShowToast(msg.DeleteSuccessfully);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

}//end cclass
