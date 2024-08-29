using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class AddUserModel : PageModel {
    private readonly ILogger<AddUserModel> _logger;
    private readonly IPasswordHasher<String> _passwordHasher;
    //private readonly UserService _userService;

    [BindProperty]
    public string adminTask { get; set; }

    // [BindProperty]
    // public IFormFile file { get; set; }

    [BindProperty]
    public string email { get; set; }
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

    public AddUserModel(ILogger<AddUserModel> logger, IPasswordHasher<String> passwordHasher)
    {
        _logger = logger;
        _passwordHasher = passwordHasher;
        //_userService = new UserService();
    }

    public IActionResult OnPost() {
        if(ModelState.IsValid) {
            if(adminTask == "addNewUser") {
                var hashedPassword = _passwordHasher.HashPassword(password, password);
                try
                {
                    String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";
                    using(NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                         connection.Open();
                         String sql = "INSERT INTO users VALUES (@email, @password, @fullname, @birthdate, @admin, @image)";
                         using(NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@email", email);
                            command.Parameters.AddWithValue("@password", hashedPassword);
                            command.Parameters.AddWithValue("@fullname", fullname);
                            command.Parameters.AddWithValue("@birthdate", birthdate);
                            command.Parameters.AddWithValue("@admin", admin);
                            command.Parameters.AddWithValue("@image", image);
                            command.ExecuteNonQuery();
                         }   
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
            }
        } else {
            Console.WriteLine("Invalid Submission!!");
        }
        return Page();
    }

    // public async Task<IActionResult> OnPostAsync() {
    //     if(file == null || file.Length == 0) {
    //         return Page();
    //     }

    //     var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);

    //     using(var stream = new FileStream(filePath, FileMode.Create)) {
    //         await file.CopyToAsync(stream);
    //     }
    //     return Page();
    // }
}