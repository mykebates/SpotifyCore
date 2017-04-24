using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SpotifyCore.Models;

namespace SpotifyCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchResults(String search)
        {
            var client = new RestClient("https://api.spotify.com/v1");
            var request = new RestRequest("search", Method.GET);
            request.AddParameter("limit", 3);
            request.AddParameter("type", "artist,track,album,playlist");
            request.AddParameter("q", search);

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

            var asyncClient = client.ExecuteAsync(request, r =>
            {
                taskCompletion.SetResult(r);
            });

            RestResponse response = (RestResponse)(await taskCompletion.Task);

            var searchData = JsonConvert.DeserializeObject<SpotifySearch>(response.Content);

            return View("SearchResults", searchData);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
