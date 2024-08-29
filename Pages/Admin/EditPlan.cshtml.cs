using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class EditPlanModel : PageModel {
    private readonly ILogger<EditPlanModel> _logger;

    public EditPlanModel(ILogger<EditPlanModel> logger)
    {
        _logger = logger;
    }

    [FromQuery]
    public string type { get; set; }

    [FromQuery]
    public string sendName { get; set; }

    public List<PlanModel> Plans = new List<PlanModel>();

    public IActionResult OnGet() {
        if (type == "delete") {
            try
            {
                String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                    connection.Open();

                    String sql = "DELETE FROM plans WHERE name = @name";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@name", sendName);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
        try {
            String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                connection.Open();

                String sql = "SELECT * FROM plans";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                    using(NpgsqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            PlanModel temp = new PlanModel();

                            temp.name = reader.GetString(0);
                            temp.date = reader.GetString(1);
                            temp.time = reader.GetString(2);
                            temp.type = reader.GetString(3);

                            Plans.Add(temp);
                        }
                    }
                }
            }
        } catch (Exception ex){
            Console.WriteLine("Exception: " +ex.ToString());
        }
        return Page();
    }
}