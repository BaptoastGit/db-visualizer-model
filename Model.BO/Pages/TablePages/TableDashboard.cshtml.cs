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
        public List<dynamic> dataList = [];

        public int totalCount = 0;
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
                (dataList, totalCount) = TableDataService.SortAndFilterTable(context, jsonPayload);
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
                        jsonPayload[key] += Request.Query[key];
                    }
                }
                try
                {
                    (dataList, totalCount) = TableDataService.SortAndFilterTable(context, jsonPayload);
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

    

        

        public IActionResult OnPost(List<string> selectedFiles, string ActionName, string TableName = "Accounts", string FormType = "")
        {
            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireEditor" + TableName).Result;
            if (!authorizationResult.Succeeded)
            {
                return RedirectToPage("/AccessDenied");

            }
            Console.WriteLine("FormType: " + FormType);

            string entityName = TableName?.TrimEnd('s', 'S') ?? "Accounts";
            var entityType = (context.Model.GetEntityTypes()
                    .FirstOrDefault(e => e.ClrType.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase))?.ClrType) ?? throw new InvalidOperationException($"L'entité '{entityName}' est introuvable dans le DbContext.");

            if (FormType == "Delete")
            {
                authorizationResult = authorizationService.AuthorizeAsync(User, "RequireAdmin" + TableName).Result;
                if (authorizationResult.Succeeded)
                {
                    foreach (var key in Request.Form.Keys)
                    {
                        if (key != "TableName" && key != "FormType" && key != "__RequestVerificationToken")
                        {
                            object? entity;
                            if (Guid.TryParse(key, out Guid guidKey))
                            {
                                entity = context.Find(entityType, Guid.Parse(key));

                            }
                            else
                            {
                                entity = context.Find(entityType, key);
                            }
                            if (entity != null)
                            {
                                Console.WriteLine("Deleting entity with key: " + key);
                                context.Remove(entity);
                            }
                        }
                    }

                }

                context.SaveChanges();
                return new OkResult();
            }
            if (entityType.GetProperty(FormType) != null)
            {
                var propertyType = entityType.GetProperty(FormType).PropertyType;
                if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
                {

                    foreach (var key in Request.Form.Keys)
                    {

                        if (key != "TableName" && key != "FormType" && key != "__RequestVerificationToken")
                        {
                            object? entity;
                            if (Guid.TryParse(key, out Guid guidKey))
                            {
                                entity = context.Find(entityType, guidKey);

                            }
                            else
                            {
                                entity = context.Find(entityType, key);
                            }

                            if (entity != null)
                            {
                                var targetProperty = entityType.GetProperty(FormType);
                                Console.WriteLine("Updating "+ targetProperty + " entity with key: " + key);
                                targetProperty.SetValue(entity, DateTime.Parse(Request.Form[key]));
                            }

                        }
                    }
                }
                if (propertyType == typeof(int?) || propertyType == typeof(int))
                {

                    foreach (var key in Request.Form.Keys)
                    {

                        if (key != "TableName" && key != "FormType" && key != "__RequestVerificationToken")
                        {
                            object? entity;
                            if (Guid.TryParse(key, out Guid guidKey))
                            {
                                entity = context.Find(entityType, guidKey);

                            }
                            else
                            {
                                entity = context.Find(entityType, key);
                            }

                            if (entity != null)
                            {
                                var targetProperty = entityType.GetProperty(FormType);
                                Console.WriteLine("Updating " + targetProperty + " entity with key: " + key);
                                targetProperty.SetValue(entity, int.Parse(Request.Form[key]));
                            }

                        }
                    }
                }
                else
                {
                    foreach (var key in Request.Form.Keys)
                    {

                        if (key != "TableName" && key != "FormType" && key != "__RequestVerificationToken")
                        {
                            object? entity;
                            if (Guid.TryParse(key, out Guid guidKey))
                            {
                                entity = context.Find(entityType, Guid.Parse(key));

                            }
                            else
                            {
                                entity = context.Find(entityType, key);
                            }
                            if (entity != null)
                            {
                                var targetProperty = entityType.GetProperty(FormType);
                                Console.WriteLine("Updating " + targetProperty + " entity with key: " + key);
                                targetProperty.SetValue(entity, Request.Form[key].ToString());
                            }

                        }
                    }
                }

                context.SaveChanges();
                return new OkResult();
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


    }
}

