using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class EditNewsModel : PageModel {
    public EditNewsModel() {

    }

    [FromQuery]
    public string type { get; set; }

    [FromQuery]
    public string sendName { get; set; }

    public List<NewModel> News = new List<NewModel>();

    public IActionResult OnGet() {
        if (type == "delete") {
            try
            {
                String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                    connection.Open();

                    String sql = "DELETE FROM news WHERE name = @name";
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

                String sql = "SELECT * FROM news";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                    using(NpgsqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            NewModel temp = new NewModel();

                            temp.name = reader.GetString(0);
                            temp.author = reader.GetString(1);
                            temp.date = reader.GetString(2);
                            temp.image = reader.GetString(3);
                            temp.category = reader.GetString(4);
                            News.Add(temp);
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