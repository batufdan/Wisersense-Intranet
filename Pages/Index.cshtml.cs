using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class SigninModel : PageModel {
    private readonly ILogger<SigninModel> _logger;
    private readonly IPasswordHasher<String> _passwordHasher;
    
    public SigninModel(ILogger<SigninModel> logger, IPasswordHasher<String> passwordHasher) {
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public string error = "";

    [BindProperty]
    public UserModel User { get; set; }

    public void OnGet() {
        
    }

    public IActionResult OnPost() {
        if(ModelState.IsValid) {
            if(User.email == null || User.pass == null) {
                error = "Please Enter Login Details !!";
                return Page();
            }
            UserInfo UInfo = new UserInfo();
            try {
                String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                    connection.Open();

                    String sql = "SELECT * FROM users WHERE email = @email";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@email", User.email);
                        using(NpgsqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                UInfo.password = reader.GetString(1);
                                UInfo.email = reader.GetString(0);
                                UInfo.fullname = reader.GetString(2);
                                UInfo.birthdate = reader.GetString(3);
                                UInfo.admin = reader.GetInt32(4);
                                UInfo.image = reader.GetString(5);
                            }
                        }
                    }
                }
                
            } catch (Exception ex){
                Console.WriteLine("Exception: " +ex.ToString());
            }

            if(UInfo.email == User.email) {
                var verificationResult = _passwordHasher.VerifyHashedPassword(null, UInfo.password, User.pass);
                if(verificationResult == PasswordVerificationResult.Success) {
                    if(User.id == 1) {
                        Console.WriteLine("Succesful Login");
                        HttpContext.Session.SetObjectAsJson("CurrentUser", UInfo);
                        return RedirectToPage("Home");
                    } else {
                        if(UInfo.admin == 1) {
                            Console.WriteLine("Succesful Login");
                            HttpContext.Session.SetObjectAsJson("CurrentUser", UInfo);
                            return RedirectToPage("/Admin/Admin");
                        } else {
                            error = "You are not authorized!!";
                        }
                    }
                } else {
                    error = "Wrong Password !!";
                }
                
            } else {
                error = "Invalid Email !!";
            }
            
        }
        return Page();
    }
}