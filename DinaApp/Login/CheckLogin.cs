namespace DinaApp.Login;

public class CheckLogin
{
    SaveLogin saveLogin;
    public CheckLogin()
    {
        saveLogin = new SaveLogin();
    }

    public async Task<bool> login()
    {
        bool isLogin = false;
        try
        {
            var e = (saveLogin.employees.FirstOrDefault() != null) ? saveLogin.employees.FirstOrDefault() : null;

            if (e != null)
            {
                Employee exist = await new EmployeesFBH().GetEmployee(e.Name, e.Password);
                if (exist != null)
                {
                    UserData.userId = exist.Id;
                    UserData.userName = exist.Name;
                    UserData.userPassword = exist.Password;
                    UserData.userRool = exist.IsAdmin;
                    isLogin = true;
                }
            }
        }
        catch (Exception ex)
        {
            msg.ShowAlertOK(ex.Message, "Error" );
        }
        return isLogin;
    }

}
