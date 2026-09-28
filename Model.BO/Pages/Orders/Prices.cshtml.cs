using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Model.BO.Data.Model;
using Model.BO.Service;
using System.Text.Json.Nodes;


namespace Model.BO.Pages.Orders
{

    public class PricesModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {

        public List<dynamic> dataList = [];
        public string successMessage;
        public Dictionary<string, string> successMessageList = [];


        public async Task<IActionResult> OnGetAsync(string TableName = "Prices")
        {

            var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReaderPrices");

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

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderPrices").Result;

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
                var viewData = new ViewDataDictionary<PricesModel>(ViewData, this)
                {
                    ["Title"] = "Prices"
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

        //public IActionResult OnPost()
        //{
        //    //var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireEditorServiceImport").Result;

        //    //if (authorizationResult.Succeeded)
        //    //{
        //        foreach (var key in Request.Form.Keys)
        //        {
        //            Console.WriteLine("Key: " + key + " Value: " + Request.Form[key]);
        //            if (context.ServiceImports.Find(key)?.Value != null)
        //            {
        //                context.ServiceImports.Find(key)?.Value = Request.Form[key];
        //            }
        //        }
        //        context.SaveChanges();


        //        try
        //        {
        //            dataList = [.. context.ServiceImports];
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Exception: " + ex.ToString());
        //        }
        //        return Page();
        //    //}
        //    //else
        //    //{
        //    //    return Redirect("/AccessDenied");

        //    //}

        //}
    }
}
