using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class AddNewModel : PageModel {
    public AddNewModel() {

    }

    [BindProperty]
    public string adminTask { get; set; }

    [BindProperty]
    public NewModel New { get; set; }

    public IActionResult OnPost() {
        if(ModelState.IsValid) {
            if(adminTask == "addNewNew") {
                try
                {
                    String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";
                    using(NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                         connection.Open();
                         String sql = "INSERT INTO news VALUES (@name, @author, @date, @image)";
                         using(NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@name", New.name);
                            command.Parameters.AddWithValue("@author", New.author);
                            command.Parameters.AddWithValue("@date", New.date);
                            command.Parameters.AddWithValue("@image", New.image);
                            command.ExecuteNonQuery();
                         }   
                    }
                    return RedirectToPage("Admin");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
            }
        }
        return Page();
    }
}