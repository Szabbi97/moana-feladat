using Microsoft.AspNetCore.Mvc;
using moana_feladat.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace moana_feladat.Controllers
{
    public class CardController : Controller
    {
        string Baseurl = "http://79.172.201.168/";
        private readonly ILogger<HomeController> _logger;
        public async Task<ActionResult> Details(string id)
        {
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Request.Cookies["moanaToken"] == null)
                {
                    return Redirect("/User/Login");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", Request.Cookies["moanaToken"]);
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("Cards/GetById?id=" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var CardResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    Card card = JsonConvert.DeserializeObject<Card>(CardResponse);
                    return View(card);
                }
                return null;
            }
        }
        public async Task<ActionResult> Edit(string id)
        {
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Request.Cookies["moanaToken"] == null)
                {
                    return Redirect("/User/Login");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", Request.Cookies["moanaToken"]);
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("Cards/GetById?id=" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var CardResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    Card card = JsonConvert.DeserializeObject<Card>(CardResponse);
                    return View(card);
                }
                return null;
            }
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Card inputCard)
        {
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", Request.Cookies["moanaToken"]);
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                object data = new
                {
                    id = inputCard.Id,
                    title = inputCard.Title,
                    description = inputCard.Description,
                    status = inputCard.Status,
                    position = inputCard.Position,
                    asigneeId = inputCard.AsigneeId
                };
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage Res = await client.PutAsync("Cards/Update",byteContent);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    return Redirect("/Card/Details/"+inputCard.Id);
                }
                return null;
            }
        }
        public async Task<ActionResult> Delete(string id)
        {
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", Request.Cookies["moanaToken"]);
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                object data = new
                {
                    id = id,
                };

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(Baseurl + "Cards/Delete"),
                    Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                //Checking the response is successful or not which is sent using HttpClient
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/Home/Index/");
                }
                return null;
            }
        }
    }
}
