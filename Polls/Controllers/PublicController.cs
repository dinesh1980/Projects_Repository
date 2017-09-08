using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using Polls.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Polls.Controllers
{



    // [Route(Name ="Public")]
    // [RoutePrefix("Public")]
    //  [Route("{action=index}")] //Specifies the Index action as default for this route prefix
    public class PublicController : Controller
    {


        [Route("")]
        [Route("Public")]
        public ActionResult PublicIndex()
        {
            // return Redirect("~/Public/c/catname=All");
            return RedirectToRoute("PublicDefaultPage", new { catname = "All" });

        }

        // GET: Book 

        [Route("Public/c/{catname?}", Name = "PublicDefaultPage")]
        public ActionResult Index(int? page, string catname = "")
        {

            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPublic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            List<MyPolls> pools = null;
            var client_Categories = new RestClient(Common.CommonUtility.ApirUrl + "Polls/GetCategories");
            var request_Categories = new RestRequest(Method.POST);
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
                if (!string.IsNullOrEmpty(catname) && catname != "All" && catname != "catname=All")
                    pools = response.Data.Where(x => x.mainCatName.ToLower() == catname.ToLower()).ToList();
                else
                    pools = response.Data.ToList();
            }

            return View(pools.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }


        [Route("Public/u/{username}")]
        public ActionResult UserName(int? page, string userId = "", string username = "")
        {
            ViewBag.UserName = username;
            var response = GetPublicPolls(page);
            List<MyPolls> pools = null;
            var client_Categories = new RestClient(Common.CommonUtility.ApirUrl + "Polls/GetCategories");
            var request_Categories = new RestRequest(Method.POST);
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

            if (!string.IsNullOrEmpty(userId))
                pools = response.Where(x => x.userId.ToLower() == userId.ToLower()).ToList();
            else
                pools = response.ToList();


            return View(pools.ToPagedList(page ?? 1, 20));
        }

        private List<MyPolls> GetPublicPolls(int? page)
        {
            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            if (page != 0)
            {
                pollsparameter.pageSize = 20;
                pollsparameter.pageNumber = (page ?? 1);
            }

            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPublic");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<MyPolls>> response = client.Execute<List<MyPolls>>(request);
            if (response.StatusCode.ToString() == "OK")
                return response.Data;
            else
                return new List<MyPolls>();
        }


        [Route("Public/GetPollResult")]
        public ActionResult GetPollResult(string pollId)
        {

            PollResultViewModel pollResultviewModel = new PollResultViewModel();
            if (pollId == null)
            {
                // return RedirectToAction("Index", "Book");
                return Redirect("~/Public");
            }
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPollResult");
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

            var client_poll = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/GetPublic");
            IRestResponse<List<MyPolls>> response_poll = client_poll.Execute<List<MyPolls>>(request);
            if (response_poll.StatusCode.ToString() == "OK" && response_poll.Data != null)
            {
                // pollResultviewModel.myPolls = response_poll.Data;
                var pollresult = response_poll.Data.Where(m => m.pollId == Convert.ToInt32(pollId)).FirstOrDefault();
                var image = pollresult.firstImagePath;
                var secimage = pollresult.secondImagePath;
                pollresult.firstImagePath = Common.CommonUtility.ThumbnailBaseUrl + pollresult.firstImagePath;
                pollresult.secondImagePath = Common.CommonUtility.ThumbnailBaseUrl + pollresult.secondImagePath;
                pollresult.firstImagePathFull = Common.CommonUtility.FullImageBaseUrl + image;
                pollresult.secondImagePathfull = Common.CommonUtility.FullImageBaseUrl + secimage;
                pollResultviewModel.myPolls = pollresult;
                return View(pollResultviewModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Username/Password !!");
            }
            return View(); // modify as per your need
        }
        [Route("Public/Viewprofile/{userId}")]
        public ActionResult Viewprofile(string userId)
        {
            if (userId == "")
            {
                return RedirectToAction("Index");
            }
            ViewPublicProfile obj = new ViewPublicProfile();
            obj.id = userId;
            obj.viewAll = true;
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/ViewPublicProfile");
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
            PublicViewProfileViewModel viewmodel = new PublicViewProfileViewModel();
            var list_poll = GetPublicPolls(0);
            ViewBag.count = list_poll.Count();
            viewmodel.MyPolls = list_poll.Take(5).ToList();
            viewmodel.PublicProfileResponse = profile;
            return View(viewmodel);

        }

        [Route("Public/u/All", Name = "PublicUser")]
        public ActionResult PublicUserList(int? page)
        {

            GetMyPollsModel pollsparameter = new GetMyPollsModel();
            pollsparameter.pageSize = 20;
            pollsparameter.pageNumber = (page ?? 1);
            var client = new RestClient(Common.CommonUtility.ApirUrl + "PublicPoll/ListPublicUsers");
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(pollsparameter);
            IRestResponse<List<ViewPublicProfileResponse>> response = client.Execute<List<ViewPublicProfileResponse>>(request);
            List<ViewPublicProfileResponse> profile = new List<ViewPublicProfileResponse>();
            JsonItem<ViewPublicProfileResponse> profiles = new JsonItem<ViewPublicProfileResponse>();
            if (response.StatusCode.ToString() == "OK")
            {
                JObject jobject = JObject.Parse(response.Content);

                //var resultObjects = AllChildren(JObject.Parse(response.Content))
                //.First(c => c.Type == JTokenType.Array && c.Path.Contains(jobject.Path))
                //.Children<JObject>();
                //foreach (var obj in resultObjects)
                //    profile.Add(JsonConvert.DeserializeObject<ViewPublicProfileResponse>(obj.ToString()));
                // profiles = JsonConvert.DeserializeObject<ViewPublicProfileResponse[]>(response.Content);

                profiles = Common.CommonUtility.DoSerialize<JsonItem<ViewPublicProfileResponse>>(response.Content);
                // var myname = DeserializeNames<JsonItemProperty<ViewPublicProfileResponse>(),Content.ToString());
            }

            return View(profiles.item.ToPagedList(pollsparameter.pageNumber, pollsparameter.pageSize));
        }
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
               

    }


}