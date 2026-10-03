using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Model.BO.Data.Model;
using Model.BO.Service;
using System.Text.Json.Nodes;


namespace Model.BO.Pages.TablePages
{

    public class TableDashBoardModel(ModelDbContext context, IAuthorizationService authorizationService, IConfiguration configuration) : PageModel
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

            var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReader" + TableName);
            if (!authorizationResult.Succeeded)
            {
                return RedirectToPage("/AccessDenied");

            }
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

        public IActionResult OnGetTableData(string TableName = "Accounts")
        {

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReader" + TableName).Result;
            if (!authorizationResult.Succeeded)
            {
                return RedirectToPage("/AccessDenied");

            }
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
                var viewData = new ViewDataDictionary<dynamic>(ViewData, this)
                {
                    ["Title"] = jsonPayload["TableName"]
                };

                return new PartialViewResult
                {
                    ViewName = "_TableRowsPartial",
                    ViewData = viewData

                };
        }

    

        

        public IActionResult OnPost(List<string> selectedFiles, string ActionName, string TableName = "Accounts")
        {

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireEditor" + TableName).Result;
            if (!authorizationResult.Succeeded)
            {
                return RedirectToPage("/AccessDenied");

            }
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

                return RedirectToPage("/TablePages/TableDashboard", new { success = successMessage, error = errorMessage });

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

