using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Model.BO.Data.Model;
using Model.BO.Service;


namespace Model.BO.Pages.Messages
{

    public class MessagesModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {
        public List<Message> dataList = [];
        public async Task<IActionResult> OnGetAsync(string sortBy, string Order, string RowCount = "", string ErrorMessage = "", List<string> PayloadSize = null, string Priority = "", List<string> ProcessedOn = null, string CorrelationId = "")
        {

            var authorizationResult = await authorizationService.AuthorizeAsync(User, "RequireReaderMessages");

            if (authorizationResult.Succeeded)
            {
                try
                {
                dataList = MessagesService.SortAndFilterMessagesTable(context, sortBy, Order, RowCount, ErrorMessage, PayloadSize, Priority, ProcessedOn, CorrelationId);
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

        public IActionResult OnGetTableData(string sortBy, string Order, string RowCount = "", string ErrorMessage = "", List<string> PayloadSize = null, string Priority = "", List<string> ProcessedOn = null, string CorrelationId = "")
        {

            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderMessages").Result;

            if (authorizationResult.Succeeded)
            {
                try
                {
                    dataList = MessagesService.SortAndFilterMessagesTable(context, sortBy, Order, RowCount, ErrorMessage, PayloadSize, Priority, ProcessedOn, CorrelationId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
                var viewData = new ViewDataDictionary<MessagesModel>(ViewData, this)
                {
                    ["Title"] = "Messages"
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
