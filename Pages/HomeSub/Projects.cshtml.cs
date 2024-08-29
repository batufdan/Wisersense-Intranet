using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages;

public class ProjectsModel : PageModel {
    public ProjectsModel() {
        
    }

    [BindProperty]
    public string homeTask { get; set; }

    public IActionResult OnPost() {
        if(ModelState.IsValid) {
            if(homeTask == "goback") {
                return RedirectToPage("../Home");
            }
            if(homeTask == "logout") {
                HttpContext.Session.Clear();
                return RedirectToPage("../Index");
            }
        }
        return Page();
    }
}