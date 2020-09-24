using System;
using System.Collections.Generic;

namespace ApiFriends.Models
{
    public class Friend
    {
        public Guid Id { set; get; }
        public string PhotoUrl { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public DateTime Birthday { set; get; }
        public Country Country { set; get; }
        public Guid CountryId { set; get; }
        public State State { set; get; }
        public Guid StateId { set; get; }
        public List<FriendShip> FriendShips { set; get; }
    }
}
