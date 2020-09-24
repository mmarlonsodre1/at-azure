using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiFriends.Models;
using ApiFriends.Repository;

namespace ApiFriends.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendShipsController : ControllerBase
    {
        private readonly FriendContext _context;

        public FriendShipsController(FriendContext context)
        {
            _context = context;
        }

        // GET: api/FriendShips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FriendShip>>> GetFriendShip()
        {
            return await _context.FriendShip.Include(x => x.UserOrFriend).ToListAsync();
        }

        // GET: api/FriendShips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FriendShip>>> GetFriendShip(Guid id)
        {
            var friendShip = await _context.FriendShip.Where(x => x.UserId == id).ToListAsync();
        
            if (friendShip == null)
            {
                return NotFound();
            }

            List<FriendShip> friendships = new List<FriendShip>();
            
            foreach (FriendShip u in friendShip)
            {
                try
                {
                    Friend friend = await _context.Friends.Include(x => x.Country).Include(x => x.State).FirstOrDefaultAsync(x => x.Id == u.FriendId);
                    friendships.Add(new FriendShip
                    {
                        UserId = u.UserId,
                        FriendId = u.FriendId,
                        UserOrFriend = friend
                    });
                }
                catch (Exception e) { }
            }

            return friendships;
        }

        // PUT: api/FriendShips/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriendShip(Guid id, FriendShip friendShip)
        {
            if (id != friendShip.UserId)
            {
                return BadRequest();
            }

            _context.Entry(friendShip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendShipExists(id))
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

        // POST: api/FriendShips
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FriendShip>> PostFriendShip(FriendShip friendShip)
        {
            var friendShip2 = new FriendShip { 
                UserId = friendShip.FriendId, 
                UserOrFriend = friendShip.UserOrFriend, 
                FriendId = friendShip.UserId };

            _context.FriendShip.Add(friendShip);
            _context.FriendShip.Add(friendShip2);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FriendShipExists(friendShip.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFriendShip", new { id = friendShip.UserId }, friendShip);
        }

        // DELETE: api/FriendShips/5
        [HttpDelete("{id}/{id2}")]
        public async Task<ActionResult<FriendShip>> DeleteFriendShip(Guid id, Guid id2)
        {
            var friendShip = await _context.FriendShip.FirstOrDefaultAsync(x => x.UserId == id && x.FriendId == id2);
            var friendShip1 = await _context.FriendShip.FirstOrDefaultAsync(x => x.FriendId == id && x.UserId == id2);
            if (friendShip == null)
            {
                return NotFound();
            }

            _context.FriendShip.Remove(friendShip);
            _context.FriendShip.Remove(friendShip1);
            await _context.SaveChangesAsync();

            return friendShip;
        }

        private bool FriendShipExists(Guid id)
        {
            return _context.FriendShip.Any(e => e.UserId == id);
        }
    }
}
