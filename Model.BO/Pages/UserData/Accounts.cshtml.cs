using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Model.BO.Data.Model;
using Model.BO.Service;
using System.Security.Claims;
using System.Text.Json.Nodes;


namespace Model.BO.Pages.UserData
{

    public class AccountsModel(ModelDbContext context, IAuthorizationService authorizationService, IConfiguration configuration) : PageModel
    {
        private readonly IConfiguration _configuration = configuration;
        public List<dynamic> dataList = [];
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


        public async Task<IActionResult> OnGetAsync(string TableName = "Accounts")
        {

            var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReaderAccounts");

            if (authorizationResult.Succeeded)
            {
                var jsonPayload = new JsonObject();
                foreach (var key in Request.Query.Keys)
                {
                    if (key != "handler" && key != "__RequestVerificationToken")
                    {
                        Console.WriteLine("Key: " + key + " Value: " + Request.Query[key]);
                        jsonPayload[key] += Request.Query[key];
                    }
                }
                bool hasTableName = jsonPayload.ContainsKey("TableName") && !string.IsNullOrEmpty(jsonPayload["TableName"]?.ToString());
                if (!hasTableName)
                {
                    jsonPayload["TableName"] = TableName;
                }
                try
                {
                    dataList = TableDataService.SortAndFilterTable(context, jsonPayload);
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

        public IActionResult OnGetTableData()
        {

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderAccounts").Result;

            if (authorizationResult.Succeeded)
            {
                var jsonPayload = new JsonObject();
                foreach (var key in Request.Query.Keys)
                {
                    if (key != "handler" && key != "__RequestVerificationToken")
                    {
                        Console.WriteLine("Key: " + key + " Value: " + Request.Query[key]);
                        jsonPayload[key] += Request.Query[key];
                    }
                }
                try
                {
                    dataList = TableDataService.SortAndFilterTable(context, jsonPayload);
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
