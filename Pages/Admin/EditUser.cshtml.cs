using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class EditUserModel : PageModel {
    private readonly ILogger<EditUserModel> _logger;

    public EditUserModel(ILogger<EditUserModel> logger)
    {
        _logger = logger;
    }

    [FromQuery]
    public string type { get; set; }

    [FromQuery]
    public string sendEmail { get; set; }

    public List<UserInfo> Users = new List<UserInfo>();

    public IActionResult OnGet() {
        if (type == "delete") {
            try
            {
                String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                    connection.Open();

                    String sql = "DELETE FROM users WHERE email = @email";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@email", sendEmail);
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

                String sql = "SELECT * FROM users";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                    using(NpgsqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            UserInfo temp = new UserInfo();

                            temp.email = reader.GetString(0);
                            temp.password = reader.GetString(1);
                            temp.fullname = reader.GetString(2);
                            temp.birthdate = reader.GetString(3);
                            temp.admin = reader.GetInt32(4);
                            temp.image = reader.GetString(5);

                            Users.Add(temp);
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