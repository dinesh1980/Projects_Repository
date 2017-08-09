using Polls.Filters;
using Polls.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polls.Controllers
{
    [CheckSession]
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewProfile()
        {
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            var client = new RestClient(Common.Common.ApirUrl + "/api/Polls/ViewProfile");
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", loginRespone.token);
            request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("content-type", "application/json");           
            IRestResponse<Profile> response = client.Execute<Profile>(request);

            if (response.StatusCode.ToString() == "OK")
            {

            }

            return View();
        }
    }
}