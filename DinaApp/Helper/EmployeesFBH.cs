namespace DinaApp.Helper;

public class EmployeesFBH
{
    //new connection and set url
    FirebaseClient firebase = new FirebaseClient(DatabaseURL.url);

    public ObservableCollection<Employee> DatabaseItems { get; set; } = new ObservableCollection<Employee>();

    //the model name in firebase project database
    string tblName = "Employees";

    //connect real time 
    public ObservableCollection<Employee> GetAllEmployees()
    {
        try
        {
            var observable = firebase
          .Child(tblName)
          .AsObservable<Employee>()
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
    public async Task<List<Employee>> GetAll()
    {
        List<Employee> cm = new List<Employee>();
        try
        {
            cm = (await firebase
          .Child(tblName)
          .OnceAsync<Employee>()).Select(item => new Employee
          {
              Id = item.Object.Id,
              Name= item.Object.Name,
              Password= item.Object.Password,
              IsAdmin= item.Object.IsAdmin,
              IsActive = item.Object.IsActive,
              AppId= item.Object.AppId
          }).ToList();
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
        return cm;
    }

    //add new
    public async Task AddEmployee(Employee model)
    {
        try
        {
            List<Employee> Employees = await GetAll();
            int id = (Employees.Count == 0 || Employees == null) ? 1 : Employees.Max(c => c.Id) + 1;

            if (id == 1)
                await msg.ShowToast(msg.RestartApp, true);

            await firebase
              .Child(tblName)
              .PostAsync(new Employee()
              { 
                  Id = id, 
                  Name= model.Name, 
                  Password = model.Password, 
                  IsAdmin= model.IsAdmin,
                  IsActive = model.IsActive,
                  AppId= model.AppId
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
    public async Task<Employee> GetEmployee(string name,string pass)
    {
        var toGetYear = (await firebase
          .Child(tblName)
          .OnceAsync<Employee>()).Where(a => a.Object.Name== name&&a.Object.Password==pass).FirstOrDefault();
        if (toGetYear == null)
            return null;

        return toGetYear.Object;
    }

    //update
    public async Task UpdateEmployee(int id, Employee model)
    {
        try
        {
            if (id == 1)
            {
                await msg.ShowToast(msg.NoEditOrDelete, true);
                return;
            }

            var ToUpdateEmployee = (await firebase
          .Child(tblName)
          .OnceAsync<Employee>()).Where(a => a.Object.Id == id).FirstOrDefault();

            await firebase
              .Child(tblName)
              .Child(ToUpdateEmployee.Key)
              .PutAsync(new Employee()
              { 
                  Id = id, 
                  Name= model.Name, 
                  Password = model.Password, 
                  IsAdmin= model.IsAdmin,
                  IsActive = model.IsActive,
                  AppId= model.AppId
              });
            await msg.ShowToast(msg.EditSuccessfully);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

    //delete
    public async Task DeleteEmployee(int id)
    {
        try
        {
            if (id == 1)
            {
                await msg.ShowToast(msg.NoEditOrDelete, true);
                return;
            }

            var toDeleteEmployee = (await firebase
          .Child(tblName)
          .OnceAsync<Employee>()).Where(a => a.Object.Id == id).FirstOrDefault();

            await firebase.Child(tblName).Child(toDeleteEmployee.Key).DeleteAsync();
            await msg.ShowToast(msg.DeleteSuccessfully);
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error");
        }
    }

}//end cclass
