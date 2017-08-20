using PagedList;
using Polls.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polls.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index(int? page, string catname = "")
        {

            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.Common.ApirUrl + "/api/PublicPoll/GetPublic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            List<MyPolls> pools = null;
            var client_Categories = new RestClient(Common.Common.ApirUrl + "/api/Polls/GetCategories");
            var request_Categories = new RestRequest(Method.GET);
            request_Categories.AddHeader("content-type", "application/json");
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
    }
}