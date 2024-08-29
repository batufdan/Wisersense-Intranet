using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Project.Pages;

public class HomeModel : PageModel
{
    private readonly ILogger<HomeModel> _logger;

    public HomeModel(ILogger<HomeModel> logger)
    {
        _logger = logger;
    }

    public dynamic CUser { get; private set; }

    public List<UserInfo> listUsers = new List<UserInfo>();
    public List<PlanModel> Plans = new List<PlanModel>();

    public string[] months = {"JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"};

    public void OnGet()
    {
        CUser = HttpContext.Session.GetObjectFromJson<dynamic>("CurrentUser");
        
        try {
            String connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=23292329";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString)) {
                connection.Open();

                String usersSql = "SELECT * FROM users";
                using (NpgsqlCommand command = new NpgsqlCommand(usersSql, connection)) {
                    using(NpgsqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            UserInfo userInfo = new UserInfo();

                            userInfo.email = reader.GetString(0);
                            userInfo.password = reader.GetString(1);
                            userInfo.fullname = reader.GetString(2);
                            userInfo.birthdate = reader.GetString(3);
                            userInfo.admin = reader.GetInt32(4);
                            userInfo.image = reader.GetString(5);

                            string[] tempDate = userInfo.birthdate.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            if(DateTime.Now.Month <= int.Parse(tempDate[1])) {
                                if(DateTime.Now.Day <= int.Parse(tempDate[0])) {
                                    listUsers.Add(userInfo);
                                }
                            }
                        }
                    }
                }
            
                String plansSql = "SELECT * FROM plans";
                using (NpgsqlCommand command = new NpgsqlCommand(plansSql, connection)) {
                    using(NpgsqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            PlanModel temp = new PlanModel();

                            temp.name = reader.GetString(0);
                            temp.date = reader.GetString(1);
                            temp.time = reader.GetString(2);
                            temp.type = reader.GetString(3);

                            string[] tempDate = temp.date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            if(DateTime.Now.Month <= int.Parse(tempDate[1])) {
                                if(DateTime.Now.Day <= int.Parse(tempDate[0])) {
                                    Plans.Add(temp);
                                }
                            }
                        }
                    }
                }
            }
            BubbleSort(listUsers);
            BubbleSort(Plans);
        } catch (Exception ex){
            Console.WriteLine("Exception: " +ex.ToString());
        }
    }

    public IActionResult OnPost() {
        HttpContext.Session.Clear();
        return RedirectToPage("Index");
    }

    private void BubbleSort(List<UserInfo> list) {
        int n = list.Count;
        string[] A, B;
    
        for(int i=0; i < n - 1; i++) {
            for(int j=0; j < n - i - 1; j++) {
                A = list[j].birthdate.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                B = list[j+1].birthdate.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                if(int.Parse(A[1]) > int.Parse(B[1])) {
                    UserInfo temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                } else if(int.Parse(A[1]) == int.Parse(B[1])) {
                    if(int.Parse(A[0]) > int.Parse(B[0])) {
                        UserInfo temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }
    }
    private void BubbleSort(List<PlanModel> list) {
        int n = list.Count;
        string[] A, B;
    
        for(int i=0; i < n - 1; i++) {
            for(int j=0; j < n - i - 1; j++) {
                A = list[j].date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                B = list[j+1].date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                if(int.Parse(A[1]) > int.Parse(B[1])) {
                    PlanModel temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                } else if(int.Parse(A[1]) == int.Parse(B[1])) {
                    if(int.Parse(A[0]) > int.Parse(B[0])) {
                        PlanModel temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }
    }
}