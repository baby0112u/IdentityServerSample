using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ThirdPartyClientMVC.Models;

namespace ThirdPartyClientMVC.Controllers {


    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        [Authorize(Roles = "普通用户,管理员")]
        public IActionResult About()
        {
            return View();
        }

        [Authorize(Policy = "SmithInSomewhere")]
        public IActionResult Privacy() {
            return View();
        }

        public IActionResult Logout() {
            return SignOut("Cookies", "oidc");
        }

        public async Task<IActionResult> CallApi() {
            
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/api/values");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("Json");
        }


        public async Task<IActionResult> CallApi2() {

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/api/values/112");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("Json");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
