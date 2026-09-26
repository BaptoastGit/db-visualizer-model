using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOfficeV4.Pages
{
    public class AccessDeniedModel : PageModel
    {
    public IActionResult OnGet()
    {
        return Page();
    }
    }
}
