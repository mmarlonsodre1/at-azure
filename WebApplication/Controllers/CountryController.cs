using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFriends.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WebApplication.Controllers
{
    public class CountryController : Controller
    {
        private readonly string _UriAPI = "https://localhost:44364/api/";

        // GET: CountryController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Countries");
            var response = client.Get<List<Country>>(request);
            return View(response.Data);
        }

        // GET: CountryController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
            var response = client.Get<Country>(request);
            return View(response.Data);
        }

        // GET: CountryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CountryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Country model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Countries", DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Post<Country>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro, por favor tente mais tarde.");
                return View(model);
            }
        }

        // GET: CountryController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
            var response = client.Get<Country>(request);

            return View(response.Data);
        }

        // POST: CountryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Country model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Put<Country>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CountryController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
            var response = client.Get<Country>(request);

            return View(response.Data);
        }

        // POST: CountryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, Country model)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Delete<Country>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
