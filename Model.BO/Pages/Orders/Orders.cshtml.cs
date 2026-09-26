using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Model.BO.Data.Model;
using Model.BO.Service;


namespace Model.BO.Pages.Orders
{

    public class OrdersModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {

        public List<Order> dataList = [];
        public string successMessage;
        public Dictionary<string, string> successMessageList = [];


        public async Task<IActionResult> OnGetAsync(string sortBy, string Order, string RowCount = "", string OrderNumber = "", string CustomerEmail = "", string OrderStatus = "", string PaymentStatus = "", List<string> TotalAmount = null, string Currency = "", string ShippingMethod = "", string TrackingNumber = "", List<string> ItemCount = null, List<string> OrderedOn = null)
        {
            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderOrders").Result;


            if (authorizationResult.Succeeded)
            {
                try
                {
                dataList = OrdersService.SortAndFilterOrdersTable(context, sortBy, Order, RowCount, OrderNumber, CustomerEmail, OrderStatus, PaymentStatus, TotalAmount, Currency, ShippingMethod, TrackingNumber, ItemCount, OrderedOn);
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

        public  IActionResult OnGetTableData(string sortBy, string Order, string RowCount = "", string OrderNumber = "", string CustomerEmail = "", string OrderStatus = "", string PaymentStatus = "", List<string> TotalAmount = null, string Currency = "", string ShippingMethod = "", string TrackingNumber = "", List<string> ItemCount = null, List<string> OrderedOn = null)
        {
            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderOrders").Result;


            if (authorizationResult.Succeeded)
            {
                try
                {
                    dataList = OrdersService.SortAndFilterOrdersTable(context, sortBy, Order, RowCount, OrderNumber, CustomerEmail, OrderStatus, PaymentStatus, TotalAmount, Currency, ShippingMethod, TrackingNumber, ItemCount, OrderedOn);
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
