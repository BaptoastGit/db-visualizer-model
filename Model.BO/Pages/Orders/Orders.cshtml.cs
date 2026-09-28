using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Model.BO.Data.Model;
using Model.BO.Pages.UserData;
using Model.BO.Service;
using System.Text.Json.Nodes;


namespace Model.BO.Pages.Orders
{

    public class OrdersModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {

        public List<dynamic> dataList = [];
        public string successMessage;
        public Dictionary<string, string> successMessageList = [];


        public async Task<IActionResult> OnGetAsync(string TableName = "Orders")
        {

            var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReaderOrders");

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

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderOrders").Result;

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
                var viewData = new ViewDataDictionary<OrdersModel>(ViewData, this)
                {
                    ["Title"] = "Orders"
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

        //public async Task<IActionResult> OnPost()
        //{
        //    string formType = Request.Form["FormType"];
        //    Console.WriteLine("FormType: " + formType);
        //    if (formType == "Delete")
        //    {
        //        var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireAdminContactsPIM").Result;
        //        if (authorizationResult.Succeeded)
        //        {
        //            foreach (var key in Request.Form.Keys)
        //            {
        //                if (key != "FormType" && key != "__RequestVerificationToken")
        //                {
        //                    Console.WriteLine("Key: " + key + " Value: " + Request.Form[key]);
        //                    var entity = context.ContactBases.Find(Guid.Parse(key));
        //                    if (entity != null)
        //                    {
        //                        Console.WriteLine("Deleting entity with key: " + key);
        //                        context.ContactBases.Remove(entity);
        //                    }
        //                }
        //            }

        //        }
        //        context.SaveChanges();
        //        return new OkResult();
        //    }
        //    else if (formType == "LastModificationDate")
        //    {
        //        var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireEditorContactsPIM").Result;
        //        if (authorizationResult.Succeeded)
        //        {
        //            foreach (var key in Request.Form.Keys)
        //            {
        //                if (key != "FormType" && key != "__RequestVerificationToken")
        //                {
        //                    Console.WriteLine("Key: " + key + " Value: " + Request.Form[key]);
        //                    context.ContactBases.Find(Guid.Parse(key))?.LastModificationDate = DateTime.Parse(Request.Form[key]);

        //                }
        //            }

        //        }
        //        context.SaveChanges();
        //        return new OkResult();


        //    }
        //    else
        //    {
        //        return Redirect("/AccessDenied");
        //    }




        //}

    }
}
