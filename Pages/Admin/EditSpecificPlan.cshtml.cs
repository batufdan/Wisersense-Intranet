using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class EditSpecificPlanModel : PageModel {
    private readonly ILogger<EditSpecificPlanModel> _logger;

    public EditSpecificPlanModel(ILogger<EditSpecificPlanModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public string adminTask { get; set; }

    [FromQuery]
    public string sendName { get; set; }

    [BindProperty]
    public PlanModel Plan { get; set; }

    public PlanModel temp = new PlanModel();


    public void OnGet() {
        try {
            String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                connection.Open();

                String sql = "SELECT * FROM plans where name = @name";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@name", sendName);
                    using(NpgsqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            temp.name = reader.GetString(0);
                            temp.date = reader.GetString(1);
                            temp.time = reader.GetString(2);
                            temp.type = reader.GetString(3);
                        }
                    }
                }
            }
        } catch (Exception ex){
            Console.WriteLine("Exception: " +ex.ToString());
        }
    }

    public IActionResult OnPost() {
        if(ModelState.IsValid) {
            if(adminTask == "updatePlan") {
                try {
                    String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                        connection.Open();

                        String sql = "UPDATE plans SET date = @date, time = @time, type = @type WHERE name = @name";
                        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@name", Plan.name);
                            command.Parameters.AddWithValue("@date", Plan.date);
                            command.Parameters.AddWithValue("@time", Plan.time);
                            command.Parameters.AddWithValue("@type", Plan.type);
                            
                            command.ExecuteNonQuery();
                        }
                    }
                } catch (Exception ex){
                    Console.WriteLine("Exception: " +ex.ToString());
                }
            }
        }
        return Page();
    }
}