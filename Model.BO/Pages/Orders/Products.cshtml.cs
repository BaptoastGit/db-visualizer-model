using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Model.BO.Data.Model;
using Model.BO.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Model.BO.Pages.Orders
{

    public class ProductsModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {

        public List<Product> dataList = [];
        public string successMessage;
        public Dictionary<string, string> successMessageList = [];


        public async Task<IActionResult> OnGetAsync(string sortBy, string Order, string RowCount = "", string Sku = "", string Title = "", string Category = "", List<string> StockQuantity = null, List<string> IsPublished = null, List<string> WeightKg = null, List<string> ModifiedOn = null)
        {
            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderPrices").Result;


            if (authorizationResult.Succeeded)
            {
                try
                {
                dataList = OrdersService.SortAndFilterProductsTable(context, sortBy, Order, RowCount, Sku, Title, Category, StockQuantity, IsPublished, WeightKg, ModifiedOn);
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

        public  IActionResult OnGetTableData(string sortBy, string Order, string RowCount = "", string Sku = "", string Title = "", string Category = "", List<string> StockQuantity = null, List<string> IsPublished = null, List<string> WeightKg = null, List<string> ModifiedOn = null)
        {
            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderPrices").Result;


            if (authorizationResult.Succeeded)
            {
                try
                {
                    dataList = OrdersService.SortAndFilterProductsTable(context, sortBy, Order, RowCount, Sku, Title, Category, StockQuantity, IsPublished, WeightKg, ModifiedOn);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
                var viewData = new ViewDataDictionary<ProductsModel>(ViewData, this)
                {
                    ["Title"] = "Products"
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
