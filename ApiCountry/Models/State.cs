using System;

namespace ApiCountry.Models
{
    public class State
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string PhotoUrl { set; get; }
        public Guid CountryId { set; get; }
        public Country Country { set; get; }
    }
}
