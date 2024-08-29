using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.pages;

public class AdminModel : PageModel {
    private readonly ILogger<AdminModel> _logger;

    [BindProperty]
    public string adminTask { get; set; }

    public AdminModel(ILogger<AdminModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnPost() {
        if(ModelState.IsValid) {
            if(adminTask == "AddUser") {
                Console.WriteLine("AddUser is clicked");
                return RedirectToPage("AddUser");
            }
            if(adminTask == "EditUser") {
                Console.WriteLine("EditUser is clicked");
                return RedirectToPage("EditUser");
            }
            if(adminTask == "AddPlan") {
                Console.WriteLine("AddPlan is clicked");
                return RedirectToPage("AddPlan");
            }
            if(adminTask == "EditPlan") {
                Console.WriteLine("EditPlan is clicked");
                return RedirectToPage("EditPlan");
            }
            if(adminTask == "AddNew") {
                Console.WriteLine("AddNew is clicked");
                return RedirectToPage("AddNew");
            }
            if(adminTask == "EditNews") {
                Console.WriteLine("EditNews is clicked");
                return RedirectToPage("EditNews");
            }
        }
        return Page();
    }
}