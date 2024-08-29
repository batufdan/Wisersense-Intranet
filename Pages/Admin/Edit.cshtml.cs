using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class EditModel : PageModel {
    private readonly ILogger<EditModel> _logger;

    private readonly IPasswordHasher<String> _passwordHasher;

    public EditModel(ILogger<EditModel> logger, IPasswordHasher<String> passwordHasher)
    {
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    [BindProperty]
    public string adminTask { get; set; }

    [FromQuery]
    public string sendEmail { get; set; }

    public UserInfo temp = new UserInfo();

    [BindProperty]
    public string password { get; set; }
    [BindProperty]
    public string fullname { get; set; }
    [BindProperty]
    public string birthdate { get; set; }
    [BindProperty]
    public int admin { get; set; }
    [BindProperty]
    public string image { get; set; }

    public void OnGet() {
        try {
            String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                connection.Open();

                String sql = "SELECT * FROM users where email = @email";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@email", sendEmail);
                    using(NpgsqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            temp.email = reader.GetString(0);
                            temp.password = reader.GetString(1);
                            temp.fullname = reader.GetString(2);
                            temp.birthdate = reader.GetString(3);
                            temp.admin = reader.GetInt32(4);
                            temp.image = reader.GetString(5);
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
            if(adminTask == "updateUser") {
                var hashedPassword = _passwordHasher.HashPassword(null, password);
                try {
                    String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                        connection.Open();

                        String sql = "UPDATE users SET password = @password, fullname = @fullname, birthdate = @birthdate, admin = @admin, image = @image WHERE email = @email";
                        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@password", hashedPassword);
                            command.Parameters.AddWithValue("@fullname", fullname);
                            command.Parameters.AddWithValue("@birthdate", birthdate);
                            command.Parameters.AddWithValue("@admin", admin);
                            command.Parameters.AddWithValue("@image", image);
                            command.Parameters.AddWithValue("@email", sendEmail);
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