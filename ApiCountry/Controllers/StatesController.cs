﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCountry.Models;
using ApiCountry.Repository;
using Azure.Storage.Blobs;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace ApiCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly CountryContext _context;
        private string ConnectionString { get; set; }

        public StatesController(CountryContext context, IConfiguration configuration)
        {
            _context = context;
            ConnectionString = configuration.GetConnectionString("blob");
        }

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetState()
        {
            return await _context.State.ToListAsync();
        }

        // GET: api/States/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(Guid id)
        {
            var state = await _context.State.FindAsync(id);

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        // PUT: api/States/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(Guid id, State state)
        {
            if (id != state.Id)
            {
                return BadRequest();
            }
            _context.Entry(state).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
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

        // POST: api/States
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state)
        {
            _context.State.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetState", new { id = state.Id }, state);
        }

        [HttpPost("Photo")]
        public async Task<ActionResult<string>> PostPhoto(IFormFile file)
        {
            if (file.Length > 0)
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

        // DELETE: api/States/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<State>> DeleteState(Guid id)
        {
            var state = await _context.State.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.State.Remove(state);
            await _context.SaveChangesAsync();

            return state;
        }

        private bool StateExists(Guid id)
        {
            return _context.State.Any(e => e.Id == id);
        }
    }
}
