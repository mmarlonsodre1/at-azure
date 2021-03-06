﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCountry.Models;
using ApiCountry.Repository;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.Globalization;

namespace ApiCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly CountryContext _context;
        private string ConnectionString { get; set; }

        public CountriesController(CountryContext context, IConfiguration configuration)
        {
            _context = context;
            ConnectionString = configuration.GetConnectionString("blob"); 
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.Include(x => x.States).ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(Guid id)
        {
            var country = await _context.Countries.Include(x => x.States).FirstOrDefaultAsync(x => x.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(Guid id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }
            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        [HttpPost("Photo")]
        public async Task<ActionResult<string>> PostPhoto(IFormFile file)
        {
            if(file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    BlobContainerClient blobServiceClient = new BlobContainerClient(ConnectionString, "blob");
                    blobServiceClient.CreateIfNotExists();
                    DateTime now = DateTime.UtcNow;
                    var blobClient = blobServiceClient.GetBlobClient($"{now.Ticks}-{file.FileName}");
                    await blobClient.UploadAsync(stream);
                    return blobClient.Uri.ToString();
                }
            }
            return null;
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Country>> DeleteCountry(Guid id)
        {
            var country = await _context.Countries.Include(x => x.States).FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            var a = await _context.SaveChangesAsync();

            return country;
        }

        private bool CountryExists(Guid id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
