namespace DinaApp.Login;

public class SaveLogin
{
    public List<Employee> employees { get; set; } = new List<Employee>();

    string filePath = "";

    public SaveLogin()
    {
        filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EmployeeLogin.xml");

        if (File.Exists(filePath))
            Read();
        else
            Write();

    }

    public void Read()
    {
        var rawData = File.ReadAllText(filePath);
        employees = JsonSerializer.Deserialize<List<Employee>>(rawData);
    }

    public void Write()
    {
        var serializedData = JsonSerializer.Serialize(employees);
        File.WriteAllText(filePath, serializedData);
    }

    public void Add(Employee e)
    {
        int id = (employees.Count > 0) ? employees.Max(c => c.Id) + 1 : 1;
        e.Id = id;
        employees.Add(e);
        Write();
    }

}
