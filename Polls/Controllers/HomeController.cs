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
        public ActionResult Index(int? page, string catname = "")
        {
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.Common.ApirUrl + "/api/PublicPoll/GetPublic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", loginRespone.token);
            request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            List<MyPolls> pools = null;
            var client_Categories = new RestClient(Common.Common.ApirUrl + "/api/Polls/GetCategories");
            var request_Categories = new RestRequest(Method.GET);
            request_Categories.AddHeader("content-type", "application/json");
            request_Categories.AddHeader("userid", loginRespone.userId);
            request_Categories.AddHeader("token", loginRespone.token);
            IRestResponse<List<GetCategoriesResponse>> response_Categories = client_Categories.Execute<List<GetCategoriesResponse>>(request_Categories);
            if (response_Categories.StatusCode.ToString() == "OK" && response_Categories.Data != null)
            {
                ViewBag.Categories = response_Categories.Data;
            }
            else
            {
                ViewBag.Categories = null;
            }

            if (response.StatusCode.ToString() == "OK")
            {
                if (!string.IsNullOrEmpty(catname))
                    pools = response.Data.Where(x => x.mainCatName.ToLower() == catname.ToLower()).ToList();
                else
                    pools = response.Data.ToList();
            }

            return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }

        public ActionResult GetAllFilterCategories()
        {
            LoginResponse loginRespone = (LoginResponse)Session["UserDetails"];
            var client = new RestClient(Common.Common.ApirUrl + "/api/Polls/GetAllFilterCategories");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("userid", loginRespone.userId);
            request.AddHeader("token", loginRespone.token);

            IRestResponse<List<GetAllFilterbyCategoryResponse>> response = client.Execute<List<GetAllFilterbyCategoryResponse>>(request);

            if (response.StatusCode.ToString() == "OK" && response.Data != null)
            {

                return View("Index", "Home"); // modify as per your need
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Username/Password !!");
            }
            return View(); // modify as per your need
        }

        public ActionResult GetPollResult(int? pollId)
        {
            return View();
        }



    }
}