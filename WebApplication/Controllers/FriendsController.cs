using System;
using System.Collections.Generic;
using System.IO;
using ApiFriends.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WebApplication.Controllers
{
    public class FriendsController : Controller
    {
        private readonly string _UriAPI = "https://atazureapifriend.azurewebsites.net/api/";
        private readonly string _UriAPICountry = "http://atazureapicountry.azurewebsites.net/api/";

        // GET: FriendsController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friends");
            var response = client.Get<List<Friend>>(request);
            ViewBag.Count = response.Data.Count;

            var request2 = new RestRequest(_UriAPICountry + "Countries");
            var response2 = client.Get<List<Country>>(request2);
            ViewData["Countries"] = response2.Data;

            var request3 = new RestRequest(_UriAPICountry + "States");
            var response3 = client.Get<List<State>>(request3);
            ViewData["States"] = response3.Data;
            return View(response.Data);
        }

        // GET: FriendsController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friends/" + id);
            var response = client.Get<Friend>(request);

            var request2 = new RestRequest(_UriAPI + "Friends/Persons/" + id);
            var persons = client.Get<List<Friend>>(request2);
            ViewData["Persons"] = persons.Data;

            var request3 = new RestRequest(_UriAPI + "FriendShips/" + id);
            var friends = client.Get<List<FriendShip>>(request3);
            ViewData["Friends"] = friends.Data;

            var friendIds = new List<Guid>();
            foreach (var data in friends.Data)
            {
                friendIds.Add(data.FriendId);
            }

            ViewData["FriendIds"] = friendIds;

            return View(response.Data);
        }

        // GET: FriendsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FriendsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Friend model, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Friends/Photo");
                request.AlwaysMultipartFormData = true;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    request.AddFile("file", fileBytes, file.FileName);
                }
                var response = client.Post<string>(request);

                if(!String.IsNullOrEmpty(response.Data))
                {
                    model.PhotoUrl = response.Data;
                    var client2 = new RestClient();
                    var request2 = new RestRequest(_UriAPI + "Friends", DataFormat.Json);
                    request2.AddJsonBody(model);

                    var response2 = client2.Post<Friend>(request2);
                }
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro, por favor tente mais tarde.");
                return View(model);
            }
        }

        // GET: FriendsController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friends/" + id, DataFormat.Json);
            var response = client.Get<Friend>(request);

            return View(response.Data);
        }

        public ActionResult AddFriendShip(Guid userId, Guid friendId)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "FriendShips");
            var model = new FriendShip { UserId = userId, FriendId = friendId };
            request.AddJsonBody(model);
            var response = client.Post<FriendShip>(request);

            return RedirectToAction("Details", "Friends", new { id = userId });
        }

        public ActionResult DeleteFriendShip(Guid userId, Guid friendId)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "FriendShips/" + userId + "/" + friendId);
            var model = new FriendShip { UserId = userId, FriendId = friendId };
            var response = client.Delete<FriendShip>(request);

            return RedirectToAction("Details", "Friends", new { id = userId });
        }

        // POST: FriendsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Friend model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Friends/" + id, DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Put<Friend>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendsController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friends/" + id, DataFormat.Json);
            var response = client.Get<Friend>(request);

            return View(response.Data);
        }

        // POST: FriendsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, Friend model)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Friends/" + id, DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Delete<Friend>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
