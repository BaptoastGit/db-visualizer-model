using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Model.BO.Data.Model;
using Model.BO.Service;
using System.Xml.Linq;


namespace Model.BO.Pages.UserData
{

    public class UsersModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {
        public List<User> dataList = [];
        public async Task<IActionResult> OnGetAsync(string sortBy, string Order, string RowCount, string Id="", string Email = "", string Username= "",  List<string> CreatedOn = null , List<string> FailedLoginAttempts = null, string PhoneNumber = "", string InternalNotes = "")
        {

            var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReaderUsers");

            if (authorizationResult.Succeeded)
            {
                try
                {
                dataList = UsersDataService.SortAndFilterUsersTable(context, sortBy, Order, RowCount, Id, Email, Username, CreatedOn, FailedLoginAttempts, PhoneNumber, InternalNotes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
                return Page();
            }
            else
            {
                return Redirect("/AccessDenied");
            }
        }

        public IActionResult OnGetTableData(string sortBy, string Order, string RowCount, string Id = "", string Email = "", string Username = "", List<string> CreatedOn = null, List<string> FailedLoginAttempts = null, string PhoneNumber = "", string InternalNotes = "")
        {

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderUsers").Result;

            if (authorizationResult.Succeeded)
            {
                try
                {
                    dataList = UsersDataService.SortAndFilterUsersTable(context, sortBy, Order, RowCount, Id, Email, Username, CreatedOn, FailedLoginAttempts, PhoneNumber, InternalNotes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
                var viewData = new ViewDataDictionary<UsersModel>(ViewData, this)
                {
                    ["Title"] = "Users"
                };

                return new PartialViewResult
                {
                    ViewName = "_TableRowsPartial",
                    ViewData = viewData

                };
            }
            else
            {
                return Redirect("/AccessDenied");
            }
        }

    }
}
