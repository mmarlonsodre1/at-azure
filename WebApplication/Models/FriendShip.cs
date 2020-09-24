using System;

namespace ApiFriends.Models
{
    public class FriendShip
    {
        public Guid UserId { set; get; }
        public Friend UserOrFriend { set; get; }
        public Guid FriendId { set; get; }
    }
}
