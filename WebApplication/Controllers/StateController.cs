using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiFriends.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WebApplication.Controllers
{
    public class StateController : Controller
    {
        private readonly string _UriAPI = "https://atazureapicountry.azurewebsites.net/api/";

        // GET: StateController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "States");
            var response = client.Get<List<State>>(request);
            return View(response.Data);
        }

        // GET: StateController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
            var response = client.Get<State>(request);
            return View(response.Data);
        }

        // GET: StateController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StateController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(State model, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "States/Photo");
                request.AlwaysMultipartFormData = true;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    request.AddFile("file", fileBytes, file.FileName);
                }
                var response = client.Post<string>(request);
                if (!String.IsNullOrEmpty(response.Data))
                {
                    model.PhotoUrl = response.Data;
                    var client2 = new RestClient();
                    var request2 = new RestRequest(_UriAPI + "States", DataFormat.Json);
                    request2.AddJsonBody(model);

                    var response2 = client.Post<State>(request2);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro, por favor tente mais tarde.");
                return View(model);
            }
        }

        // GET: StateController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
            var response = client.Get<State>(request);

            return View(response.Data);
        }

        // POST: StateController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, State model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Put<State>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StateController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
            var response = client.Get<State>(request);

            return View(response.Data);
        }

        // POST: StateController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, State model)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Delete<State>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
