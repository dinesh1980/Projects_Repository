using Newtonsoft.Json;
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
    
    [Route(Name ="Public")]
    public class BookController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", new {catname = "Public" });
        }

        // GET: Book
        [Route("~/{catname}/Index")]
        public ActionResult Index(int? page, string catname = "")
        {

            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.Common.ApirUrl + "PublicPoll/GetPublic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            List<MyPolls> pools = null;
            var client_Categories = new RestClient(Common.Common.ApirUrl + "Polls/GetCategories");
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
                if (!string.IsNullOrEmpty(catname) && catname != "Public")
                    pools = response.Data.Where(x => x.mainCatName.ToLower() == catname.ToLower()).ToList();
                else
                    pools = response.Data.ToList();
            }

            return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }


        [Route("~/{CateName}/GetPollResult")]
        public ActionResult GetPollResult(string pollId,string CateName)
        {

            PollResultViewModel pollResultviewModel = new PollResultViewModel();
            if (pollId == null)
            {
                return RedirectToAction("Index", "Book");
            }
            var client = new RestClient(Common.Common.ApirUrl + "PublicPoll/GetPollResult");
            GetPollRequest requestbody = new GetPollRequest();
            requestbody.viewAll = true;
            requestbody.pollId = Convert.ToInt32(pollId);


            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(requestbody);

            IRestResponse<List<PollResult>> response = client.Execute<List<PollResult>>(request);
            if (response.StatusCode.ToString() == "OK" && response.Data != null)
            {
                pollResultviewModel.PollResults = response.Data.OrderBy(c => c.choice).ToList();
            }

            var client_poll = new RestClient(Common.Common.ApirUrl + "PublicPoll/GetPublic");
            IRestResponse<List<MyPolls>> response_poll = client_poll.Execute<List<MyPolls>>(request);
            if (response_poll.StatusCode.ToString() == "OK" && response_poll.Data != null)
            {
                // pollResultviewModel.myPolls = response_poll.Data;
                var pollresult = response_poll.Data.Where(m => m.pollId == Convert.ToInt32(pollId)).FirstOrDefault();
                pollresult.firstImagePath = Common.Common.ThumbnailBaseUrl + pollresult.firstImagePath;
                pollresult.secondImagePath = Common.Common.ThumbnailBaseUrl + pollresult.secondImagePath;
                pollresult.firstImagePathFull = Common.Common.FullImageBaseUrl + pollresult.firstImagePath;
                pollresult.secondImagePathfull = Common.Common.FullImageBaseUrl + pollresult.secondImagePath;
                pollResultviewModel.myPolls = pollresult;
                return View(pollResultviewModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Username/Password !!");
            }
            return View(); // modify as per your need
        }
        [Route("Public/Viewprofile")]
        public ActionResult Viewprofile(string userId)
        {
            if (userId == "")
            {
                return RedirectToAction("Index");
            }
            ViewPublicProfile obj = new ViewPublicProfile();
            obj.id = userId;
            obj.viewAll = true;
            var client = new RestClient(Common.Common.ApirUrl + "PublicPoll/ViewPublicProfile");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(obj);
            request.AddHeader("content-type", "application/json");
            IRestResponse<ViewPublicProfileResponse> response = client.Execute<ViewPublicProfileResponse>(request);
            ViewPublicProfileResponse profile = new Models.ViewPublicProfileResponse();
            if (response.StatusCode.ToString() == "OK")
            {
               // profile = response.Data;
                profile = JsonConvert.DeserializeObject<ViewPublicProfileResponse>(response.Content);
                
            }

            return View(profile);

        }
    }


}