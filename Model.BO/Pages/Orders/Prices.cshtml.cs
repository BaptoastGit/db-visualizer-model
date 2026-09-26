using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Model.BO.Data.Model;
using Model.BO.Service;


namespace Model.BO.Pages.Orders
{

    public class PricesModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {

        public List<Price> dataList = [];
        public string successMessage;
        public Dictionary<string, string> successMessageList = [];


        public async Task<IActionResult> OnGetAsync(string sortBy, string Order, string RowCount = "", string Sku = "", string PriceListCode = "", List<string> Amount = null, string Currency = "", List<string> ValidFrom = null, List<string> ValidTo = null)
        {

            //var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReaderServiceImport");

            //if (authorizationResult.Succeeded)
            //{
            try
                {
                Console.WriteLine("sortBy:" + sortBy); Console.WriteLine("order:" + Order); 
                dataList = OrdersService.SortAndFilterPricesTable(context, sortBy, Order, RowCount, Sku, PriceListCode, Amount, Currency, ValidFrom, ValidTo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
                return Page();
            //}
            //else
            //{
            //    return Redirect("/AccessDenied");
            //}
        }

        public IActionResult OnGetTableData(string sortBy, string Order, string RowCount = "", string Sku = "", string PriceListCode = "", List<string> Amount = null, string Currency = "", List<string> ValidFrom = null, List<string> ValidTo = null)
        {

            //var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderServiceImport").Result;

            //if (authorizationResult.Succeeded)
            //{
            try
                {
                    Console.WriteLine("sortBy:" + sortBy); Console.WriteLine("order:" + Order);
                dataList = OrdersService.SortAndFilterPricesTable(context, sortBy, Order, RowCount, Sku, PriceListCode, Amount, Currency, ValidFrom, ValidTo);
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
            //}
            //else
            //{
            //    return Redirect("/AccessDenied");
            //}
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
