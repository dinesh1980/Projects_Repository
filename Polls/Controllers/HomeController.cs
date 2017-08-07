using PagedList;
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
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient("http://demo.honeyshyam.com/api/Polls/GetMyAllPolls");
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", loginRespone.token);
            request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            List<MyPolls> pools = null;
           
            if (response.StatusCode.ToString() == "OK")
            {
                
                pools = response.Data.ToList();
            }
            return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }

       
    }
}