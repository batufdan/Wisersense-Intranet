using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class AddPlanModel : PageModel {
    private readonly ILogger<AddPlanModel> _logger;

    public AddPlanModel(ILogger<AddPlanModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public string adminTask { get; set; }

    [BindProperty]
    public PlanModel Plan { get; set; }

    public IActionResult OnPost() {
        if(ModelState.IsValid) {
            if(adminTask == "addNewPlan") {
                try
                {
                    String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";
                    using(NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                         connection.Open();
                         String sql = "INSERT INTO plans VALUES (@name, @date, @time, @type)";
                         using(NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@name", Plan.name);
                            command.Parameters.AddWithValue("@date", Plan.date);
                            command.Parameters.AddWithValue("@time", Plan.time);
                            command.Parameters.AddWithValue("@type", Plan.type);
                            command.ExecuteNonQuery();
                         }   
                    }
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