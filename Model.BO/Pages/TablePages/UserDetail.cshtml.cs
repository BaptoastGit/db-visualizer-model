using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.BO.Data.Model;




namespace Model.BO.Pages.TablePages
{
    public class UserDetailModel(ModelDbContext context, IAuthorizationService authorizationService) : PageModel
    {
        public User userInfo;
        public string errorMessage = "";
        public string successMessage = "";
        public Dictionary<string, string> successMessageList = new()
        {
        };
        public IActionResult OnGet()
        {
            var authorizationResult = authorizationService.AuthorizeAsync(User, "RequireReaderUsers").Result;

            if (authorizationResult.Succeeded)
            {
                string id = Request.Query["Id"];

                    try
                    {
                    userInfo = context.Users.Where(c => id == null || c.Id == Guid.Parse(id)).FirstOrDefault();
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
