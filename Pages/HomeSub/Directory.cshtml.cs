using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class DirectoryModel : PageModel {

    private readonly IPasswordHasher<String> _passwordHasher;
    
    public DirectoryModel(IPasswordHasher<String> passwordHasher) {
        _passwordHasher = passwordHasher;
    }

    [BindProperty]
    public string homeTask { get; set; }

    [BindProperty]
    public string password { get; set; }
    [BindProperty]
    public string fullname { get; set; }
    [BindProperty]
    public string birthdate { get; set; }
    [BindProperty]
    public string email { get; set; }

    public UserInfo temp = new UserInfo();

    public dynamic CUser { get; private set; }

    public void OnGet() {
        CUser = HttpContext.Session.GetObjectFromJson<dynamic>("CurrentUser");

        try {
            String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                connection.Open();

                String sql = "SELECT * FROM users where email = @email";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@email", CUser.email.ToString());
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
            if(homeTask == "goback") {
                Console.WriteLine("Entered");
                return RedirectToPage("../Home");
            }
            if(homeTask == "logout") {
                HttpContext.Session.Clear();
                return RedirectToPage("../Index");
            }
            if(homeTask == "updateUser") {
                var hashedPassword = _passwordHasher.HashPassword(null, password);
                try {
                    String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                        connection.Open();

                        String sql = "UPDATE users SET password = @password, fullname = @fullname, birthdate = @birthdate WHERE email = @email";
                        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@password", hashedPassword);
                            command.Parameters.AddWithValue("@fullname", fullname);
                            command.Parameters.AddWithValue("@birthdate", birthdate);
                            command.Parameters.AddWithValue("@email",email);
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToPage("../Home");
                } catch (Exception ex){
                    Console.WriteLine("Exception: " +ex.ToString());
                }
            }
        } else {
            if(homeTask == "goback") {
                return RedirectToPage("../Home");
            }
        }
        return Page();
    }
}