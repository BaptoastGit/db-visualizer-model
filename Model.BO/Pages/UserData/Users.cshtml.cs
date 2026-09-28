using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model.BO.Data.Model;
using Model.BO.Service;
using System.Text.Json.Nodes;
using System.Xml.Linq;


namespace Model.BO.Pages.UserData
{

    public class UsersModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {
        public List<dynamic> dataList = [];
        public async Task<IActionResult> OnGetAsync(string TableName = "Users")
        {

            var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReaderUsers");

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

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderUsers").Result;

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
