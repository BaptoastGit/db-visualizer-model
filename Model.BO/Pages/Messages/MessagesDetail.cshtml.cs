using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.BO.Data.Model;




namespace Model.BO.Pages.Messages
{
    public class MessagesDetailModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {
        public Message messageInfo;
        public string errorMessage = "";
        public string successMessage = "";
        public Dictionary<string, string> successMessageList = new()
        {
        };
        public IActionResult OnGet()
        {
            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderMessages").Result;

            if (authorizationResult.Succeeded)
            {
                string id = Request.Query["Id"];

                    try
                    {
                    messageInfo = context.Messages.FirstOrDefault();
                    }

                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                
                return Page();

            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }
        }




        
    }
}
