using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebShopping.Models;
using WebShopping.ModelsView;

namespace WebShopping.Repos.Interface
{
    public interface IAccountRepo
    {
        Admin Login (Login login);
        HttpStatusCode Logout ();
        HttpStatusCode ChangePassword(AdminPassword password);
        HttpStatusCode ChangeName(AdminName name);
        HttpStatusCode ResetAllAdminsPasswords();
        HttpStatusCode ResetAdminPassword(int adminID);
        List<Log> GetLogs();
        HttpStatusCode AddLog(Log log);
    }
}
