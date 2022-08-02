using Microsoft.AspNetCore.Mvc;
using moana_feladat.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace moana_feladat.Controllers
{
    public class UserController : Controller
    {
        string Baseurl = "http://79.172.201.168/";
        string token;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        //Hosted web API REST Service base url
        public async Task<ActionResult> All()
        {
            List<User> users = new List<User>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", Request.Cookies["moanaToken"]);
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("/Users/GetAll");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    users = JsonConvert.DeserializeObject<List<User>>(UserResponse);
                }
                //returning the employee list to view
                return View(users);
            }
        }
        /// <summary>
        /// Validating a login on page
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> Validate(LoginModel loginModel)
        {
            using (var client = new HttpClient())
            {
                LoginResponse loginResponse = new LoginResponse();
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                object data = new
                {
                    email = loginModel.Email,
                    password = loginModel.Password
                };
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage Res = await client.PostAsync("/Authentication/SignIn", byteContent);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(UserResponse);

                    var option = new CookieOptions
                    {
                        Expires = loginResponse.ExpiresAt
                    };

                    Response.Cookies.Append("moanaToken", loginResponse.Token, option);
                    Response.Cookies.Append("moanaUserId", loginResponse.UserId, option);

                    return Json(new { status = true, message = "Login Successfull!" });
                }
                else
                {
                    return Json(new { status = false, message = "Login unsuccesful!" });
                }
            }
        }
        /// <summary>
        /// Registering a user
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> Register(LoginModel loginModel)
        {
            using (var client = new HttpClient())
            {
                LoginResponse loginResponse = new LoginResponse();
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                object data = new
                {
                    email = loginModel.Email,
                    password = loginModel.Password
                };
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage Res = await client.PostAsync("/Authentication/SignUp", byteContent);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    return Json(new { status = true, message = "Registration Successfull!" });
                }
                else
                {
                    return Json(new { status = false, message = "Registration unsuccesful!" });
                }
            }
        }
    }
}
