using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Model.BO.Data.Model;
using Model.BO.Service;
using System.Security.Claims;


namespace Model.BO.Pages.UserData
{

    public class AccountsModel(ModelDbContext context, IAuthorizationService authorizationService, IConfiguration configuration) : PageModel
    {
        private readonly IConfiguration _configuration = configuration;
        public List<Account> dataList = [];
        public string successMessage;
        public string errorMessage;
        public Dictionary<string, string> successMessageList = new()
        {
            ["verificationSuccess"] = "Verification successful.",
            ["logSuccess"] = "Log successful."
        };

        public Dictionary<string, string> errorMessageList = new()
        {
        };





        public async Task<IActionResult> OnGetAsync(string sortBy, string Order, string RowCount = "", string Id = "", string Name = "", string Type = "", string Status = "", string Tier = "", List<string> Balance = null, List<string> IsVatExempt = null, List<string> CreatedOn = null)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            Console.WriteLine(string.Join(",", roles));

            var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReaderAccounts");

            if (authorizationResult.Succeeded)
            {
                try
                {
                dataList = UsersDataService.SortAndFilterAccountsTable(context, sortBy, Order, RowCount, Id, Name, Type, Status, Tier, Balance, IsVatExempt, CreatedOn);
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

        public IActionResult OnGetTableData(string sortBy, string Order, string RowCount = "", string Id = "", string Name = "", string Type = "", string Status = "", string Tier = "", List<string> Balance = null, List<string> IsVatExempt = null, List<string> CreatedOn = null)
        {

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderAccounts").Result;

            if (authorizationResult.Succeeded)
            {
                try
                {
                    dataList = UsersDataService.SortAndFilterAccountsTable(context, sortBy, Order, RowCount, Id, Name, Type, Status, Tier, Balance, IsVatExempt, CreatedOn);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
                var viewData = new ViewDataDictionary<AccountsModel>(ViewData, this)
                {
                    ["Title"] = "Accounts"
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



        public IActionResult OnPost(List<string> selectedFiles, string ActionName)
        {

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireEditorAccounts").Result;

            if (authorizationResult.Succeeded)
            {
                try
                {
                    Console.WriteLine("Action: " + ActionName);

                    if (ActionName == "VerificationLink")
                    {
                        //add function call here
                        successMessage = "verificationSuccess";
                       
                    }
                    else if (ActionName == "Log")
                    {
                        //add function call here
                        successMessage = "logSuccess";
                    }
                    else
                    {
                        errorMessage = "Action non reconnue.";
                    }

                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }

                return RedirectToPage("/UserData/Accounts", new { success = successMessage, error = errorMessage });

            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }

        }

    }
}
