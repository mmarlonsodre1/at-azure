using System.Collections.Generic;
using ApiFriends.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WebApplication.Controllers
{
    public class FriendsController : Controller
    {
        private readonly string _UriAPI = "https://localhost:44344/api/";

        // GET: FriendsController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friends");
            var response = client.Get<List<Friend>>(request);
            return View(response.Data);
        }

        // GET: FriendsController/Details/5
        public ActionResult Details(int id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friends/" + id, DataFormat.Json);
            var response = client.Get<Friend>(request);
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
        public ActionResult Create(Friend model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Friends", DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Post<Friend>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro, por favor tente mais tarde.");
                return View(model);
            }
        }

        // GET: FriendsController/Edit/5
        public ActionResult Edit(int id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friends/" + id, DataFormat.Json);
            var response = client.Get<Friend>(request);

            return View(response.Data);
        }

        // POST: FriendsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Friend model)
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
        public ActionResult Delete(int id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friends/" + id, DataFormat.Json);
            var response = client.Get<Friend>(request);

            return View(response.Data);
        }

        // POST: FriendsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Friend model)
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
